using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BadEndMenu : MonoBehaviour
{
    public Animator animatorCatharsis;
    public ScreenTransitionCurtain screenTransitionCurtain;

    IEnumerator Start()
    {
        AnimatorStateInfo stateInfo = animatorCatharsis.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        screenTransitionCurtain.FermetureRideaux();
        screenTransitionCurtain.TransitionAppear();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenuScene");
    }

   
}
