using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace ApocalipseZ
{
    public class BalisticProjectile : NetworkBehaviour
    {

        public float initialVelocity = 1000;
        [HideInInspector]
        public float airResistance = 0.1f;
        private float time;
        private float livingTime = 5f;
        Vector3 lastPosition;
        [Tooltip("Maximal and minimal damage ammounts to apply on target")]
        public int damageMinimum;
        public int damageMaximum;

        private void OnEnable()
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * initialVelocity);

            lastPosition = transform.position;
        }


        private void Update()
        {
            time += Time.deltaTime;

            RaycastHit hit;
            if (Physics.Linecast(lastPosition, transform.position, out hit))
            {
                if (hit.collider.CompareTag("HUD"))
                {
                    return;
                }
                //HitFXManager.Instance.ApplyFX ( hit );

                IStats stat = hit.collider.GetComponent<IStats>();
                if (stat != null)
                {
                    stat.CmdTakeDamage(Random.Range(damageMinimum, damageMaximum));
                }
                NetworkBehaviour.Destroy(gameObject);
            }

            lastPosition = transform.position;

            if (time > livingTime)
            {
                NetworkBehaviour.Destroy(gameObject);
            }
        }


        private void OnDisable()
        {
            time = 0;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = Vector3.zero;
        }
    }

}