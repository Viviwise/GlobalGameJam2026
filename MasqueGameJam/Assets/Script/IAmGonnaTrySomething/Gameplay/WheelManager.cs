using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.IAmGonnaTrySomething.Gameplay
{
    public class WheelManager : MonoBehaviour
    {
        public GameObject mainCursor;
        public GameObject smallCursor;

        [SerializeField] private GameObject[] livesOk;
        [SerializeField] private GameObject[] livesLose;

        [SerializeField] private float angleBySegment = 35f;
        [SerializeField] private float baseSpeed = 5f;

        public int lives = 2;

        private int score = 0;
        private bool isRotating = false;

        public event Action EndPhaseSurviving;
        public event Action<int> EndPhaseDying;

        private void Start()
        {
            foreach (GameObject obj in livesLose)
                obj.SetActive(false);

            foreach (GameObject obj in livesOk)
                obj.SetActive(true);
        }

        float GetTargetAngleFromScore(int score)
        {
            switch (score)
            {
                case 1:  return 58f;
                case 2:  return 35f;
                case 3:  return 7f;

                case -1: return 120f;
                case -2: return 145f;
                case -3: return 170f;

                default: return 90f;
            }
        }

        public void AjustWheel(WheelScore wheelScore)
        {
            if (isRotating) return;

            SoundManager.PlaySound(SoundType.WheelDrums, 0.9f);

            if (wheelScore == WheelScore.Boredom) score++;
            else if (wheelScore == WheelScore.Bad) score--;

            if (wheelScore == WheelScore.Good)
                StartCoroutine(DramaticWheelNoTurn());
            else
                StartCoroutine(DramaticWheelTurn());
        }

        IEnumerator DramaticWheelNoTurn()
        {
            isRotating = true;

            float goalAngle = mainCursor.transform.eulerAngles.z;
            float localSpeed = baseSpeed;
            int baitDirection = Random.Range(0, 2) == 0 ? 1 : -1;

            float baitDone = 0f;
            float baitAmount = angleBySegment / 4f;

            while (baitDone < baitAmount)
            {
                float delta = localSpeed * Time.deltaTime;
                mainCursor.transform.eulerAngles += new Vector3(0, 0, baitDirection * delta);
                smallCursor.transform.eulerAngles = mainCursor.transform.eulerAngles;
                baitDone += delta;
                yield return null;
            }

            localSpeed *= 6f;

            while (Mathf.Abs(Mathf.DeltaAngle(mainCursor.transform.eulerAngles.z, goalAngle)) > 0.1f)
            {
                float newAngle = Mathf.MoveTowardsAngle(
                    mainCursor.transform.eulerAngles.z,
                    goalAngle,
                    localSpeed * Time.deltaTime
                );

                mainCursor.transform.eulerAngles = new Vector3(0, 0, newAngle);
                smallCursor.transform.eulerAngles = mainCursor.transform.eulerAngles;
                yield return null;
            }

            SoundManager.PlaySound(SoundType.Applause, 0.9f);
            EndPhase();
            isRotating = false;
        }

        IEnumerator DramaticWheelTurn()
        {
            isRotating = true;

            float baitSpeed = baseSpeed;
            float finalSpeed = baseSpeed * 6f;
            float targetAngle = GetTargetAngleFromScore(score);

            int baitDirection = Random.Range(0, 2) == 0 ? 1 : -1;
            float baitAmount = angleBySegment / 4f;
            float baitDone = 0f;

            while (baitDone < baitAmount)
            {
                float delta = baitSpeed * Time.deltaTime;
                mainCursor.transform.eulerAngles += new Vector3(0, 0, baitDirection * delta);
                smallCursor.transform.eulerAngles = mainCursor.transform.eulerAngles;
                baitDone += delta;
                yield return null;
            }

            while (Mathf.Abs(Mathf.DeltaAngle(mainCursor.transform.eulerAngles.z, targetAngle)) > 0.1f)
            {
                float newAngle = Mathf.MoveTowardsAngle(
                    mainCursor.transform.eulerAngles.z,
                    targetAngle,
                    finalSpeed * Time.deltaTime
                );

                mainCursor.transform.eulerAngles = new Vector3(0, 0, newAngle);
                smallCursor.transform.eulerAngles = mainCursor.transform.eulerAngles;
                yield return null;
            }

            mainCursor.transform.eulerAngles = new Vector3(0, 0, targetAngle);
            smallCursor.transform.eulerAngles = mainCursor.transform.eulerAngles;

            EndPhase();
            isRotating = false;
        }
        public void ResetCursorsForKillActor()
        {
            StartCoroutine(RotateCursorsToDefault());
        }

        IEnumerator RotateCursorsToDefault()
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 90);

            while (Quaternion.Angle(mainCursor.transform.rotation, targetRotation) > 0.1f)
            {
                mainCursor.transform.rotation =
                    Quaternion.RotateTowards(mainCursor.transform.rotation, targetRotation, baseSpeed * 100f * Time.deltaTime);

                smallCursor.transform.rotation = mainCursor.transform.rotation;
                yield return null;
            }

            mainCursor.transform.rotation = targetRotation;
            smallCursor.transform.rotation = targetRotation;
        }
        private void EndPhase()
        {
            if (score >= 3 || score <= -3)
            {
                lives--;

                if (lives >= 0 && lives < livesLose.Length)
                    livesLose[lives].SetActive(true);

                if (lives >= 0 && lives < livesOk.Length)
                    livesOk[lives].SetActive(false);

                score = 0;
                EndPhaseDying?.Invoke(score);
            }
            else
            {
                EndPhaseSurviving?.Invoke();
            }
        }
    }
}
