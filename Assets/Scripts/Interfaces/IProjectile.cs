using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    GameObject gameobject { get; }

    void Initialize(Vector3 direction, float passedTime);
}
