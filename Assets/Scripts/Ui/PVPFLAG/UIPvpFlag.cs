using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPvpFlag : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private PVPFLAGManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager.OnTimeFormat += PVPFLAGManager_OnSetTimetText; ;
    }

    private void PVPFLAGManager_OnSetTimetText(string obj)
    {
        TimeText.text = obj;
    }
}
