using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class UiFastItems : MonoBehaviour
    {
        public List<UISlotItem> FastSlot = new List<UISlotItem>();
        IFpsPlayer player;
        private void OnEnable()
        {
            if (player == null)
            {
                return;
            }
            // player.GetFastItems ( ).CmdGetContainer ( TypeContainer.FASTITEMS );

        }
        public void SetFpsPlayer(IFpsPlayer _player)
        {
            player = _player;

            FastSlot.ForEach((item) =>
            {
                item.SetFpsPlayer(player);
                item.SetContainer(player.GetFastItems());
            });
            player.GetFastItems().OnContainerAltered += UpdateSlotsFastItems; ;

            UpdateSlotsFastItems();
        }
        private void OnDestroy()
        {
            player.GetFastItems().OnContainerAltered -= UpdateSlotsFastItems; ;
        }
        public void UpdateSlotsFastItems()
        {
            for (int i = 0; i < FastSlot.Count; i++)
            {
                FastSlot[i].UpdateSlot();
            }

        }

    }
}