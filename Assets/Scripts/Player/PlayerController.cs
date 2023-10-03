using ApocalipseZ;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
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
    public string PlayerName;
    [SerializeField] private FpsPlayer player;
    [SerializeField] private NetworkBehaviour PrefabPlayer;
    // Start is called before the first frame update
    private GameObject[] PrefabCanvasUi;
    [SerializeField] MotionBlur motionBlur;
    [SerializeField] Volume volume;
    public static bool IsInventoryOpen;
    private void Awake()
    {
        PrefabCanvasUi = Resources.LoadAll<GameObject>("UI");

        //  volume = GameObject.Find ( "PostProcessing" ).GetComponent<Volume> ( );
        //  VolumeProfile proflile = volume.sharedProfile;
        // volume.profile.TryGet ( out motionBlur );
    }
    void Update()
    {
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
    public void CmdSpawPlayer(NetworkConnection sender = null)
    {
        PlayerSpawPointsManager playerspaw = GameController.Instance.PlayerSpawPoints;
        Transform novo = playerspaw.GetPointSpaw();
        NetworkBehaviour go = Instantiate(PrefabPlayer, novo.position, Quaternion.identity);
        player = go.GetComponent<FpsPlayer>();
        go.transform.SetParent(this.transform);
        base.Spawn(go.gameObject, sender);
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
