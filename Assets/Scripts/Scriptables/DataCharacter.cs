using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DataCharacter", menuName = "Data/Character", order = 2)]
public class DataCharacter : ScriptableObject
{
    public string Name;
    public GameObject PrefabCharacter;

}
