using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RideauxTransition : MonoBehaviour
{
  public RectTransform rideauGauche;
  public RectTransform rideauDroit;

  public float vitesse = 10f;

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
  
  IEnumerator Start()
  {
    yield return null; 
    StartCoroutine(OuvrirRideaux());
  }


  public void ChargerScene(string nomScene)
  {
    StartCoroutine(TransitionScene(nomScene));
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
