using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Vector3 positionOffset, tempPosition;

    private void Start()
    {
        positionOffset = transform.localPosition;
    }
    private void FixedUpdate()
    {
        tempPosition = positionOffset;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI) * 0.25f;
        transform.localPosition = tempPosition;
    }
}
