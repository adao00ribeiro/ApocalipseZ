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

        }
        private void OnDestroy()
        {
            // player.GetFastItems().OnContainerAltered -= UpdateSlotsFastItems; ;
        }
        public void UpdateSlotsFastItems()
        {


        }

    }
}