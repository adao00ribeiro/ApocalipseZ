using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public abstract class UIAbstract : MonoBehaviour
{
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Canvas Canvas
    {
        get
        {
            if (canvas == null)
            {
                canvas = gameObject.GetComponentInParent<Canvas>();
            }
            return canvas;
        }
    }
}
