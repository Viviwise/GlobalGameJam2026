using System;
using System.Collections;
using Script.Menus;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenTransitionCurtain : MonoBehaviour
{
    private Image blackScreen;
    [SerializeField] private MainMenuManager mainMenuManager;
    private Color transparent = new Color(0f, 0f, 0f, 0f);
    private Color black = new Color(0f, 0f, 0f, 1f);
    public float transitionTime = 3f;
    
    public RectTransform rideauGauche;
    public RectTransform rideauDroit;
    private float vitesse = 800f;
    
    Vector2 gaucheOuvert;
    Vector2 gaucheFerme;
    Vector2 droitOuvert;
    Vector2 droitFerme;
    
    void Awake()
    {
        gaucheFerme = rideauGauche.anchoredPosition;
        droitFerme = rideauDroit.anchoredPosition;

        gaucheOuvert = gaucheFerme + Vector2.left * rideauGauche.rect.width;
        droitOuvert = droitFerme + Vector2.right * rideauDroit.rect.width;
    }
    private void OnEnable()
    {
        if (mainMenuManager != null)
        {
            mainMenuManager.ChangeScene += TransitionAppear;
        }
    }

    void Start()
    {
        blackScreen = gameObject.GetComponent<Image>();
        blackScreen.color = transparent;
        blackScreen.enabled = false;
        OuvertureRideaux();
        TransitionDisappear();

    }

    public void TransitionAppear(SceneAsset sceneToLaunch=null)
    {
        StartCoroutine(Appear(sceneToLaunch));
    }

    public void TransitionDisappear(SceneAsset sceneToLaunch = null)
    {
        StartCoroutine(Disappear(sceneToLaunch));
    }

    IEnumerator Appear(SceneAsset sceneToLaunch=null)
    {
        blackScreen.enabled = true;
        blackScreen.color = transparent;
        float duration = transitionTime;
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
        float duration = transitionTime;
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
    
    IEnumerator TransitionScene(string nomScene)
    {
        yield return FermerRideaux();
        SceneManager.LoadScene(nomScene);
    }

    public void OuvertureRideaux()
    {
        StartCoroutine(OuvrirRideaux());
    }
  
    public void FermetureRideaux()
    {
        StartCoroutine(FermerRideaux());
    }

    IEnumerator FermerRideaux()
    {
        while (Vector2.Distance(rideauGauche.anchoredPosition, gaucheFerme) > 1f)
        {
            rideauGauche.anchoredPosition =
                Vector2.MoveTowards(rideauGauche.anchoredPosition, gaucheFerme, vitesse * Time.deltaTime);

            rideauDroit.anchoredPosition =
                Vector2.MoveTowards(rideauDroit.anchoredPosition, droitFerme, vitesse * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator OuvrirRideaux()
    {
        while (Vector2.Distance(rideauGauche.anchoredPosition, gaucheOuvert) > 1f)
        {
            rideauGauche.anchoredPosition =
                Vector2.MoveTowards(rideauGauche.anchoredPosition, gaucheOuvert, vitesse * Time.deltaTime);

            rideauDroit.anchoredPosition =
                Vector2.MoveTowards(rideauDroit.anchoredPosition, droitOuvert, vitesse * Time.deltaTime);

            yield return null;
        }
    }
}
