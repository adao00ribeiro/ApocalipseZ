using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgAnimation : MonoBehaviour
{
    public GameObject Rocket;
  public void ActiveRocket ( )
    {
        Rocket.SetActive ( true);
    }

    public void DisableRocket ( )
    {
        Rocket.SetActive ( false );
    }
}
