using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimCredit : MonoBehaviour
{
    public float speed;
    public float height;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;

        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
