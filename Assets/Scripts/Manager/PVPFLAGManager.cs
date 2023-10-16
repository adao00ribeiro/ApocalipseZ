using System;
using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using FishNet;
using Unity.VisualScripting;
using UnityEngine;


public class PVPFLAGManager : ConnectionManager, IPvpManager
{
    public Action<string> OnTimeFormat;
    [Header("PVP Setup")]
    [SerializeField] private int TimePreparationMinutes = 3;
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
        currentTime = TimePreparationMinutes * 60;
        StartCoroutine(Preparation());
    }
    IEnumerator Preparation()
    {
        while (currentTime > 0)
        {
            // Exibe o tempo restante no console (você pode atualizar a interface gráfica aqui)
            // Debug.Log("Tempo Restante: " + FormatTime(currentTime));
            OnTimeFormat?.Invoke(FormatTime(currentTime));
            yield return new WaitForSeconds(1.0f);
            currentTime--;
        }
        // O contador chegou a zero
        Debug.Log("Tempo Esgotado!");
        currentTime = MaxTimeGameMinutes * 60;
        IsStart = true;
        StopCoroutine(Preparation());
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

        if (!IsStart)
        {
            return;
        }
        currentTime -= Time.deltaTime;
        if (!IsEnd && currentTime <= 0 || FlagsTeamA == 3 || FlagsTeamB == 3)
        {
            StartCoroutine(Desconecta());
            //desconecta todo mundo
            currentTime = 0;
            IsEnd = true;
        }
        // Debug.Log("Tempo Restante: " + minutes + " minutos " + seconds + " segundos");
        OnTimeFormat?.Invoke(FormatTime(currentTime));
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


    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}


