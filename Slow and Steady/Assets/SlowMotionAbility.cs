using UnityEngine;

public class SlowMotionAbility : MonoBehaviour
{
    public float slowMotionFactor = 0.2f;
    public float slowMotionDuration = 5f;
    public Camera playerCamera; 
    public float zoomedSize = 9f; 
    public float zoomSpeed = 10f; 

    private float normalTimeScale = 1f;
    private bool isSlowMotionActive = false;
    private float slowMotionTimer = 0f;
    private float defaultSize; 

    void Start()
    {
        
        if (!playerCamera) playerCamera = Camera.main;

        
        defaultSize = playerCamera.orthographicSize;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isSlowMotionActive)
        {
            ActivateSlowMotion();
        }

        if ((Input.GetMouseButtonUp(1) || slowMotionTimer <= 0) && isSlowMotionActive)
        {
            DeactivateSlowMotion();
        }

        // Update the slow motion timer
        if (isSlowMotionActive)
        {
            slowMotionTimer -= Time.unscaledDeltaTime;
        }

        //using lerf function math is cool
        if (isSlowMotionActive)
        {
            playerCamera.orthographicSize = Mathf.Lerp(playerCamera.orthographicSize, zoomedSize, Time.unscaledDeltaTime * zoomSpeed);
        }
        else
        {
            playerCamera.orthographicSize = Mathf.Lerp(playerCamera.orthographicSize, defaultSize, Time.unscaledDeltaTime * zoomSpeed);
        }
    }

    private void ActivateSlowMotion()
    {
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        slowMotionTimer = slowMotionDuration;
        isSlowMotionActive = true;
    }

    private void DeactivateSlowMotion()
    {
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = normalTimeScale * 0.02f;
        isSlowMotionActive = false;
    }
}