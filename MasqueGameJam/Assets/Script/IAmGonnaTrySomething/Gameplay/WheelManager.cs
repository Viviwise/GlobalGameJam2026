using System.Collections;
using UnityEngine;

namespace Script.IAmGonnaTrySomething.Gameplay
{
    public class WheelManager : MonoBehaviour
    {
        public GameObject mainCursor, smallCursor;
        public GameObject smallWheel;
        
        private float angleBySegment = 20f;

        public void AjustWheel(WheelScore score)
        {
            if (score == WheelScore.Boredom)
            {
                StartCoroutine(DramaticWheelTurnLeft());
            }
        }

        IEnumerator DramaticWheelTurnLeft()
        {
            float goalAngle = mainCursor.transform.eulerAngles.z+angleBySegment;
            float speed = 10f;
            int bait = Random.Range(0,1);
            if (bait == 0)
            {
                float elapsedAngles = 0;
                while (elapsedAngles < angleBySegment / 4)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z + speed * Time.deltaTime);
                    yield return null;
                }
                speed = speed * 2;
                while (mainCursor.transform.eulerAngles.z < goalAngle)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z + speed * Time.deltaTime);
                    yield return null;
                }
            }
            else if (bait == 1)
            {
                float elapsedAngles = 0;
                while (elapsedAngles < angleBySegment / 4)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z - speed * Time.deltaTime);
                    elapsedAngles += Time.deltaTime * speed;
                    yield return null;
                }
                speed = speed * 2;
                while (mainCursor.transform.eulerAngles.z < goalAngle)
                {
                    mainCursor.transform.eulerAngles = new Vector3(0, 0, mainCursor.transform.eulerAngles.z + speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }
}