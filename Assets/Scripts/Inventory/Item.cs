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
        [SerializeField] private Material Outiline;

        SpawObjectsManager spawObjectManager;
        private void OnEnable()
        {
            transform.position += Vector3.up * 2;
        }

        private void Start()
        {
            dataItem = GameController.Instance.DataManager.GetDataItem(this.name);
            Ammo = dataItem.Ammo;
            Outiline = Resources.Load<Material>("MatOutiline");

        }
        public override void OnStartClient()
        {
            base.OnStartClient();
            if (base.IsServer)
            {
                return;
            }
            // Destroy(GetComponent<Rigidbody>());
        }

        public void SetSpawObjectManager(SpawObjectsManager objectsManager){
            spawObjectManager = objectsManager;
        }
        public void SetAmmo(int _ammo)
        {
            Ammo = _ammo;
        }
        public void EndFocus()
        {
            foreach (Transform item in transform)
            {
                MeshRenderer renderer = item.GetComponent<MeshRenderer>();
                Material[] materials = renderer.materials;

                // Adicionar o novo material à lista
                // Por exemplo, se 'newMaterial' é o material que você deseja adicionar:
                Material[] newMaterials = new Material[materials.Length - 1];
                for (int i = 0; i < materials.Length - 1; i++)
                {
                    newMaterials[i] = materials[i];
                }
                // Atribuir a nova lista de materiais de volta ao MeshRenderer
                renderer.materials = newMaterials;
            }
        }

        public string GetTitle()
        {
            return dataItem.Name;
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
               // GameController.Instance.SpawObjectsManager.SpawTimeObject();
                base.Despawn(gameObject);
            }
        }

        public void StartFocus()
        {

            foreach (Transform item in transform)
            {
                MeshRenderer renderer = item.GetComponent<MeshRenderer>();
                Material[] materials = renderer.materials;

                // Adicionar o novo material à lista
                // Por exemplo, se 'newMaterial' é o material que você deseja adicionar:
                Material[] newMaterials = new Material[materials.Length + 1];
                materials.CopyTo(newMaterials, 0);
                newMaterials[materials.Length] = Outiline;

                // Atribuir a nova lista de materiais de volta ao MeshRenderer
                renderer.materials = newMaterials;
            }
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