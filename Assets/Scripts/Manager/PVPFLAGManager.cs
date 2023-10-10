using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;

public class PVPFLAGManager : MonoBehaviour,IPvpManager
{
    public Action<PlayerController> OnPlayer;
    public List<PlayerController> arrayPlayercontroller;
    public Transform point;
    public bool IsStart;
    // Start is called before the first frame update
    void Start()
    {
        OnPlayer += OnChangeOnPlayer; ;
    }

    private void OnChangeOnPlayer(PlayerController controller)
    {
        foreach (var item in arrayPlayercontroller)
        {
            if(item == controller){
                return;
            }
        }
        if (InstanceFinder.IsServer)
        {
            controller.transform.position = point.position;
            controller.currentScene = gameObject.scene.handle;
            controller.SpawPlayer();
            arrayPlayercontroller.Add(controller);
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (IsStart)
        {

            foreach (var item in arrayPlayercontroller)
            {

                item.GetPlayer().transform.position = point.position;
            }

            IsStart = false;
        }
    }
}
