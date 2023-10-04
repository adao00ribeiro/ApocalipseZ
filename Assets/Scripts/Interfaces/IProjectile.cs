using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    GameObject gameobject { get; }

    void Initialize(float passedTime, int damage);
}
