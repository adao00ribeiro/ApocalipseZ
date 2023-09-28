using FishNet.Object;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class EnviroFishNetPlayer : NetworkBehaviour
{
    public bool assignOnStart = true;
    public bool findSceneCamera = true;
    public GameObject Player;
    public Camera PlayerCamera;
    /*
    public void Start()
    {
        // Deactivate if it isn't ours!
        if (!base.IsOwner && !base.IsServer)
        {
            this.enabled = false;
            return;
        }

        if (PlayerCamera == null && findSceneCamera)
            PlayerCamera = Camera.main;

        if (base.IsOwner)
        {
            if (assignOnStart && Player != null && PlayerCamera != null)
                EnviroSkyMgr.instance.AssignAndStart(Player, PlayerCamera);

            Cmd_RequestSeason();
            Cmd_RequestCurrentWeather();
        }
    }
    */
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
        {
            this.enabled = false;
            return;
        }
        if (PlayerCamera == null && findSceneCamera)
            PlayerCamera = Camera.main;

        if (base.IsOwner)
        {
            if (assignOnStart && Player != null && PlayerCamera != null)
                EnviroSkyMgr.instance.AssignAndStart(Player, PlayerCamera);

            Cmd_RequestSeason();
            Cmd_RequestCurrentWeather();
        }
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        this.enabled = false;
    }
    [ServerRpc]
    void Cmd_RequestSeason()
    {
        RpcRequestSeason((int)EnviroSkyMgr.instance.GetCurrentSeason());
    }

    [ObserversRpc]
    void RpcRequestSeason(int season)
    {
        EnviroSkyMgr.instance.ChangeSeason((EnviroSeasons.Seasons)season);
    }

    [ServerRpc]
    void Cmd_RequestCurrentWeather()
    {
        for (int i = 0; i < EnviroSkyMgr.instance.Weather.zones.Count; i++)
        {
            for (int w = 0; w < EnviroSkyMgr.instance.Weather.WeatherPrefabs.Count; w++)
            {
                if (EnviroSkyMgr.instance.Weather.WeatherPrefabs[w] == EnviroSkyMgr.instance.Weather.zones[i].currentActiveZoneWeatherPrefab)
                    RpcRequestCurrentWeather(w, i);
            }
        }
    }

    [ObserversRpc]
    void RpcRequestCurrentWeather(int weather, int zone)
    {
        EnviroSkyMgr.instance.ChangeZoneWeather(zone, weather);
    }
}
