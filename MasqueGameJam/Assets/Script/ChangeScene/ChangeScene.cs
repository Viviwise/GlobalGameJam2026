using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTest : MonoBehaviour
{
    public string sceneToLoad;

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoad);
        Debug.Log(sceneToLoad);
    }
}