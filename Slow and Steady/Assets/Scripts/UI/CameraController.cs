using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform crttv;
    private Vector3 shakeOffset = Vector3.zero;
    public float maxShakeMagnitude = 0.5f;

    private void Update()
    {
        if (crttv != null)
        {
            Vector3 basePosition = new Vector3(crttv.position.x, crttv.position.y, transform.position.z);
            shakeOffset = Vector3.ClampMagnitude(shakeOffset, maxShakeMagnitude);
            transform.position = basePosition + shakeOffset;
        }
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