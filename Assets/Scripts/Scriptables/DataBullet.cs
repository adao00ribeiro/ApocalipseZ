using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Bullet", order = 1)]
public class DataBullet : ScriptableObject
{
    public string Name;
    public GameObject Bullet;
}
