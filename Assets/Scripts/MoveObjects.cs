using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace ApocalipseZ
{

    public class MoveObjects : NetworkBehaviour, IInteract
    {
        public string nameObject;
        public float speed;
        public bool IsOpen;

        [SerializeField] private Vector3 PosicaoAberto;
        [SerializeField] private Vector3 PosicaoFechado;


        private void FixedUpdate()
        {
            if (IsOpen)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, PosicaoAberto, speed * Time.deltaTime);
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, PosicaoFechado, speed * Time.deltaTime);
            }

        }


        [ServerRpc(RequireOwnership = false)]
        public void CmdInteract(NetworkConnection sender = null)
        {
            // OnInteract ( sender.identity.GetComponent<FpsPlayer> ( ) );
        }

        public void EndFocus()
        {
            throw new System.NotImplementedException();
        }

        public string GetTitle()
        {
            return nameObject;
        }

        public void OnInteract(IFpsPlayer player)
        {
            IsOpen = !IsOpen;

            if (IsOpen)
            {

            }
            else
            {

            }
        }

        public void StartFocus()
        {
            throw new System.NotImplementedException();
        }

    }
}
