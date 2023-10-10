using System;
using ApocalipseZ;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : NetworkBehaviour
{
    [Header("Ui Components")]
    [SerializeField] private UiPrimaryAndSecondWeapons uiPrimaryAndSecondWeapons;
    [SerializeField] private UiInventory uiInventory;
    [SerializeField] private UiFastItems uiFastItems;
    [SerializeField] private UiFpsScopeCursorReticles uiFpsScopeCursorReticles;
    [SerializeField] private UiPlayerStats uiPlayerStats;
    [SerializeField] private UiDeadStats uiDeadStats;

    [SyncVar]
    public int currentScene;
    public int CurrentScene
    {
        get
        {
            return currentScene = gameObject.scene.handle;

        }
    }
    [SyncVar]
    public string PlayerName;
    [SerializeField] private FpsPlayer player;
    [SerializeField] private NetworkBehaviour PrefabPlayer;
    // Start is called before the first frame update
    private GameObject[] PrefabCanvasUi;
    [SerializeField] MotionBlur motionBlur;
    [SerializeField] Volume volume;
    public static bool IsInventoryOpen;
    [SyncVar]
    public bool IsLoading;


    [SerializeField] bool IsChecked;
    private void Awake()
    {
        PrefabCanvasUi = Resources.LoadAll<GameObject>("UI");

        //  volume = GameObject.Find ( "PostProcessing" ).GetComponent<Volume> ( );
        //  VolumeProfile proflile = volume.sharedProfile;
        // volume.profile.TryGet ( out motionBlur );
    }
    void Update()
    {

        if (player == null)
        {
            return;
        }
        if (GameController.Instance.InputManager.GetInventory() && !player.GetPlayerStats().IsDead())
        {
            IsInventoryOpen = !IsInventoryOpen;
            WeaponManager.IsChekInventory = true;
            UiInventory.Canvas.enabled = IsInventoryOpen;
            UiPrimaryAndSecondWeapons.Canvas.enabled = IsInventoryOpen;
            UiFastItems.Canvas.enabled = IsInventoryOpen;
            ActiveMotionBlur(IsInventoryOpen);
        }
        if (GameController.Instance.InputManager.GetEsc())
        {
            IsInventoryOpen = false;
            WeaponManager.IsChekInventory = true;
            UiInventory.Canvas.enabled = IsInventoryOpen;
            UiPrimaryAndSecondWeapons.Canvas.enabled = IsInventoryOpen;
            UiFastItems.Canvas.enabled = IsInventoryOpen;
            ActiveMotionBlur(IsInventoryOpen);
        }

        if (base.IsOwner)
        {
            if (GameController.Instance.InputManager.GetPvpFlag())
            {
                print("PVP APERTADO");
                if (!IsChecked)
                {
                    CmdAddWaitinLine();
                    IsChecked = true;
                }
                else
                {
                   CmdRemoveWaitinLine();
                    IsChecked = false;
                }

            }
        }

    }
    [ServerRpc]
    public void CmdAddWaitinLine(){
        
          GameController.Instance.PvpManager.AddWaitinLine(base.Owner);
    }
     [ServerRpc]
    public void CmdRemoveWaitinLine(){
           GameController.Instance.PvpManager.RemoveWaitinLine(base.Owner);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            //ChatUI.localPlayerName = playerName;
            CmdSpawPlayer();
            CmdSetPlayerName(GameController.Instance.playerName);
            GameController.Instance.playerController = this;

        }

    }
    [ServerRpc]
    public void CmdSetPlayerName(string playerName)
    {
        PlayerName = playerName;
    }
    [ServerRpc]
    public void CmdSpawPlayer()
    {
        SpawPlayer();
    }
    public void SpawPlayer()
    {
        PlayerSpawPointsManager playerspaw = GameController.Instance.GetPlayerSpawPointManager(CurrentScene);
        SpawPointPlayer point = playerspaw.GetPointSpaw();
        NetworkBehaviour go = Instantiate(PrefabPlayer, point.transform.position, Quaternion.identity);
        player = go.GetComponent<FpsPlayer>();
        go.transform.SetParent(this.transform);
        base.Spawn(go.gameObject, base.Owner);
        ObserverSpawPlayer(go.gameObject);
    }
   
    [ObserversRpc]
    public void ObserverSpawPlayer(GameObject player)
    {
        player.transform.SetParent(this.transform);

        if (base.IsOwner)
        {
            GameController.Instance.playerController.player = player.GetComponent<FpsPlayer>();
            foreach (GameObject item in PrefabCanvasUi)
            {
                Instantiate(item, player.transform);
            }
        }
        else
        {
            player.tag = "Flash";
        }
    }
    public FpsPlayer GetPlayer()
    {
        return player;
    }

    public void ActiveMotionBlur(bool active)
    {
        player.GetFirstPersonCamera().ActiveCursor(active);
        // motionBlur.active = active;
        //  Time.timeScale = active ? 0 : 1;
    }

    internal void DespawnPlayer()
    {
        base.Despawn(player.gameObject);
    }

    public UiPrimaryAndSecondWeapons UiPrimaryAndSecondWeapons
    {
        get
        {
            if (uiPrimaryAndSecondWeapons == null)
            {
                uiPrimaryAndSecondWeapons = GameObject.FindAnyObjectByType<UiPrimaryAndSecondWeapons>();
            }
            return uiPrimaryAndSecondWeapons;
        }
    }
    public UiInventory UiInventory
    {
        get
        {
            if (uiInventory == null)
            {
                uiInventory = GameObject.FindAnyObjectByType<UiInventory>();
            }
            return uiInventory;
        }
    }
    public UiFastItems UiFastItems
    {
        get
        {
            if (uiFastItems == null)
            {
                uiFastItems = GameObject.FindAnyObjectByType<UiFastItems>();
            }
            return uiFastItems;
        }
    }
    public UiFpsScopeCursorReticles UiFpsScopeCursorReticles
    {
        get
        {
            if (uiFpsScopeCursorReticles == null)
            {
                uiFpsScopeCursorReticles = GameObject.FindAnyObjectByType<UiFpsScopeCursorReticles>();
            }
            return uiFpsScopeCursorReticles;
        }
    }
    public UiPlayerStats UiPlayerStats
    {
        get
        {
            if (uiPlayerStats == null)
            {
                uiPlayerStats = GameObject.FindAnyObjectByType<UiPlayerStats>();
            }
            return uiPlayerStats;
        }
    }

    public UiDeadStats UiDeadStats
    {
        get
        {
            if (uiDeadStats == null)
            {
                uiDeadStats = GameObject.FindAnyObjectByType<UiDeadStats>();
            }
            return uiDeadStats;
        }
    }


}
