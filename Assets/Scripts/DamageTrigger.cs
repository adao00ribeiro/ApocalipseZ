using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerStats>().TakeDamage(10);
    }
}
