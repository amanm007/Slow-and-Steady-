using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveZoneController : MonoBehaviour
{
    public static WaveZoneController instance;

    private BoxCollider2D zoneBoundry;
    public bool enteredWaveZone;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        zoneBoundry = GetComponent<BoxCollider2D>();
        enteredWaveZone = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("entered");
            zoneBoundry.isTrigger = false;
            enteredWaveZone = true;
        }
    }
}
