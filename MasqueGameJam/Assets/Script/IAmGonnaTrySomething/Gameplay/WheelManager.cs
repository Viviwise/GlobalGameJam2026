using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.IAmGonnaTrySomething.Gameplay
{
    public class WheelManager : MonoBehaviour
    {
        public GameObject mainCursor, smallCursor;
        
        private float angleBySegment = 40f;
        private int score = 0;

        public event Action EndPhaseSurviving;
        public event Action EndPhaseDying;

        public void AjustWheel(WheelScore wheelScore)
        {
            if (wheelScore == WheelScore.Boredom)
            {
                score -= 1;
                StartCoroutine(DramaticWheelTurnLeft());
            }
            else if (wheelScore == WheelScore.Bad)
            {
                score += 1;
                StartCoroutine(DramaticWheelTurnRight());
            }
            else if (wheelScore == WheelScore.Good)
            {
                StartCoroutine(DramaticWheelNoTurn());
            }
        }

        IEnumerator DramaticWheelNoTurn()
        {
            float goalAngle = mainCursor.transform.eulerAngles.z%360f;
            float speed = 5f;
            int baitDirection = Random.Range(0,2);
            if (baitDirection == 0)
            {
                float elapsedAngles = 0;
                while (elapsedAngles < angleBySegment / 4)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z%360f + speed * Time.deltaTime);
                    elapsedAngles += Time.deltaTime * speed;
                    yield return null;
                }
                speed = speed * 6;
                while (mainCursor.transform.eulerAngles.z > goalAngle)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z%360f - speed * Time.deltaTime);
                    yield return null;
                }
            }
            else if (baitDirection == 1)
            {
                float elapsedAngles = 0;
                while (elapsedAngles < angleBySegment / 4)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z%360f - speed * Time.deltaTime);
                    elapsedAngles += Time.deltaTime * speed;
                    yield return null;
                }
                speed = speed * 6;
                while (mainCursor.transform.eulerAngles.z > goalAngle)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z%360f - speed * Time.deltaTime);
                    yield return null;
                }
            }
            EndPhase();
        }
        IEnumerator DramaticWheelTurnLeft()
        {
            float goalAngle = mainCursor.transform.eulerAngles.z%360f+angleBySegment;
            float speed = 5f;
            int bait = Random.Range(0,2);
            if (bait == 0)
            {
                float elapsedAngles = 0;
                while (elapsedAngles < angleBySegment / 4)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z%360f + speed * Time.deltaTime);
                    elapsedAngles += Time.deltaTime * speed;
                    yield return null;
                }
                speed = speed * 6;
                while (mainCursor.transform.eulerAngles.z < goalAngle)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z%360f + speed * Time.deltaTime);
                    yield return null;
                }
            }
            else if (bait == 1)
            {
                float elapsedAngles = 0;
                while (elapsedAngles < angleBySegment / 4)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z%360f - speed * Time.deltaTime);
                    elapsedAngles += Time.deltaTime * speed;
                    yield return null;
                }
                speed = speed * 6;
                while (mainCursor.transform.eulerAngles.z < goalAngle)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z%360f + speed * Time.deltaTime);
                    yield return null;
                }
            }
            EndPhase();
        }
        IEnumerator DramaticWheelTurnRight()
        {
            float goalAngle = mainCursor.transform.eulerAngles.z-angleBySegment;
            Debug.Log(goalAngle);
            float speed = 5f;
            int bait = Random.Range(0,2);
            if (bait == 0)
            {
                float elapsedAngles = 0;
                while (elapsedAngles < angleBySegment / 4)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z + speed * Time.deltaTime);
                    elapsedAngles += Time.deltaTime * speed;
                    yield return null;
                }
                speed = speed * 4;
                while (mainCursor.transform.eulerAngles.z > goalAngle)
                {
                    Debug.Log(speed);
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z - speed * Time.deltaTime);
                    yield return null;
                }
            }
            else if (bait == 1)
            {
                float elapsedAngles = 0;
                while (elapsedAngles < angleBySegment / 4)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z + speed * Time.deltaTime);
                    elapsedAngles += Time.deltaTime * speed;
                    yield return null;
                }
                speed = speed * 2;
                while (mainCursor.transform.eulerAngles.z > goalAngle)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z - speed * Time.deltaTime);
                    yield return null;
                }
            }
            EndPhase();
        }

        private void EndPhase()
        {
            smallCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z);
            if (score == 3 || score == -3)
            {
                EndPhaseDying?.Invoke();
            }
            else
            {
                EndPhaseSurviving?.Invoke();
            }
        }
    }
}