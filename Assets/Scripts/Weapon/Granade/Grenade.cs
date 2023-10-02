using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace ApocalipseZ
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grenade : NetworkBehaviour, IProjectile
    {
        private int damage;
        public float throwForce;
        public float explosionTimer;
        public float explosionForce;

        public GameObject explosionEffects;

        GameObject effects_temp;
        [SerializeField] private Rigidbody rd;
        private Vector3 _direction;

        private float _passedTime = 0f;

        public GameObject gameobject { get => this.gameObject; }

        void Awake()
        {
            rd = GetComponent<Rigidbody>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Destroy(GetComponent<Rigidbody>());
        }
        void OnEnable()
        {
            if (base.IsClient)
            {
                return;
            }
            StartCoroutine(Timer(explosionTimer));


        }
        public void Initialize(Vector3 direction, float passedTime, int _damage)
        {
            damage = _damage;
            _direction = direction;
            _passedTime = passedTime;
            rd.AddForce(_direction * throwForce);
        }
        IEnumerator Timer(float explosionTimer)
        {
            yield return new WaitForSeconds(explosionTimer);
            Explosion();
        }

        void Explosion()
        {
            effects_temp = Instantiate(explosionEffects);
            effects_temp.transform.position = transform.position;
            effects_temp.transform.rotation = transform.rotation;
            effects_temp.GetComponent<Explosion>().damage = damage;
            effects_temp.GetComponent<Explosion>().EnableExplosion();
            base.Spawn(effects_temp);
            base.Despawn(gameObject);
        }

    }
}
