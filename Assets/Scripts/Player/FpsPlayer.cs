using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using FishNet.Connection;
using TMPro;
using FishNet.Object.Prediction;
using FishNet;
using FishNet.Managing.Timing;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using FishNet.Component.Animating;

namespace ApocalipseZ
{
    //adicionar e conter components
    public struct MoveData : IReplicateData
    {
        public bool Jump;
        public float Horizontal;
        public float Forward;
        public bool IsRun;
        public bool IsCrouch;
        public float RotationX;
        public Vector2 MouseDelta;
        private uint _tick;

        public void Dispose() { }
        public uint GetTick() => _tick;
        public void SetTick(uint value) => _tick = value;
    }
    public struct ReconcileData : IReconcileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float VerticalVelocity;
        public bool Grounded;
        private uint _tick;

        public ReconcileData(Vector3 position, Quaternion rotation, float velocity, bool grounded)
        {
            Position = position;
            Rotation = rotation;
            VerticalVelocity = velocity;
            Grounded = grounded;
            _tick = 0;
        }
        public void Dispose() { }
        public uint GetTick() => _tick;
        public void SetTick(uint value) => _tick = value;
    }
    [RequireComponent(typeof(Moviment))]
    [RequireComponent(typeof(WeaponManager))]
    public class FpsPlayer : NetworkBehaviour, IFpsPlayer
    {

        Moviment Moviment;
        WeaponManager WeaponManager;
        IFastItemsManager FastItemsManager;
        public Inventory Inventory;
        IInteractObjects InteractObjects;
        PlayerStats PlayerStats;
        public FirstPersonCamera FirstPersonCamera;
        //--------------------------------------------
        public bool isClimbing = true;
        private Vector3 previousPos = new Vector3();

        [SerializeField] private Animator AnimatorController;
        [SerializeField] private Animator AnimatorWeaponHolderController;
        [SerializeField] private GameObject[] mesh;
        public Transform pivohead;
        public Light Lanterna;
        [SerializeField] MeshRenderer meshteste;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(PlayerColorChanged))]
        public Color32 playerColor = Color.white;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(CharacterChanged))]
        public string CharacterName = "";
        MoveData md = new();
        ReconcileData rd = new();

        private NetworkAnimator networkAnimator;
        //MoveData for client simulation

        // Start is called before the first frame update
        private void Awake()
        {
            networkAnimator = GetComponent<NetworkAnimator>();
            Inventory = GetComponent<Inventory>();
            Moviment = GetComponent<Moviment>();
            WeaponManager = GetComponent<WeaponManager>();
            FastItemsManager = GetComponent<FastItemsManager>();
            InteractObjects = transform.Find("Recoil/Camera & Recoil").GetComponent<InteractObjects>();
            AnimatorWeaponHolderController = transform.Find("Recoil/Camera & Recoil/Weapon holder").GetComponent<Animator>();
            PlayerStats = GetComponent<PlayerStats>();
            FirstPersonCamera = transform.Find("Recoil/Camera & Recoil").GetComponent<FirstPersonCamera>();
            WeaponManager.SetFpsPlayer(this);
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            base.TimeManager.OnTick += TimeManager_OnTick;
            base.TimeManager.OnUpdate += TimeManager_OnUpdate;
            base.TimeManager.OnPostTick += TimeManager_OnPostTick;

        }
        public override void OnStopNetwork()
        {
            base.OnStopNetwork();

            if (base.TimeManager != null)
            {
                base.TimeManager.OnTick -= TimeManager_OnTick;

                base.TimeManager.OnPostTick -= TimeManager_OnPostTick;
            }
        }
        private void LateUpdate()
        {
            if (!base.IsOwner || PlayerStats.IsDead())
            {
                return;
            }
            FirstPersonCamera.UpdateCamera();
        }

        private void TimeManager_OnTick()
        {
            Move(BuildMoveData());
        }
        private void TimeManager_OnPostTick()
        {

            if (IsServer)
            {
                rd.Position = transform.position;
                rd.Rotation = transform.rotation;
                rd.VerticalVelocity = Moviment.PlayerVelocity.y;
                rd.Grounded = Moviment.isGrounded();
                Reconciliation(rd);
            }

        }
        private void TimeManager_OnUpdate()
        {
            if (base.IsOwner)
            {
            
                if (PlayerStats.Disable)
                {
                    return;
                }
                    Animation();
                if (PlayerStats.IsDead())
                {
                    Moviment.DisableCharacterController();
                    FirstPersonCamera.CameraDeath();
                    AnimatorWeaponHolderController.SetBool("HideWeapon", true);
                    CmdRespawn();
                    return;
                }
                InteractObjects.UpdateInteract();
                if (InputManager.GetLanterna())
                {
                    Lanterna.enabled = !Lanterna.enabled;
                }

            }
        }
        [ServerRpc]
        private void CmdRespawn()
        {

            GameController.Instance.TimerManager.Add(() =>
            {
                transform.position = GameController.Instance.PlayerSpawPoints.GetPointSpaw().position;
                PlayerStats.AddHealth(200);
                PlayerStats.AddHydratation(100);
                PlayerStats.AddSatiety(100);
                TargetRespaw(base.Owner);
            }, 5);
        }


        private MoveData BuildMoveData()
        {
            if (!base.IsOwner)
            {
                return default;
            }
            md.Jump = InputManager.GetIsJump();
            md.Forward = InputManager.GetMoviment().y;
            md.Horizontal = InputManager.GetMoviment().x;
            md.IsRun = InputManager.GetRun();
            md.IsCrouch = InputManager.GetCrouch();
            md.MouseDelta.x = InputManager.GetMouseDelta().x;
            md.MouseDelta.y = InputManager.GetMouseDelta().y;
            md.RotationX = FirstPersonCamera.GetRotationX();
            return md;
        }

        [ReplicateV2]
        private void Move(MoveData md, ReplicateState state = ReplicateState.Invalid, Channel channel = Channel.Unreliable)
        {
            Moviment.MoveTick(md, (float)base.TimeManager.TickDelta);
            Moviment.GravityJumpUpdate(md.Jump, (float)base.TimeManager.TickDelta);
        }
        [ReconcileV2]
        private void Reconciliation(ReconcileData rd, Channel channel = Channel.Unreliable)
        {
            //Reset the client to the received position. It's okay to do this
            //even if there is no de-synchronization.
            transform.position = rd.Position;
            transform.rotation = rd.Rotation;
            Moviment.PlayerVelocity.y = rd.VerticalVelocity;
            Moviment.SetIsGround(rd.Grounded);
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
            //playerName = (string)connectionToClient.authenticationData;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (base.IsOwner)
            {
                FirstPersonCamera.tag = "MainCamera";
                FirstPersonCamera.GetComponent<Camera>().enabled = true;
                FirstPersonCamera.ActiveCursor(false);
                CanvasFpsPlayer CanvasFpsPlayer = GameObject.FindObjectOfType<CanvasFpsPlayer>();
                CanvasFpsPlayer.SetFirtPersonCamera(FirstPersonCamera);
                CanvasFpsPlayer.SetPlayerStats(PlayerStats);
                CmdSpawCharacter(PlayerPrefs.GetString("NamePlayer"));
            }
            else
            {
                FirstPersonCamera.RemoveAudioListener();
            }

            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));


            CmdSetupPlayer("player", color);
        }
        [ServerRpc(RequireOwnership = false)]
        void CmdSpawCharacter(string nameCharacter)
        {
            CharacterName = nameCharacter;
        }

        void SpawCharacter(string nameCharacter)
        {
            DataCharacter cha = GameController.Instance.DataManager.GetDataCharacter(nameCharacter);
            if (cha)
            {
                GameObject go = Instantiate(cha.PrefabCharacter, transform.GetChild(0).transform);
                meshteste = go.GetComponent<MeshRenderer>();
                AnimatorController = go.GetComponent<Animator>();
                networkAnimator.SetAnimator(AnimatorController);
            }
        }
        void PlayerColorChanged(Color32 _, Color32 newPlayerColor, bool asServer)
        {
            /*
        for (int i = 0; i < mesh.Length; i++)
        {
            Material[] mats = mesh[i].GetComponent<SkinnedMeshRenderer>().materials;

            for (int j = 0; j < mats.Length; j++)
            {
                mats[j].color = newPlayerColor;
            }
        }
        */

            if (meshteste)
            {
                meshteste.material.color = newPlayerColor;
            }
        }
        void CharacterChanged(string _, string newPlayerColor, bool asServer)
        {
            /*
        for (int i = 0; i < mesh.Length; i++)
        {
            Material[] mats = mesh[i].GetComponent<SkinnedMeshRenderer>().materials;

            for (int j = 0; j < mats.Length; j++)
            {
                mats[j].color = newPlayerColor;
            }
        }
        */
            if (asServer)
            {
                return;

            }
            SpawCharacter(newPlayerColor);

        }
        [Server]
        public void DroppAllItems()
        {


        }
        [TargetRpc]
        public void TargetRespaw(NetworkConnection conn)
        {

            AnimatorController.Play("Walk");
            FirstPersonCamera.CameraAlive();
            Moviment.EnableCharacterController();
            AnimatorWeaponHolderController.SetBool("HideWeapon", false);

        }

        [ServerRpc]
        public void CmdDropAllItems(NetworkConnection sender = null)
        {


            // IContainer containerInventory = fpstemp.GetInventory();
            // IContainer containerFastItems = fpstemp.GetFastItems();
            //
            // containerWeapon.TargetGetContainer(opponentIdentity.connectionToClient, TypeContainer.WEAPONS, containerWeapon.GetContainerTemp());

        }


        [ServerRpc]
        public void CmdSetupPlayer(string _name, Color _col)
        {
            playerColor = _col;
        }
        [ObserversRpc]
        internal void RpcSpawBullet(SpawBulletTransform spawbulettransform)
        {
            //Instantiate(ScriptableManager.Instance.GetBullet(spawbulettransform.NameBullet), spawbulettransform.Position, spawbulettransform.Rotation);
            // NetworkServer.Spawn ( Instantiate ( ScriptableManager.bullet , spawbulettransform.Position , spawbulettransform.Rotation ));
            //print ("posicao:" +  spawbulettransform.Position + "rotacao" + spawbulettransform.Rotation);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            //  Destroy(CanvasFpsPlayer.gameObject);
        }
        // Update is called once per frame


        public void Animation()
        {
            if (AnimatorController == null)
            {
                return;
            }
            //animatorcontroller
            AnimatorController.SetFloat("Horizontal", InputManager.GetMoviment().x);
            AnimatorController.SetFloat("Vertical", InputManager.GetMoviment().y);
            AnimatorController.SetBool("IsJump", !Moviment.isGrounded());
            AnimatorController.SetBool("IsRun", Moviment.CheckMovement() && InputManager.GetRun());
            AnimatorController.SetBool("IsCrouch", InputManager.GetCrouch());

            if (PlayerStats.IsDead())
            {
                AnimatorController.SetFloat("SelectDeath", InputManager.GetCrouch() ? 0 : Random.Range(1, 5));
                AnimatorController.SetTrigger("IsDeath");
            }

            AnimatorWeaponHolderController.SetBool("Walk", Moviment.CheckMovement() && Moviment.isGrounded() && !PlayerStats.IsDead());
            AnimatorWeaponHolderController.SetBool("Run", Moviment.CheckMovement() && InputManager.GetRun() && Moviment.isGrounded() && !PlayerStats.IsDead());
            AnimatorWeaponHolderController.SetBool("Crouch", Moviment.CheckMovement() && InputManager.GetCrouch() && Moviment.isGrounded() && !PlayerStats.IsDead());

        }

        public float GetVelocityMagnitude()
        {
            var velocity = ((transform.position - previousPos).magnitude) / Time.deltaTime;
            previousPos = transform.position;
            return velocity;
        }
        public Moviment GetMoviment()
        {
            return Moviment;
        }

        public WeaponManager GetWeaponManager()
        {
            return WeaponManager;
        }
        public PlayerStats GetPlayerStats()
        {
            return PlayerStats;
        }

        public FirstPersonCamera GetFirstPersonCamera()
        {
            return FirstPersonCamera;
        }
        private InputManager PInputManager;


        public InputManager InputManager
        {
            get
            {
                if (PInputManager == null)
                {
                    PInputManager = GameController.Instance.InputManager;
                }
                return PInputManager;
            }
        }


        #region command
        [ServerRpc]
        public void CmdSpawBullet(SpawBulletTransform spawbulettransform, NetworkConnection sender = null)
        {

        }

        public Inventory GetInventory()
        {
            return Inventory;
        }
        #endregion
    }
}