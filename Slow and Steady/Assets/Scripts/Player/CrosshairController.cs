using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private Transform crosshair;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 basePosition = new Vector3(crosshair.position.x, crosshair.position.y, transform.position.z);
        transform.position = basePosition;
    }
}
