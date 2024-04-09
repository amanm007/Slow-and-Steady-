using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAimWeapon : MonoBehaviour
{
    public float shootingRange = 100f;
    private Camera mainCamera;
    private Transform aimTransform;
    private Transform aimGunEndPointTransform;
    public Material tracerMaterial;
    int shootingLayerMask;
    

    public float shootingCooldown, aimedShootingCooldown; // Cooldown period in seconds'
    private bool isCooldown = false; // To track if we are in cooldown
    private float currentCooldownTime = 0f;
    public Image reloadBar;

    public AudioManager audioManager;

    /*    private bool aimCooldownBool = false;
        private float aimCooldown = 5000f;
        private float aimTimer = 3000f;*/

    private void Awake()
    {
        mainCamera = Camera.main;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (transform.Find("Aim") != null)
        {
            aimTransform = transform.Find("Aim");
            aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        }

    }

    private void Update()
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Factory"))
        {
            HandleAiming();
            HandleShooting();
            UpdateReloadBar();
        }
    }


    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && !isCooldown && (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Factory_Simon")))
        {
            StartCoroutine(Shoot());
        }
    }



    private IEnumerator Shoot()
    {
        isCooldown = true; // Start cooldown
        currentCooldownTime = shootingCooldown;

        audioManager.PlaySFX(audioManager.shot);

        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 shootingDirection = (mousePosition - aimGunEndPointTransform.position).normalized;
        float distanceToMouse = Vector2.Distance(aimGunEndPointTransform.position, mousePosition);

        shootingLayerMask = 1 << 2;
        shootingLayerMask = ~shootingLayerMask;
        RaycastHit2D hit = Physics2D.Raycast(aimGunEndPointTransform.position, shootingDirection, 960, shootingLayerMask);
        Ray2D shot = new(aimGunEndPointTransform.position, shootingDirection);

        if (hit.collider != null)
        {
            //Debug.Log("Raycast hit: " + hit.collider.name);
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                var enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(1); // Assuming each bullet deals 1 damage

                }
            }
        }
        else
        {
            //Debug.Log("Raycast did not hit anything.");
        }

        if (GameObject.Find("CRT TV").GetComponent<SlowMotionAbility>().isSlowMotionActive)
        {
            WeaponTracer.Create(aimGunEndPointTransform.position, shot.GetPoint(960), tracerMaterial, 0.05f);
            yield return new WaitForSeconds(aimedShootingCooldown); // Wait for cooldown period
        }
        else
        {
            WeaponTracer.Create(aimGunEndPointTransform.position, shot.GetPoint(960), tracerMaterial, 0.4f);
            StartCoroutine(ShakeCamera(0.1f, 0.2f));
            yield return new WaitForSeconds(shootingCooldown - 0.45f); // Wait for cooldown period
            audioManager.PlaySFX(audioManager.rechamber, 0.1f);
            yield return new WaitForSeconds(0.45f);
        }
        
        isCooldown = false; // End cooldown
    }
    private void UpdateReloadBar()
    {
        if (isCooldown)
        {
            currentCooldownTime -= Time.deltaTime; // Decrease cooldown time
            reloadBar.fillAmount = (shootingCooldown - currentCooldownTime) / shootingCooldown;
        }
        else
        {
            reloadBar.fillAmount = 1; // Ready to shoot
        }
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        bool isLeft = mousePosition.x < transform.position.x;
        SpriteRenderer sniperSpriteRenderer = aimTransform.Find("Sniper").GetComponent<SpriteRenderer>();
        sniperSpriteRenderer.flipY = isLeft;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }


    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;


    }


    private IEnumerator ShakeCamera(float intensity, float duration)
    {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Mathf.Clamp(UnityEngine.Random.Range(-1f, 1f) * intensity, -intensity, intensity);
            float y = Mathf.Clamp(UnityEngine.Random.Range(-1f, 1f) * intensity, -intensity, intensity);
            cameraController.ApplyShake(new Vector3(x, y, 0));
            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraController.ResetShake();
    }
}
