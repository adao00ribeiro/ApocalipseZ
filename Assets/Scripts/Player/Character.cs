using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform spine;
    public Flag flag;

    public bool testeDropFlag;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (testeDropFlag)
        {
            DropFlag();
            testeDropFlag = false;
        }
    }

    public void SetFlag(Flag _flag)
    {
        _flag.transform.SetParent(spine);
        _flag.transform.localPosition = Vector3.zero;
        _flag.transform.localRotation = Quaternion.identity;
        flag = _flag;
    }
    public void DropFlag()
    {
        flag.transform.SetParent(null);
    }
}
