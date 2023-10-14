using System;
using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet;
using Unity.VisualScripting;
using UnityEngine;


public class PVPFLAGManager : ConnectionManager, IPvpManager
{
    [Header("PVP Setup")]
    [SerializeField] private int MaxTimeGameMinutes = 15;
    [SerializeField] private float currentTime;

    [Header("Team Points")]
    [SerializeField] private int FlagsTeamA;
    [SerializeField] private int FlagsTeamB;

    [Header("Spaw Setup")]


    public SpawPointPlayer[] TeamA;
    private int indexA;
    public SpawPointPlayer RespawPointTeamA;
    public SpawPointPlayer[] TeamB;
    private int indexB;
    public SpawPointPlayer RespawPointTeamB;
    public bool IsStart;

    public bool IsEnd;
    // Start is called before the first frame update
    void Start()
    {
        OnPlayer += OnChangeOnPlayer; ;

        currentTime = MaxTimeGameMinutes * 60;
    }

    private void OnChangeOnPlayer(int clientId, PlayerController controller)
    {
        if (players.ContainsKey(clientId))
        {
            return;
        }

        if (InstanceFinder.IsServer)
        {

            SpawPointPlayer point;
            if (players.Count % 2 == 0)
            {
                point = TeamA[indexA];
                indexA++;
                tag = "TeamA";
            }
            else
            {
                point = TeamB[indexB];
                indexB++;
                tag = "TeamB";
            }
            controller.SpawPlayerPvp(point.transform, tag);
            players.Add(clientId, controller);
        }

    }
    private void Update()
    {

        currentTime -= Time.deltaTime;


        if (!IsEnd && currentTime <= 0 || FlagsTeamA == 3 || FlagsTeamB == 3)
        {


            StartCoroutine(Desconecta());
            //desconecta todo mundo
            currentTime = 0;
            IsEnd = true;
        }


        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);


        // Debug.Log("Tempo Restante: " + minutes + " minutos " + seconds + " segundos");
    }

    IEnumerator Desconecta()
    {
        yield return new WaitForSeconds(10f);

        foreach (var item in players.Values)
        {
            item.DespawnPlayer();
            GameController.Instance.SceneManager.LoadSceneC(item.Owner.FirstObject);
        }
        yield return new WaitForEndOfFrame();
    }
    public void IncrementFlagTeamA()
    {
        FlagsTeamA++;
    }
    public void IncrementFlagTeamB()
    {
        FlagsTeamB++;
    }
}


