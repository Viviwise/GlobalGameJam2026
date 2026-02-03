using System.Collections;
using UnityEngine;
using TMPro;

public class FrameMove : MonoBehaviour
{
    public RectTransform cadreAnnonceScene; 
    public TMP_Text sceneNameText;          
    public SceneData firstScene;            

    public ScenarioManager scenarioManager;
    public OpenCurtain openCurtain;

    public float speed = 5f;

    private Vector2 cadreHaut;
    private Vector2 cadreBas;

    private void Awake()
    {
        cadreHaut = cadreAnnonceScene.anchoredPosition;
        float hauteurCadre = cadreAnnonceScene.rect.height;
        cadreBas = cadreHaut + Vector2.down * hauteurCadre;

        if (firstScene != null)
        {
            sceneNameText.text = firstScene.sceneName;
        }
    }

    private void Start()
    {
        scenarioManager.SetUpAll();
        StartCoroutine(SequenceIntro());
        scenarioManager.enabled = false;
    }

    private IEnumerator SequenceIntro()
    {
        cadreAnnonceScene.gameObject.SetActive(true);

        while (Vector2.Distance(cadreAnnonceScene.anchoredPosition, cadreBas) > 0.1f)
        {
            cadreAnnonceScene.anchoredPosition =
                Vector2.MoveTowards(cadreAnnonceScene.anchoredPosition, cadreBas, speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        
        openCurtain.Ouvrir();
        
        scenarioManager.enabled = true;

        while (Vector2.Distance(cadreAnnonceScene.anchoredPosition, cadreHaut) > 0.1f)
        {
            cadreAnnonceScene.anchoredPosition =
                Vector2.MoveTowards(cadreAnnonceScene.anchoredPosition, cadreHaut, speed * Time.deltaTime);
            yield return null;
        }

        cadreAnnonceScene.gameObject.SetActive(false);
    }
}
