using System;
using System.Collections;
using Script.Menus;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    private Image blackScreen;
    [SerializeField] private MainMenuManager mainMenuManager;
    private Color transparent = new Color(0f, 0f, 0f, 0f);
    private Color black = new Color(0f, 0f, 0f, 1f);

    private void OnEnable()
    {
        mainMenuManager.ChangeScene += TransitionEntrance;
    }

    void Start()
    {
        blackScreen = gameObject.GetComponent<Image>();
        blackScreen.color = transparent;
        blackScreen.enabled = false;
        TransitionOut();
    }

    public void TransitionEntrance(SceneAsset sceneToLaunch=null)
    {
        StartCoroutine(Appear(sceneToLaunch));
    }

    public void TransitionOut(SceneAsset sceneToLaunch = null)
    {
        StartCoroutine(Disappear(sceneToLaunch));
    }

    IEnumerator Appear(SceneAsset sceneToLaunch=null)
    {
        blackScreen.enabled = true;
        blackScreen.color = transparent;
        float duration = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration);
            blackScreen.color = Color.Lerp(transparent, black, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (sceneToLaunch != null)
        {
            SceneManager.LoadScene(sceneToLaunch.name);
        }
    }
    IEnumerator Disappear(SceneAsset sceneToLaunch=null)
    {
        blackScreen.enabled = true;
        blackScreen.color = black;
        float duration = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration);
            blackScreen.color = Color.Lerp(black, transparent, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackScreen.enabled = false;

        if (sceneToLaunch != null)
        {
            SceneManager.LoadScene(sceneToLaunch.name);
        }
    }
}
