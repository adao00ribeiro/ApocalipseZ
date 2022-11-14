using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class Explosion : NetworkBehaviour
{

    public float explosionForce;
    public float damageRadius;
    public float damage;
    Collider[] colliders;
    GameObject effects_temp;
    public AudioSource audioSource;
    private void OnEnable()
    {

    }
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
                stats.CmdTakeDamage((int)damage);
            }


            if (collider.GetComponent<Rigidbody>())
            {
                collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
        }
        NetworkBehaviour.Destroy(gameObject, 1);
    }
}
