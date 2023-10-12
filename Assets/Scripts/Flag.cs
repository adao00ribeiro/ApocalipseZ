using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
namespace ApocalipseZ
{
    [RequireComponent(typeof(NetworkTransform))]
    public class Flag : NetworkBehaviour, IInteract
    {
        private PVPFLAGManager manager;
        public Transform pointSpawFlag;
        public bool IsLocalPoint;
        public float timeRespawLocal;
        // Start is called before the first frame update
        void Start()
        {
            manager = GameObject.FindFirstObjectByType<PVPFLAGManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsLocalPoint)
            {
                timeRespawLocal += Time.deltaTime;
                if (timeRespawLocal > 10)
                {
                    ReturnLocalPosition();
                }
            }


        }
        [ServerRpc(RequireOwnership = false)]
        public void CmdInteract(NetworkConnection sender = null)
        {
            FpsPlayer player = sender.FirstObject.GetComponent<PlayerController>().GetPlayer();
            if (player.tag != gameObject.tag)
            {
                OnInteract(player);
                return;
            }
            if (IsLocalPoint)
            {
                ReturnLocalPosition();
            }

        }
       
        public void ReturnLocalPosition()
        {
            transform.SetParent(pointSpawFlag);
            transform.position = pointSpawFlag.position;
            timeRespawLocal = 0;
            IsLocalPoint = true;
        }
        public void EndFocus()
        {

        }

        public string GetTitle()
        {
            return "Flag";
        }
        [ObserversRpc]
        public void ObserverInteract(GameObject player){
             GetComponent<Rigidbody>().useGravity = false;
                player.GetComponent<FpsPlayer>().SetFlag(this);
        }   
        public void OnInteract(IFpsPlayer player)
        {
            GetComponent<Rigidbody>().useGravity = false;
            player.SetFlag(this);
            ObserverInteract(player.gameobject);
        }

        public void StartFocus()
        {

        }


    }
}