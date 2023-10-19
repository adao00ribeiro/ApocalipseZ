using TMPro;
using UnityEngine;


public class UIPvpFlag : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private PVPFLAGManager manager;


    [SerializeField] private TextMeshProUGUI textPointTeamA;
    [SerializeField] private TextMeshProUGUI textPointTeamB;
    // Start is called before the first frame update
    void Start()
    {
        manager.OnTimeFormat += PVPFLAGManager_OnSetTimetText; ;
        manager.OnPointsFlagsTeamA += PVPFLAGManager_OnPointsFlagsTeamA; ;
        manager.OnPointsFlagsTeamB += PVPFLAGManager_OnPointsFlagsTeamB; ;
    }

    private void PVPFLAGManager_OnPointsFlagsTeamB(int obj)
    {
        textPointTeamA.text = obj.ToString();
    }

    private void PVPFLAGManager_OnPointsFlagsTeamA(int obj)
    {
        textPointTeamB.text = obj.ToString();
    }

    private void PVPFLAGManager_OnSetTimetText(string obj)
    {
        TimeText.text = obj;
    }
}
