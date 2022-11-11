using System;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "Data", menuName = "Data/Audio", order = 1)]
public class DataAudio : ScriptableObject
{
    public string Name;
    public AudioClip Audio;
}
