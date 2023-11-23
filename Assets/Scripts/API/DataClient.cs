using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class DataClient
{
    public UserLogin UserLogin { get; set; }
    public User User;

    public List<string> ListCharacter;
}
