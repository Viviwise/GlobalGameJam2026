using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreListener : MonoBehaviour
{
    public int score;
    public GameObject pointerMain;
    public float rotationStep = 30f;

    public void AddScore()
    {
        score++;
    }

    public void RemoveScore()
    {
        score--;
    }

    public void GoldenScore()
    {
        score = 0;
    }

    private void Update()
    {
        score = Mathf.Clamp(score, -3, 3);

        float zRotation = 0f;

        switch (score)
        {
            case -3: zRotation = 83f; break;
            case -2: zRotation = 65f; break;
            case -1: zRotation = 35f; break;
            case 0:  zRotation = 0f;   break;
            case 1:  zRotation = -35f;  break;
            case 2:  zRotation = -65f;  break;
            case 3:  zRotation = -83f;  break;
        }

        pointerMain.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }
}
