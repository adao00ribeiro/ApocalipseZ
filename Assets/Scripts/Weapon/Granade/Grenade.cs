using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace ApocalipseZ
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grenade : NetworkBehaviour, IProjectile
    {

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
        void OnEnable()
        {
            StartCoroutine(Timer(explosionTimer));
        }
        public void Initialize(Vector3 direction, float passedTime)
        {
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
            effects_temp.GetComponent<Explosion>().EnableExplosion();
            base.Spawn(effects_temp);
            Destroy(gameObject);
        }

    }
}
