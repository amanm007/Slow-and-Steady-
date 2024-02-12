using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAimWeapon : MonoBehaviour
{
    public float shootingRange = 100f;
    public LayerMask shootingLayerMask;
    private Camera mainCamera;
    private Transform aimTransform;
    private Transform aimGunEndPointTransform;
    public Material tracerMaterial;
    /*
        public event EventHandler<OnShootEventARgs> OnShoot;

        public class OnShootEventARgs: EventArgs
        {
            public Vector3 gunEndPointPosition;
            public Vector3 shootPosition;


        }
    */


    private void Awake()
    {
        mainCamera = Camera.main;
        aimTransform = transform.Find("Aim");
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
       // tracerMaterial = Resources.Load<Material>("Assets/BulletTrace/Materials/WeaponTracer.mat");


    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();


    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 shootingDirection = (mousePosition - aimGunEndPointTransform.position).normalized;
            float distanceToMouse = Vector2.Distance(aimGunEndPointTransform.position, mousePosition);

            // Use the distance to the mouse for the raycast distance
            RaycastHit2D hit = Physics2D.Raycast(aimGunEndPointTransform.position, shootingDirection, distanceToMouse, shootingLayerMask);

            if (hit.collider != null)
            {
                // Hit something
                OnShoot(hit.point, hit.collider.gameObject);
                Debug.Log("Raycast hit: " + hit.collider.name);
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            else
            {
                // Hit nothing or it hit something beyond the mouse cursor
                OnShoot(mousePosition, null); // Pass the mouse position instead of the hit point
                Debug.Log("Raycast did not hit anything.");
            }
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

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = -mainCamera.transform.position.z; // Set the distance to the camera
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
    private void OnShoot(Vector3 hitPosition, GameObject hitObject)
    {
        StartCoroutine(ShakeCamera(0.1f, 0.2f));
        // Check if the tracerMaterial has been assigned
        if (tracerMaterial != null)
        {
            
            WeaponTracer.Create(aimGunEndPointTransform.position, hitPosition, tracerMaterial);
        }
        else
        {
            // If the tracerMaterial is not assigned, log an error message
            Debug.LogError("Tracer Material is not assigned.");
        }

        // If you hit an object, handle logic like damaging an enemy here
        if (hitObject != null)
        {
            // Damage logic or other effects can be applied to the hitObject
            ;
        }

        
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
