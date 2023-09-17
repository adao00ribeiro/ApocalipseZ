using UnityEngine;
using ApocalipseZ;
using UnityEditor.Animations;
public class LobbyManager : MonoBehaviour
{
    /// <summary>
    public AnimatorController npcanimator;
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
                model = Instantiate(cha.PrefabCharacter ,point.position,point.rotation);
                model.GetComponent<Animator>().runtimeAnimatorController = npcanimator;
        }
    }

    public void SelectCharacter(string name)
    {
        PlayerPrefs.SetString("NamePlayer", name);
        SpawCharacter(name);
    }

}
