using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Transform aimTransform;
    private Transform aimGunEndPointTransform;

    public event EventHandler<OnShootEventARgs> OnShoot;

    public class OnShootEventARgs: EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;


    }



    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");


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
            //we can call an firing animation
            OnShoot?.Invoke(this, new OnShootEventARgs
                {
                gunEndPointPosition=aimGunEndPointTransform.position,
                shootPosition= mousePosition,
            });
            

            
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

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
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
    
}
