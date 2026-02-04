using System.Collections;
using UnityEngine;
using TMPro;

public class FrameMove : MonoBehaviour
{
    public RectTransform FrameAct;
    public TMP_Text sceneNameText;

    private float speed = 70f;

    private Vector2 cadreHaut;
    private Vector2 cadreBas;

    private void Awake()
    {
        Debug.Log("FrameMove Awake");
        cadreHaut = FrameAct.anchoredPosition;
        float hauteurCadre = FrameAct.rect.height;
        cadreBas = cadreHaut + Vector2.down * hauteurCadre;

        FrameAct.gameObject.SetActive(false);
    }

    public void DisplayFrame(string sceneName)
    {
        StartCoroutine(PlaySceneTitle(sceneName));
    }
    public IEnumerator PlaySceneTitle(string sceneName)
    {
        sceneNameText.text = sceneName;
        FrameAct.gameObject.SetActive(true);

        while (Vector2.Distance(FrameAct.anchoredPosition, cadreBas) > 0.1f)
        {
            FrameAct.anchoredPosition =
                Vector2.MoveTowards(FrameAct.anchoredPosition, cadreBas, speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(3.5f);

        while (Vector2.Distance(FrameAct.anchoredPosition, cadreHaut) > 0.1f)
        {
            FrameAct.anchoredPosition =
                Vector2.MoveTowards(FrameAct.anchoredPosition, cadreHaut, speed * Time.deltaTime);
            yield return null;
        }

        FrameAct.gameObject.SetActive(false);
    }
}
