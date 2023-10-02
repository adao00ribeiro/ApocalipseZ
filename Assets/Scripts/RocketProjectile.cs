using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;

namespace ApocalipseZ
{

    public class RocketProjectile : MonoBehaviour, IProjectile
    {
        public GameObject PrefabEffectExplosion;


        [SerializeField] private int damage;
        public float initialVelocity = 5;
        [HideInInspector]
        public float airResistance = 0.1f;

        private float time;

        private float livingTime = 5f;

        Vector3 lastPosition;

        public GameObject gameobject => throw new System.NotImplementedException();

        private Vector3 _direction;
        private float _passedTime = 0f;
        private void OnEnable()
        {
            // GetComponent<Rigidbody>().AddForce(transform.forward * initialVelocity);
            lastPosition = transform.position;
        }
        public void Initialize(Vector3 direction, float passedTime, int _damage)
        {
            _direction = direction;
            _passedTime = passedTime;

            damage = _damage;
        }

        private void Update()
        {
            time += Time.deltaTime;

            RaycastHit hit;
            if (Physics.Linecast(lastPosition, transform.position, out hit))
            {
                // HitFXManager.Instance.ApplyFX ( hit );
                PlayerStats stat = hit.collider.GetComponent<PlayerStats>();
                if (stat)
                {
                    stat.TakeDamage(damage);
                }
                Explosion ex = Instantiate(PrefabEffectExplosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
                ex.EnableExplosion();
                InstanceFinder.ServerManager.Spawn(ex.gameObject);
                Destroy(gameObject, 2.5f);
                enabled = false;
            }

            lastPosition = transform.position;

            if (time > livingTime)
            {
                Explosion ex = Instantiate(PrefabEffectExplosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
                ex.EnableExplosion();
                InstanceFinder.ServerManager.Spawn(ex.gameObject);
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