using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{


    public class ExitPlayer : EffectGizmo
    {

        SceneManager scene;
        void Start()
        {
            scene = GameController.Instance.SceneManager;
        }
        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            base.OnDrawGizmos();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<IFpsPlayer>().GetFirstPersonCamera().ActiveCursor(true);
                scene.CarregarCenaAsync("Missao");
            }
        }
    }
}