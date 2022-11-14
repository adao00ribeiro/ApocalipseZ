using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace ApocalipseZ
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grenade : NetworkBehaviour
    {
        public float explosionTimer;
        public float explosionForce;

        public GameObject explosionEffects;

        GameObject effects_temp;

        void OnEnable()
        {
            effects_temp = Instantiate(explosionEffects);
            effects_temp.SetActive(false);
            StartCoroutine(Timer(explosionTimer));
        }

        IEnumerator Timer(float explosionTimer)
        {
            yield return new WaitForSeconds(explosionTimer);
            Explosion();
        }

        void Explosion()
        {
            effects_temp.transform.position = transform.position;
            effects_temp.transform.rotation = transform.rotation;
            effects_temp.SetActive(true);
            effects_temp.GetComponent<Explosion>().EnableExplosion();
            Destroy(gameObject);
        }
    }
}
