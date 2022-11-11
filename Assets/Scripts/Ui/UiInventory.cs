using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class UiInventory : MonoBehaviour
    {
        public UISlotItem PrefabSlot;
        [SerializeField] private List<UISlotItem> UIItems = new List<UISlotItem>();
        private Transform slotPanel;
        IFpsPlayer player;

        private void OnEnable()
        {
            if (player == null)
            {
                return;
            }
            // player.GetInventory ().CmdGetContainer( TypeContainer.INVENTORY );
        }

        public void SetFpsPlayer(IFpsPlayer _player)
        {
            player = _player;

            slotPanel = transform.Find("SlotPanel").transform;
            AddSlots();
            player.GetInventory().OnContainerAltered += UpdateSlots; ;
            UpdateSlots();

        }
        private void OnDestroy()
        {
            player.GetInventory().OnContainerAltered -= UpdateSlots; ;
        }
        public void UpdateSlots()
        {
            if (player == null)
            {
                return;
            }
            for (int i = 0; i < UIItems.Count; i++)
            {
                UIItems[i].UpdateSlot();
            }

        }

        public void AddSlots()
        {
            foreach (UISlotItem item in UIItems)
            {
                Destroy(item.gameObject);
            }
            UIItems.Clear();
            for (int i = 0; i < player.GetInventory().GetMaxSlots(); i++)
            {
                UISlotItem instance = Instantiate(PrefabSlot, slotPanel);
                instance.SetFpsPlayer(player);
                instance.SetContainer(player.GetInventory());
                instance.SetSlotIndex(i);
                UIItems.Add(instance);
            }
        }
    }
}