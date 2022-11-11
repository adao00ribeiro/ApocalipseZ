using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEnemy
{
    ZOMBIE
}

[CreateAssetMenu(fileName = "Data", menuName = "Data/Enemy", order = 1)]
public class DataEnemy : ScriptableObject
{
    public string Name;
    public TypeEnemy Type;
    public GameObject Prefab;
}