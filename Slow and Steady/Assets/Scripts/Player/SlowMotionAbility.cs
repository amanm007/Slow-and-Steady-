using UnityEngine;
using UnityEngine.UI;

public class SlowMotionAbility : MonoBehaviour
{
    public static SlowMotionAbility instance;

    public float slowMotionFactor = 0.2f;
    public float slowMotionDuration = 5f;
    public Camera playerCamera;
    public float zoomedSize = 9f;
    public float zoomSpeed = 10f;

    private float normalTimeScale = 1f;
    public static bool isSlowMotionActive = false;
    private float slowMotionTimer = 0f;
    private float defaultSize;
    private Vector3 defaultPosition;

    public Image energyBar;
    public float maxEnergy = 100f;
    public float energyDepletionRate = 20f; // energy depletion  per second when slow motion is active
    public float energyRecoveryRate; //  energy recovery per second when slow motion is not active
    private float currentEnergy;

    private PlayerAimWeapon playerAimWeapon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        energyRecoveryRate = PlayerPrefs.GetFloat("recharge");
        energyRecoveryRate = 10f;

        if (!playerCamera) playerCamera = Camera.main;


        defaultSize = playerCamera.orthographicSize;
        defaultPosition = playerCamera.transform.position;
        currentEnergy = maxEnergy;
        UpdateEnergyBar();
        playerAimWeapon = GetComponent<PlayerAimWeapon>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isSlowMotionActive && currentEnergy > 0)
        {
            ActivateSlowMotion();
        }

        if ((Input.GetMouseButtonUp(1) || slowMotionTimer <= 0 || currentEnergy <= 0) && isSlowMotionActive)
        {
            DeactivateSlowMotion();
        }


        if (isSlowMotionActive)
        {
            slowMotionTimer -= Time.unscaledDeltaTime;
            currentEnergy -= energyDepletionRate * Time.unscaledDeltaTime; // Deplete energy
            UpdateEnergyBar();
        }
        else if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRecoveryRate * Time.unscaledDeltaTime; // Recover energy
            UpdateEnergyBar();
        }
        // Adjusting camera zoom and position towards the crosshair
        if (isSlowMotionActive && playerAimWeapon != null)
        {
            playerCamera.orthographicSize = Mathf.Lerp(playerCamera.orthographicSize, zoomedSize, Time.unscaledDeltaTime * zoomSpeed);
            Vector3 aimPosition = playerAimWeapon.GetMouseWorldPosition(); // Use the method from PlayerAimWeapon to get current aim position
            Vector3 cameraTargetPosition = new Vector3(aimPosition.x, aimPosition.y, defaultPosition.z);
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, cameraTargetPosition, Time.unscaledDeltaTime * zoomSpeed);
        }
        else
        {
            playerCamera.orthographicSize = Mathf.Lerp(playerCamera.orthographicSize, defaultSize, Time.unscaledDeltaTime * zoomSpeed);
          //  playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, defaultPosition, Time.unscaledDeltaTime * zoomSpeed);
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
    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.fillAmount = currentEnergy / maxEnergy;
        }
    }

    public void SetRecoveryRate(float upgradeValue)
    {
        energyRecoveryRate += upgradeValue;
        PlayerPrefs.SetFloat("recharge", energyRecoveryRate);
    }
}