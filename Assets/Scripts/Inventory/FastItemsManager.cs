using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace ApocalipseZ
{
    public class FastItemsManager : NetworkBehaviour, IFastItemsManager
    {
        IFpsPlayer player;
        public int switchSlotIndex = 0;

        private InputManager PInputManager;
       
        // Start is called before the first frame update

        void Update ( )
        {
            if (!isLocalPlayer)
            {
                return;
            }
            if ( InputManager.GetAlpha3 ( ) )
            {
                CmdSlotChange ( 0 );
            }
            else if ( InputManager.GetAlpha4 ( ) )
            {
                CmdSlotChange ( 1);
            }
            else if ( InputManager.GetAlpha5 ( ) )
            {
                CmdSlotChange ( 2 );
            }
        }

        #region COMMAND

        [Command]
        public void CmdSlotChange (int slotIndex, NetworkConnectionToClient sender = null )
        {
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            IContainer container = sender.identity.GetComponent<FpsPlayer> ( ).GetContainer( TypeContainer.FASTITEMS);
            if ( container != null)
            {
                container.UseItem ( slotIndex );
            }
         
        }
        #endregion

        #region GET SET
        public InputManager InputManager
        {
            get
            {
                if ( PInputManager == null )
                {
                    PInputManager = InputManager.Instance;
                }
                return PInputManager;
            }
        }
        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
        }
        #endregion
    }
}