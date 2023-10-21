using System.Collections.Generic;
using System.Linq;
using ApocalipseZ;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
public class PlayerSpawPointsManager : MonoBehaviour
{
    [SerializeField] private SpawPointPlayer[] array;
    public int currentSceneHandle;
    public string currentSceneName;
    void OnEnable()
    {
        currentSceneHandle = gameObject.scene.handle;
        currentSceneName = gameObject.scene.name;
        array = GameObject.FindObjectsByType<SpawPointPlayer>(FindObjectsSortMode.None);
        GameController.Instance.AddPlayerSpawPointManager(this);
    }
    void OnDisable()
    {

        GameController.Instance.RemovePlayerSpawPointManager(this);
    }
    // Start is called before the first frame update
    public void RespawPlayer(PlayerController playerController)
    {
        playerController.GetPlayer().DropFlag();

        SpawPointPlayer point = null;
        if (playerController.GetPlayer().tag == "TeamA" || playerController.GetPlayer().tag == "TeamB")
        {
             point   = GetPointSpawPvpFlag(playerController.GetPlayer().tag);
        }else{
            point   = GetPointSpaw();
        }
        GameController.Instance.TimerManager.Add(() =>
    {
        playerController.GetPlayer().gameObject.transform.position = point.transform.position;
        playerController.GetPlayer().GetMoviment().EnableCharacterController();
        playerController.GetPlayer().GetPlayerStats().AddHealth(200);
        playerController.GetPlayer().GetPlayerStats().AddHydratation(100);
        playerController.GetPlayer().GetPlayerStats().AddSatiety(100);
        playerController.GetPlayer().TargetRespaw(playerController.GetPlayer().Owner);
    }, 5);

    }
    public SpawPointPlayer GetPointSpaw()
    {
        return array[Random.Range(0, array.Length)];
    }
    internal SpawPointPlayer GetPointSpawPvpFlag(string tag)
    {
        SpawPointPlayer point = null;
        foreach (var item in array)
        {
            if (item.tag.Contains(tag))
            {
                point = item;
                break;
            }
        }
        return point;
    }

}
