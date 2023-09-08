using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace ApocalipseZ
{
    public class FastItemsManager : NetworkBehaviour, IFastItemsManager
    {
        IFpsPlayer player;
        public int switchSlotIndex = 0;

        private InputManager PInputManager;

        // Start is called before the first frame update

        void Update()
        {
            if (!IsOwner)
            {
                return;
            }
            if (InputManager.GetAlpha3())
            {
                CmdSlotChange(0);
            }
            else if (InputManager.GetAlpha4())
            {
                CmdSlotChange(1);
            }
            else if (InputManager.GetAlpha5())
            {
                CmdSlotChange(2);
            }
        }

        #region COMMAND

        [ServerRpc]
        public void CmdSlotChange(int slotIndex, NetworkConnection sender = null)
        {



        }
        #endregion

        #region GET SET
        public InputManager InputManager
        {
            get
            {
                if (PInputManager == null)
                {
                    PInputManager = GameController.Instance.InputManager;
                }
                return PInputManager;
            }
        }
        public void SetFpsPlayer(IFpsPlayer _player)
        {
            player = _player;
        }
        #endregion
    }
}