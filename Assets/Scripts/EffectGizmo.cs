using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectGizmo : MonoBehaviour
{

    public Vector3 size;
    Vector3 pivo;

    public virtual void OnDrawGizmos()
    {

        pivo.y = -size.y / 2;
        Gizmos.DrawWireCube(transform.position - pivo, size);
    }
}
