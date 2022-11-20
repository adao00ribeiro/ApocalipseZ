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
            if(base.IsServer){
                
            time += Time.deltaTime;

            RaycastHit hit;
            if (Physics.Linecast(lastPosition, transform.position, out hit))
            {
                //HitFXManager.Instance.ApplyFX ( hit );
                IStats stat = hit.collider.GetComponent<IStats>();
                if (stat != null)
                {
                    stat.TakeDamage(Random.Range(damageMinimum, damageMaximum));
                }
               
                base.Despawn();
            }
            lastPosition = transform.position;
            if (time > livingTime)
            {
                 base.Despawn();
            }
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