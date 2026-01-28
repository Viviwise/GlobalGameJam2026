using Unity.Collections;
using UnityEngine;

namespace Script.Choice.Actor5
{
    public class WayPointListener : MonoBehaviour
    {
        public float speed;
        public Transform[] waypoints;
       // public GameObject actor;
        private Transform target;
        private int destpoint = 0;

        
        public void DanceMove()
        {
            target = waypoints[0];

            Vector3 dir = target.position - target.transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) < 0.3f)
            {
                destpoint = (destpoint + 1) % waypoints.Length;
                target = waypoints[destpoint];
            }
        }

    }
}