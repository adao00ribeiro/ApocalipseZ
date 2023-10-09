using System.Collections.Generic;
using ApocalipseZ;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
public class PlayerSpawPointsManager : MonoBehaviour
{
    [SerializeField] private SpawPointPlayer[] array;
    public int currentScene;
    void OnEnable()
    {
        currentScene = gameObject.scene.handle;
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
         SpawPointPlayer point = GameObject.FindAnyObjectByType<PlayerSpawPointsManager>().GetPointSpaw();
         transform.position = point.transform.position;
         playerController.GetPlayer().GetPlayerStats().AddHealth(200);
         playerController.GetPlayer().GetPlayerStats().AddHydratation(100);
         playerController.GetPlayer().GetPlayerStats().AddSatiety(100);
         playerController.GetPlayer().TargetRespaw(playerController.Owner);
     }, 5);
    }
    internal SpawPointPlayer GetPointSpaw()
    {
        SpawPointPlayer point = array[Random.Range(0, array.Length)];
        return point;
    }

}
