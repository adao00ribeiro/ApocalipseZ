using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ETypeDraw
{
    CUBE,
    SPHERE
}
public class EffectGizmo : MonoBehaviour
{
    public Color32 color;
    public ETypeDraw typeDraw;
    public float radius;
    public Vector3 size;
    Vector3 pivo;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (typeDraw == ETypeDraw.CUBE)
        {
            pivo.y = -size.y / 2;
            Gizmos.DrawWireCube(transform.position - pivo, size);
        }
        if (typeDraw == ETypeDraw.SPHERE)
        {
            Gizmos.DrawWireSphere(transform.position - pivo, radius);
        }

    }
}
