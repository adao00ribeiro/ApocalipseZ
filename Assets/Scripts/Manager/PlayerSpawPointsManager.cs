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



        GameController.Instance.TimerManager.Add(() =>
     {
         playerController.GetPlayer().GetMoviment().EnableCharacterController();
         SpawPointPlayer point = GetPointSpaw(playerController.GetPlayer().tag);
         transform.position = point.transform.position;
         playerController.GetPlayer().GetPlayerStats().AddHealth(200);
         playerController.GetPlayer().GetPlayerStats().AddHydratation(100);
         playerController.GetPlayer().GetPlayerStats().AddSatiety(100);
         playerController.GetPlayer().TargetRespaw(playerController.GetPlayer().Owner);
     }, 5);
    }
    internal SpawPointPlayer GetPointSpaw(string tag)
    {
        SpawPointPlayer point = null;
        if (tag != "TeamA" && tag != "TeamB")
        {
            point = array[Random.Range(0, array.Length)];
        }
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
