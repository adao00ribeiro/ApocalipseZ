using ApocalipseZ;
using UnityEngine;
public class LobbyManager : MonoBehaviour
{
    /// <summary>
  //  public Animator npcanimator;
    public GameObject model;


    void Start()
    {
       
    }



    public void SpawCharacter(string nameCharacter)
    {
        if (model)
        {
            Destroy(model);
        }
        DataCharacter cha = GameController.Instance.DataManager.GetDataCharacter(nameCharacter);
        if (cha)
        {
            SpawPointPlayer point = GameController.Instance.GetPlayerSpawPointManager(gameObject.scene.handle).GetPointSpaw();
            model = Instantiate(cha.PrefabCharacter, point.transform.position, point.transform.rotation);
            //  model.GetComponent<Animator>().runtimeAnimatorController = npcanimator.runtimeAnimatorController;
        }
    }

    public void SelectCharacter(string name)
    {
        GameController.Instance.user.personagemSelecionado = name;
        SpawCharacter(name);
    }

}
