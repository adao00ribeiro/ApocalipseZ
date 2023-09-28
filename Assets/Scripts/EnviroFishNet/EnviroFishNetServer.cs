using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;


[RequireComponent(typeof(NetworkObject))]
public class EnviroFishNetServer : NetworkBehaviour
{
    public float updateSmoothing = 15f;

    [SyncVar] private float networkHours;
    [SyncVar] private int networkDays;
    [SyncVar] private int networkYears;

    public bool isHeadless = true;


    public override void OnStartServer()
    {
        base.OnStartServer();
        if (isHeadless)
        {
            EnviroSkyMgr.instance.StartAsServer();
        }

        EnviroSkyMgr.instance.SetAutoWeatherUpdates(true);

        EnviroSkyMgr.instance.OnSeasonChanged += (EnviroSeasons.Seasons season) =>
        {
            SendSeasonToClient(season);
        };
        EnviroSkyMgr.instance.OnZoneWeatherChanged += (EnviroWeatherPreset type, EnviroZone zone) =>
        {
            SendWeatherToClient(type, zone);
        };
    }
    public void Start()
    {
        if (!base.IsServer)
        {
            EnviroSkyMgr.instance.SetTimeProgress(EnviroTime.TimeProgressMode.None);
            EnviroSkyMgr.instance.SetAutoWeatherUpdates(false);
        }
    }
    void SendWeatherToClient(EnviroWeatherPreset w, EnviroZone z)
    {
        int zoneID = 0;

        for (int i = 0; i < EnviroSkyMgr.instance.GetZoneList().Count; i++)
        {
            if (EnviroSkyMgr.instance.GetZoneList()[i] == z)
                zoneID = i;

        }

        for (int i = 0; i < EnviroSkyMgr.instance.GetCurrentWeatherPresetList().Count; i++)
        {

            if (EnviroSkyMgr.instance.GetCurrentWeatherPresetList()[i] == w)
                RpcWeatherUpdate(i, zoneID);
        }
    }

    void SendSeasonToClient(EnviroSeasons.Seasons s)
    {
        RpcSeasonUpdate((int)s);
    }
    [ObserversRpc]
    void RpcSeasonUpdate(int season)
    {
        EnviroSkyMgr.instance.ChangeSeason((EnviroSeasons.Seasons)season);
    }

    [ObserversRpc]
    void RpcWeatherUpdate(int weather, int zone)
    {
        EnviroSkyMgr.instance.ChangeZoneWeather(zone, weather);
    }
    void Update()
    {
        if (EnviroSkyMgr.instance == null)
            return;

        if (!base.IsServer)
        {
            if (networkHours < 1f && EnviroSkyMgr.instance.GetUniversalTimeOfDay() > 23f)
                EnviroSkyMgr.instance.SetTimeOfDay(networkHours);

            EnviroSkyMgr.instance.SetTimeOfDay(Mathf.Lerp(EnviroSkyMgr.instance.GetUniversalTimeOfDay(), (float)networkHours, Time.deltaTime * updateSmoothing));
            EnviroSkyMgr.instance.SetYears(networkYears);
            EnviroSkyMgr.instance.SetDays(networkDays);

        }
        else
        {
            networkHours = EnviroSkyMgr.instance.GetUniversalTimeOfDay();
            networkDays = EnviroSkyMgr.instance.GetCurrentDay();
            networkYears = EnviroSkyMgr.instance.GetCurrentYear();
        }

    }
}
