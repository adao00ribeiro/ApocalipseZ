using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandeFunction : MonoBehaviour
{
    public GameObject PrefabGranade;
    public Transform muzzle;
    public void ThrowGrenade(){
         IProjectile go = Instantiate(PrefabGranade, muzzle.position, Quaternion.identity).GetComponent<IProjectile>();
            go.Initialize(muzzle.forward, 0);
    }
}
