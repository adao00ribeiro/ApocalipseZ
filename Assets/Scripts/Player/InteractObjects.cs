using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ApocalipseZ
{

    public class InteractObjects : MonoBehaviour, IInteractObjects
    {
        [Tooltip("The distance within which you can pick up item")]
        public float distance = 1.5f;

        private GameObject previousHitObject;
        [SerializeField] private IInteract interact;
        [SerializeField] UiFpsScopeCursorReticles PUiFpsScopeCursorReticles;
        public UiFpsScopeCursorReticles UiFpsScopeCursorReticles
        {
            get
            {
                if (PUiFpsScopeCursorReticles == null)
                {
                    PUiFpsScopeCursorReticles = GameObject.FindObjectOfType<UiFpsScopeCursorReticles>();
                }
                return PUiFpsScopeCursorReticles;
            }
        }
        public LayerMask layer;
        private InputManager PInputManager;
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


        // Update is called once per frame
        public void UpdateInteract()
        {

            RaycastHit hit;


            if (Physics.Raycast(transform.position, transform.forward, out hit, distance, layer))
            {
                interact = hit.collider.gameObject.GetComponent<IInteract>();
                if (interact != null)
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
                    // UiFpsScopeCursorReticles.EnableCursor ( );
                    // UiFpsScopeCursorReticles.SetUseText ( interact.GetTitle ( ) );
                    if (InputManager.GetUse())
                    {
                        interact.CmdInteract();
                        interact = null;
                        //UiFpsScopeCursorReticles.SetUseText ( "" );
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
                    //  UiFpsScopeCursorReticles.DisableCursor ( );
                    // UiFpsScopeCursorReticles.SetUseText ( "" );
                }
                RotationOjects door = hit.collider.gameObject.GetComponent<RotationOjects>();

                if (door != null)
                {
                    if (InputManager.GetUse())
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
                //UiFpsScopeCursorReticles.DisableCursor( );
                //UiFpsScopeCursorReticles.SetUseText ("");
            }
        }



    }
}