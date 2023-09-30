using ApocalipseZ;
using UnityEngine;
public class LobbyManager : MonoBehaviour
{
    /// <summary>
  //  public Animator npcanimator;
    public GameObject model;

    public void SpawCharacter(string nameCharacter)
    {
        if (model)
        {
            Destroy(model);
        }
        DataCharacter cha = GameController.Instance.DataManager.GetDataCharacter(nameCharacter);
        if (cha)
        {
            Transform point = GameController.Instance.PlayerSpawPoints.GetPointSpaw();
            model = Instantiate(cha.PrefabCharacter, point.position, point.rotation);
            //  model.GetComponent<Animator>().runtimeAnimatorController = npcanimator.runtimeAnimatorController;
        }
    }

    public void SelectCharacter(string name)
    {
        GameController.Instance.characterName = name;
        SpawCharacter(name);
    }
    public void SetPlayerName(string playerName)
    {
        GameController.Instance.playerName = playerName;
    }
}
