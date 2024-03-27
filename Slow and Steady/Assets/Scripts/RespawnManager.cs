using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPos;
    [SerializeField] private GameObject respawnPoint;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

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
