using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ApocalipseZ
{

    public class InteractObjects : MonoBehaviour, IInteractObjects
    {

        public event Action<bool> OnEnableCursor;
        public event Action<string> OnNameObjectInteract;

        [Tooltip("The distance within which you can pick up item")]
        public float distance = 1.5f;

        private GameObject previousHitObject;
        public LayerMask layer;

        // Update is called once per frame
        public void UpdateInteract()
        {

            RaycastHit hit;


            if (Physics.Raycast(transform.position, transform.forward, out hit, distance, layer))
            {

                if (hit.collider.gameObject.TryGetComponent<IInteract>(out IInteract interact))
                {
                    if (previousHitObject != hit.collider.gameObject)
                    {
                        // O objeto mudou desde o último quadro, então chama o método ExitTrigger
                        if (previousHitObject != null)
                        {
                            IInteract previousInteract = previousHitObject.GetComponent<IInteract>();
                            if (previousInteract != null)
                            {
                                previousInteract.EndFocus();
                            }
                        }
                        // Atualiza o objeto anterior para o objeto atual
                        previousHitObject = hit.collider.gameObject;
                        // Chama o método EnterTrigger
                        interact.StartFocus();
                    }
                    OnEnableCursor?.Invoke(true);
                    OnNameObjectInteract?.Invoke(interact.GetTitle());
                    if (GameController.Instance.InputManager.GetUse())
                    {
                        interact.CmdInteract();
                        interact = null;
                        OnNameObjectInteract?.Invoke("");
                    }

                }
                else
                {
                    // Não foi detectada uma colisão
                    if (previousHitObject != null)
                    {
                        IInteract previousInteract = previousHitObject.GetComponent<IInteract>();
                        if (previousInteract != null)
                        {
                            previousInteract.EndFocus();
                        }

                        // Reseta o objeto anterior
                        previousHitObject = null;
                    }
                    OnEnableCursor?.Invoke(false);
                    OnNameObjectInteract?.Invoke("");
                }
                RotationOjects door = hit.collider.gameObject.GetComponent<RotationOjects>();

                if (door != null)
                {
                    if (GameController.Instance.InputManager.GetUse())
                    {
                        door.OnInteract();
                    }
                }
            }
            else
            {
                // Não foi detectada uma colisão
                if (previousHitObject != null)
                {
                    IInteract previousInteract = previousHitObject.GetComponent<IInteract>();
                    if (previousInteract != null)
                    {
                        previousInteract.EndFocus();
                    }

                    // Reseta o objeto anterior
                    previousHitObject = null;
                }
                OnEnableCursor?.Invoke(false);
                OnNameObjectInteract?.Invoke("");
            }
        }



    }
}