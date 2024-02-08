using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform crttv;
    private Vector3 shakeOffset = Vector3.zero;

    private void Update()
    {
        Vector3 basePosition = new Vector3(crttv.position.x, crttv.position.y, transform.position.z);
        transform.position = basePosition + shakeOffset;
    }

    public void ApplyShake(Vector3 offset)
    {
        shakeOffset = offset;
    }

    public void ResetShake()
    {
        shakeOffset = Vector3.zero;
    }
}