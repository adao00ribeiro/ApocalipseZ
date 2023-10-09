using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollisionGround : MonoBehaviour
{

    public GameObject CollisionObject;
    public float GroundedRadius = 0.5f;
    public float GroundedOffset;
    public LayerMask GroundLayers;

    public Vector3 spherePosition;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                   transform.position.z);

        PhysicsScene physicsScene = gameObject.scene.GetPhysicsScene();
        if (physicsScene.Raycast(spherePosition, -Vector3.up, out hit, GroundedRadius))
        {
            CollisionObject = hit.collider.gameObject;
        }
        else
        {
            CollisionObject = null;
        }

    }
    public bool IsGrounded
    {
        get
        {
            return CollisionObject ? true : false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(spherePosition, spherePosition - transform.up * GroundedRadius);
    }
}
