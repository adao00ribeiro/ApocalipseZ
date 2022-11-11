using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneManager : MonoBehaviour
{
    public GameObject gameobject;
    private float Progress;

    private void Start()
    {
        gameobject = this.gameObject;
        DontDestroyOnLoad(gameObject);
    }
    public void CarregarCena(string nameScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameScene);
    }
    public void CarregarCenaAsync(string nameScene)
    {
        //chama tele de loading
        StartCoroutine(LoadLevelWithBar(nameScene));

    }

    IEnumerator LoadLevelWithBar(string nameScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");

        yield return new WaitForSecondsRealtime(3);
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Single);

        Progress = 0;
        while (!async.isDone)
        {
            Progress = Mathf.Clamp01(async.progress / .9f);
            yield return null;
        }

    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public float PegarProgresso()
    {
        return Progress;
    }
}