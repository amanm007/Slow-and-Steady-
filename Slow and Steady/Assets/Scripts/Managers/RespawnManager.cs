using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPos;
    [SerializeField] private GameObject respawnPoint;
    private Vector2 playerSavedSpawn;
    void Start()
    {

        if (EnterShootingRange.inFactory)
        {
            playerPos.transform.position = respawnPoint.transform.position;
            EnterShootingRange.inFactory = false;
        }
        else
        {
            return;
        }

    }

}
