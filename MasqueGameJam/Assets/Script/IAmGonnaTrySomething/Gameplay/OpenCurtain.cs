using System.Collections;
using UnityEngine;
public class OpenCurtain : MonoBehaviour
{
    public Transform rideauGauche;
    public Transform rideauDroit;
    
    private static OpenCurtain instance;
    
    public float speed = 10f;

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

    public void Ouvrir()
    {
        StopAllCoroutines();
        StartCoroutine(OuvrirRideauxScene());
    }

    public void Fermer()
    { 
         StopAllCoroutines();
        StartCoroutine(FermerRideauxScene());
    }

    public IEnumerator OuvrirRideauxScene()
    {
        while (Vector3.Distance(rideauGauche.position, gaucheOuvert) > 0.01f)
        {
            rideauGauche.position =
                Vector3.MoveTowards(rideauGauche.position, gaucheOuvert, speed * Time.deltaTime);

            rideauDroit.position =
                Vector3.MoveTowards(rideauDroit.position, droitOuvert, speed * Time.deltaTime);

            yield return null;
        }
    }

    public IEnumerator FermerRideauxScene()
    {
        while (Vector3.Distance(rideauGauche.position, gaucheFerme) > 0.01f)
        { 
            rideauGauche.position =
                Vector3.MoveTowards(rideauGauche.position, gaucheFerme, speed * Time.deltaTime);

            rideauDroit.position =
                Vector3.MoveTowards(rideauDroit.position, droitFerme, speed * Time.deltaTime);

            yield return null;
        }
    }
}