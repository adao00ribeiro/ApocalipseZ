using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishNet.Component.Transforming;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using FishNet.Object;
using FishNet.Connection;

namespace ApocalipseZ
{
    public class RotationOjects : MonoBehaviour
    {
        private DoorManager DoorManager;
        public int guidid;
        public string nameObject;
        public float speed;
        public bool IsOpen;
        public AudioClip OpenClip;
        public AudioClip CloseClip;

        [SerializeField] private Vector3 QuaternionAberto;
        [SerializeField] private Vector3 QuaternionFechado;


        void FixedUpdate()
        {
            if (IsOpen)
            {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(QuaternionAberto), speed * Time.deltaTime);
            }
            else
            {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(QuaternionFechado), speed * Time.deltaTime);
            }
        }

        public void CmdInteract(NetworkConnection sender = null)
        {
            // print("interacao");
            // OnInteract(sender.identity.GetComponent<FpsPlayer>());
        }
        public void CmdInteract()
        {
            print("interacao");
            // OnInteract(sender.identity.GetComponent<FpsPlayer>());
        }
        public void OnInteract()
        {
            DoorManager.CmdInteract(guidid);
        }
        public void Open()
        {
            IsOpen = !IsOpen;
        }
        public void StartFocus()
        {
            throw new System.NotImplementedException();
        }

        public void EndFocus()
        {
            throw new System.NotImplementedException();
        }

        public string GetTitle()
        {
            return nameObject;

        }
        public void SetDoorManager(DoorManager manager)
        {
            DoorManager = manager;
        }
    }
}