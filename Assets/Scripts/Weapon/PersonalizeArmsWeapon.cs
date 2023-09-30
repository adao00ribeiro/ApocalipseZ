using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalizeArmsWeapon : MonoBehaviour
{
    public GameObject[] Arms;

    public void ActiveArms(string name)
    {
        DisableAllArms();
        foreach (var item in Arms)
        {
            if (item.name.Contains(name))
            {
                item.gameObject.SetActive(true);
            }
        }
    }
    public void DisableAllArms()
    {
        foreach (var item in Arms)
        {
            item.gameObject.SetActive(false);
        }
    }
}
