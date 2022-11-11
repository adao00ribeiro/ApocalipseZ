using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollisionGround : MonoBehaviour
{

    public GameObject CollisionObject;
    public float radius = 0.5f;
   
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if ( Physics.Raycast ( transform.position + new Vector3(0,0.5f,0) , -transform.up , out hit , radius ) )
        {
            CollisionObject = hit.collider.gameObject;
        }
        else
        {
            CollisionObject = null;
        }
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine ( transform.position + new Vector3 ( 0 , 0.5f , 0 ) , transform.position -transform.up * radius);
    }
}
