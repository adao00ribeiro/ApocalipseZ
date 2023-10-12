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
        GameController.Instance.TimerManager.Add(() =>
     {
         SpawPointPlayer point = GetPointSpaw(playerController.GetPlayer().tag);
         playerController.GetPlayer().gameObject.transform.position = point.transform.position;
         playerController.GetPlayer().GetMoviment().EnableCharacterController();
         playerController.GetPlayer().GetPlayerStats().AddHealth(200);
         playerController.GetPlayer().GetPlayerStats().AddHydratation(100);
         playerController.GetPlayer().GetPlayerStats().AddSatiety(100);
       
         playerController.GetPlayer().TargetRespaw(playerController.GetPlayer().Owner);
     }, 5);
    }
    internal SpawPointPlayer GetPointSpaw(string tag)
    {
        print(tag);
        SpawPointPlayer point = null;
        if (tag != "TeamA" && tag != "TeamB")
        {
            point = array[Random.Range(0, array.Length)];
        }else
        {
            foreach (var item in array)
            {
                if (item.tag.Contains(tag))
                {
                    point = item;
                    break;
                }
            }
        }

        return point;
    }

}
