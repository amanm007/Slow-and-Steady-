using UnityEngine;

public class SlowMotionAbility : MonoBehaviour
{
    public float slowMotionFactor = 0.2f; 
    public float slowMotionDuration = 5f; // Duration of the slow-motion effect

    private float normalTimeScale = 1f; 
    private bool isSlowMotionActive = false; 
    private float slowMotionTimer = 0f; 

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
    }

    private void ActivateSlowMotion()
    {
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f; // Adjust fixedDeltaTime according to the new time scale
        slowMotionTimer = slowMotionDuration;
        isSlowMotionActive = true;
    }

    private void DeactivateSlowMotion()
    {
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = normalTimeScale * 0.02f; // Reset fixedDeltaTime to its default value
        isSlowMotionActive = false;
    }
}
