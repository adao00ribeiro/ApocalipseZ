using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;

namespace ApocalipseZ
{
    public class BalisticProjectile : MonoBehaviour , IProjectile
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

        private Vector3 _direction;

        private float _passedTime = 0f;

      
        private void OnEnable()
        {

            lastPosition = transform.position;
        }

        public void Initialize(Vector3 direction, float passedTime)
        {
            _direction = direction;
            _passedTime = passedTime;
        }


        private void Update()
        {
           
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
                    print("colissao");
                    Destroy(gameObject);
                }
                lastPosition = transform.position;
                if (time > livingTime)
                {
                      print("colissao livingtime");
                    Destroy(gameObject);
                }
            
            Move();
        }

        private void Move()
        {
            //Frame delta, nothing unusual here.
            float delta = Time.deltaTime;

            //See if to add on additional delta to consume passed time.
            float passedTimeDelta = 0f;
            if (_passedTime > 0f)
            {
                /* Rather than use a flat catch up rate the
                 * extra delta will be based on how much passed time
                 * remains. This means the projectile will accelerate
                 * faster at the beginning and slower at the end.
                 * If a flat rate was used then the projectile
                 * would accelerate at a constant rate, then abruptly
                 * change to normal move rate. This is similar to using
                 * a smooth damp. */

                /* Apply 8% of the step per frame. You can adjust
                 * this number to whatever feels good. */
                float step = (_passedTime * 0.08f);
                _passedTime -= step;

                /* If the remaining time is less than half a delta then
                 * just append it onto the step. The change won't be noticeable. */
                if (_passedTime <= (delta / 2f))
                {
                    step += _passedTime;
                    _passedTime = 0f;
                }
                passedTimeDelta = step;
            }

            //Move the projectile using moverate, delta, and passed time delta.
            transform.position += _direction * (initialVelocity * (delta + passedTimeDelta));
        }
        private void OnDisable()
        {
            time = 0;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = Vector3.zero;
        }
    }

}