using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace ApocalipseZ
{

    [RequireComponent(typeof(NetworkTransform))]
    public class Item : NetworkBehaviour, IInteract
    {
        [SerializeField] private DataItem dataItem;
        private int Ammo;
        [SerializeField] private int dropQuantity;

        public bool IsServerSpaw = false;
        private void OnEnable()
        {
            transform.position += Vector3.up * 2;
        }
        private void Awake()
        {
            Ammo = dataItem.Ammo;
        }
        [Server]
        void Start()
        {
            if (!IsServerSpaw)
            {
                NetworkBehaviour.Destroy(gameObject, 30);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void SetAmmo(int _ammo)
        {
            Ammo = _ammo;
        }
        public void EndFocus()
        {
            print("end focus");
        }

        public string GetTitle()
        {
            return dataItem.name;
        }


        public void OnInteract(IFpsPlayer player)
        {
            IContainer inventory = player.GetInventory();
            SSlotInventory slot = new SSlotInventory();
            slot.SetSItem(dataItem);
            slot.SetAmmo(Ammo);
            slot.SetQuantity(dropQuantity);
            Vector3 point = transform.position;
            if (inventory.AddItem(slot))
            {

                DataAudio audioPickup = GameController.Instance.DataManager.GetDataAudio("Pickup");
                GameController.Instance.SoundManager.PlayOneShot(transform.position, audioPickup.Audio);
                if (IsServerSpaw)
                {
                    GameController.Instance.TimerManager.Add(() =>
                    {

                        // SpawObjects.Spawn(ScriptableManager.Instance.GetPrefab(dataItem.sitem.Type), point);
                    }, 4);
                }
                NetworkBehaviour.Destroy(gameObject);
            }
        }

        public void StartFocus()
        {
            print("StartFocus");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("noCollider"))
            {
                StartFocus();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("noCollider"))
            {
                EndFocus();
            }
        }

        [Command(requiresAuthority = false)]
        public void CmdInteract(NetworkConnectionToClient sender = null)
        {
            OnInteract(sender.identity.GetComponent<FpsPlayer>());
        }
    }
}