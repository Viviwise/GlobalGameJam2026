using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class OpenCurtain : MonoBehaviour
{
    public Transform rideauGauche;
    public Transform rideauDroit;
    
    public float vitesse = 5f;

    Vector3 gaucheFerme;
    Vector3 droitFerme;
    Vector3 gaucheOuvert;
    Vector3 droitOuvert;
    
    private TMPro.TMP_Text sceneName;

    void Awake()
    {
        gaucheFerme = rideauGauche.position;
        droitFerme = rideauDroit.position;


        float largeurGauche = rideauGauche.GetComponent<SpriteRenderer>().bounds.size.x;
        float largeurDroit = rideauDroit.GetComponent<SpriteRenderer>().bounds.size.x;

        gaucheOuvert = gaucheFerme + Vector3.left * largeurGauche;
        droitOuvert = droitFerme + Vector3.right * largeurDroit;

    }


    void Start()
    {
        Ouvrir();
    }

    public void Ouvrir()
    {
        StopAllCoroutines();
        StartCoroutine(OuvrirRideaux());
    }

    public void Fermer()
    { 
         StopAllCoroutines();
        StartCoroutine(FermerRideaux());
    }

    IEnumerator OuvrirRideaux()
    {
        while (Vector3.Distance(rideauGauche.position, gaucheOuvert) > 0.01f)
        {
            rideauGauche.position =
                Vector3.MoveTowards(rideauGauche.position, gaucheOuvert, vitesse * Time.deltaTime);

            rideauDroit.position =
                Vector3.MoveTowards(rideauDroit.position, droitOuvert, vitesse * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator FermerRideaux()
    {
        while (Vector3.Distance(rideauGauche.position, gaucheFerme) > 0.01f)
        {
            rideauGauche.position =
                Vector3.MoveTowards(rideauGauche.position, gaucheFerme, vitesse * Time.deltaTime);

            rideauDroit.position =
                Vector3.MoveTowards(rideauDroit.position, droitFerme, vitesse * Time.deltaTime);

            yield return null;
        }
    }
}