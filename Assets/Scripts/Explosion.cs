using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : NetworkBehaviour
{

    public float explosionForce;
    public float damageRadius;
    public float damage;
    Collider[] colliders;
    public AudioSource audioSource;


    public void EnableExplosion()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        transform.LookAt(Camera.main.transform.position);
        Explosions();
    }

    void Explosions()
    {
      
        colliders = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider collider in colliders)
        {

            IStats stats = collider.GetComponent<IStats>();

            if (stats != null)
            {
                stats.TakeDamage((int)damage);
            }


            if (collider.GetComponent<Rigidbody>())
            {
                print(collider.gameObject.name);
                collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
        }

        Invoke("Destroy", 2);
    }

    public void Destroy()
    {
        base.Despawn(gameObject);
    }
}
