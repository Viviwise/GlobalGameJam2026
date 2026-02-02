using UnityEngine;
using TMPro;
using System.Collections;

public class FrameMove : MonoBehaviour
{
    public RectTransform cadreAnnonceScene;
    public TMP_Text sceneNameText;      
    private ScenarioManager scenarioManager; 

    Vector2 cadreHaut;
    Vector2 cadreBas;
    public float vitesse = 5f;

    private void Awake()
    {
        cadreHaut = cadreAnnonceScene.anchoredPosition;
        float hauteurCadre = cadreAnnonceScene.rect.height;
        cadreBas = cadreHaut + Vector2.down * hauteurCadre;

        scenarioManager = FindObjectOfType<ScenarioManager>();
        
/*        if (scenarioManager != null && scenarioManager.currentScene!= null)
        {
            sceneNameText.text = scenarioManager.currentScene.sceneName;
        }
        */
    }

    private void Start()
    {
        StartCoroutine(BaisserCadre());
    }

    IEnumerator BaisserCadre()
    {
        while (Vector2.Distance(cadreAnnonceScene.anchoredPosition, cadreBas) > 0.5f)
        {
            cadreAnnonceScene.anchoredPosition =
                Vector2.MoveTowards(cadreAnnonceScene.anchoredPosition, cadreBas, vitesse * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MonterCadre()
    {
        while (Vector2.Distance(cadreAnnonceScene.anchoredPosition, cadreHaut) > 0.5f)
        {
            cadreAnnonceScene.anchoredPosition =
                Vector2.MoveTowards(cadreAnnonceScene.anchoredPosition, cadreHaut, vitesse * Time.deltaTime);
            yield return null;
        }
    }
}