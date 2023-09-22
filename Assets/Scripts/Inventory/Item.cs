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

        private void Start()
        {
            dataItem = GameController.Instance.DataManager.GetDataItem(this.name);
            Ammo = dataItem.Ammo;
        }
        public override void OnStartClient()
        {
            base.OnStartClient();
            if (base.IsServer)
            {
                return;
            }
            Destroy(GetComponent<Rigidbody>());
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
            Inventory inventory = player.GetInventory();
            SlotInventoryTemp slot = new SlotInventoryTemp();
            slot.Name = dataItem.Name;
            slot.guidid = dataItem.GuidId;
            slot.Ammo = Ammo;
            slot.Quantity = dropQuantity;
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

        [ServerRpc(RequireOwnership = false)]
        public void CmdInteract(NetworkConnection sender = null)
        {
            PlayerController playerController = sender.FirstObject.GetComponent<PlayerController>();
            OnInteract(playerController.GetPlayer());
        }

        public void CmdInteract()
        {
            throw new NotImplementedException();
        }
    }
}