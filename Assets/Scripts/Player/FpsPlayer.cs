using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

namespace ApocalipseZ
{
    //adicionar e conter components

    [RequireComponent(typeof(Moviment))]
    [RequireComponent(typeof(WeaponManager))]
    public class FpsPlayer : NetworkBehaviour, IFpsPlayer
    {


        public event System.Action<FpsPlayer> OnLocalPlayerJoined;

        IMoviment Moviment;
        IWeaponManager WeaponManager;
        IFastItemsManager FastItemsManager;
        public Inventory Inventory;
        IInteractObjects InteractObjects;
        PlayerStats PlayerStats;
        FirstPersonCamera FirstPersonCamera;
        //--------------------------------------------
        public bool isClimbing = true;
        private Vector3 previousPos = new Vector3();

        [SerializeField] private Animator AnimatorController;
        [SerializeField] private Animator AnimatorWeaponHolderController;
        [SerializeField] private GameObject[] mesh;
        public Transform pivohead;
        public Light Lanterna;
        [SyncVar(hook = nameof(PlayerColorChanged))]
        public Color32 playerColor = Color.white;
        // Start is called before the first frame update
        private void Awake()
        {

            Inventory = GetComponent<Inventory>();
            Moviment = GetComponent<Moviment>();
            WeaponManager = GetComponent<WeaponManager>();
            FastItemsManager = GetComponent<FastItemsManager>();
            InteractObjects = transform.Find("Camera & Recoil").GetComponent<InteractObjects>();
            AnimatorController = transform.Find("Ch35_nonPBR").GetComponent<Animator>();
            AnimatorWeaponHolderController = transform.Find("Camera & Recoil/Weapon holder").GetComponent<Animator>();
            PlayerStats = GetComponent<PlayerStats>();
            FirstPersonCamera = transform.Find("Camera & Recoil").GetComponent<FirstPersonCamera>();
        }
        private void Start()
        {
            if (isLocalPlayer)
            {
                FirstPersonCamera.tag = "MainCamera";
            }
            else
            {
                FirstPersonCamera.RemoveAudioListener();
                FirstPersonCamera.GetComponent<Camera>().enabled = false;
            }
        }
        public override void OnStartLocalPlayer()
        {

            /*
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].layer = 7;
            }
            */
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            CanvasFpsPlayer CanvasFpsPlayer = GameObject.FindObjectOfType<CanvasFpsPlayer>();
            CanvasFpsPlayer.SetPlayerStats(PlayerStats);
            CanvasFpsPlayer.SetInventory(Inventory);

            CmdSetupPlayer("player", color);
        }
        void PlayerColorChanged(Color32 _, Color32 newPlayerColor)
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
        }
        [Server]
        public void DroppAllItems()
        {


        }
        [ClientRpc]
        public void TargetRespaw()
        {
            transform.position = PlayerSpawPoints.Instance.GetPointSpaw();
            AnimatorController.Play("Walk");
            FirstPersonCamera.CameraAlive();
            Moviment.EnableCharacterController();
        }

        [Command]
        public void CmdDropAllItems(NetworkConnectionToClient sender = null)
        {

            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            FpsPlayer fpstemp = sender.identity.GetComponent<FpsPlayer>();

            // IContainer containerInventory = fpstemp.GetInventory();
            // IContainer containerFastItems = fpstemp.GetFastItems();
            //
            // containerWeapon.TargetGetContainer(opponentIdentity.connectionToClient, TypeContainer.WEAPONS, containerWeapon.GetContainerTemp());

        }


        [Command]
        public void CmdSetupPlayer(string _name, Color _col)
        {
            playerColor = _col;
        }
        [ClientRpc]
        internal void RpcSpawBullet(SpawBulletTransform spawbulettransform)
        {
            //Instantiate(ScriptableManager.Instance.GetBullet(spawbulettransform.NameBullet), spawbulettransform.Position, spawbulettransform.Rotation);
            // NetworkServer.Spawn ( Instantiate ( ScriptableManager.bullet , spawbulettransform.Position , spawbulettransform.Rotation ));
            //print ("posicao:" +  spawbulettransform.Position + "rotacao" + spawbulettransform.Rotation);
        }

        public override void OnStopLocalPlayer()
        {
            //  Destroy(CanvasFpsPlayer.gameObject);
        }
        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            //  Animation();
            if (PlayerStats.IsDead())
            {
                Moviment.DisableCharacterController();
                FirstPersonCamera.CameraDeath();
                AnimatorController.Play("BlendDeath");
                AnimatorWeaponHolderController.Play("Unhide");
                return;
            }
            Moviment.UpdateMoviment();
            InteractObjects.UpdateInteract();
            FirstPersonCamera.UpdateCamera();
            if (InputManager.GetLanterna())
            {
                Lanterna.enabled = !Lanterna.enabled;
            }
            //  transform.rotation = Quaternion.Euler ( 0 , GameObject.FindObjectOfType<CinemachinePovExtension> ( ).GetStartrotation ( ).x , 0 );
        }

        public void Animation()
        {

            //animatorcontroller
            AnimatorController.SetFloat("Horizontal", InputManager.GetMoviment().x);
            AnimatorController.SetFloat("Vertical", InputManager.GetMoviment().y);
            AnimatorController.SetBool("IsJump", !Moviment.isGrounded());
            AnimatorController.SetBool("IsRun", Moviment.CheckMovement() && InputManager.GetRun());
            AnimatorController.SetBool("IsCrouch", InputManager.GetCrouch());

            if (!PlayerStats.IsDead())
            {
                AnimatorController.SetFloat("SelectDeath", InputManager.GetCrouch() ? 0 : Random.Range(1, 5));
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
        public IMoviment GetMoviment()
        {
            return Moviment;
        }

        public IWeaponManager GetWeaponManager()
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
        [Command]
        public void CmdSpawBullet(SpawBulletTransform spawbulettransform, NetworkConnectionToClient sender = null)
        {
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            FpsPlayer fpstemp = sender.identity.GetComponent<FpsPlayer>();
            //NetworkServer.Spawn ( Instantiate ( ScriptableManager.bullet , spawbulettransform.Position , spawbulettransform.Rotation ) );
            fpstemp.RpcSpawBullet(spawbulettransform);
        }

        public Inventory GetInventory()
        {
            return Inventory;
        }
        #endregion
    }
}