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
    [RequireComponent(typeof(NetworkTransform))]
    public class RotationOjects : NetworkBehaviour, IInteract
    {
        public string nameObject;
        public float speed;
        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(IsOpenChanged))]

        public bool IsOpen;

        public AudioClip OpenClip;
        public AudioClip CloseClip;
        void IsOpenChanged(bool _, bool newIsOpen, bool asServer)
        {
            /*
            if ( newIsOpen )
            {
                Sound.Instance.PlayOneShot ( transform.position , OpenClip );
            }
            else
            {
                Sound.Instance.PlayOneShot ( transform.position , CloseClip );
            }
           */
        }

        [SerializeField] private Vector3 QuaternionAberto;
        [SerializeField] private Vector3 QuaternionFechado;


        void FixedUpdate()
        {
            if (IsOwner)
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

        }

        [ServerRpc(RequireOwnership = false)]
        public void CmdInteract(NetworkConnection sender = null)
        {
            // OnInteract(sender.identity.GetComponent<FpsPlayer>());
        }

        public void OnInteract(IFpsPlayer player)
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
    }
}