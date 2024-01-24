using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform crttv;


    private void Update()
    {
        transform.position = new Vector3(crttv.position.x, crttv.position.y, transform.position.z);

    }
}
