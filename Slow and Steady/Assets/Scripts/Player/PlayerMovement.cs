using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float SPEED = 20f;
    private float originalSpeed;
    private bool isSlowedDown = false;

    private float dirX = 0;
    private float dirY = 0;

    private PlayerMain playerMain;

    private Vector3 moveDir;
    private Vector3 lastMoveDir;

    public bool pauseMovement = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        playerMain = GetComponent<PlayerMain>();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("speed"))
        {
            SPEED = PlayerPrefs.GetFloat("speed");
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if(pauseMovement == false)
        {
            dirX = Input.GetAxisRaw("Horizontal");    //Raw sets the axis back to '0' after key release
            dirY = Input.GetAxisRaw("Vertical");    //Raw sets the axis back to '0' after key release
        }
        else
        {
            dirX = 0;
            dirY = 0;
        }

        moveDir = new Vector3(dirX, dirY).normalized;
       
        bool isIdle = dirX == 0 && dirY == 0;

        if (isIdle)
        {
            //playerMain.PlayerSwapAimNormal.PlayIdleAnim();
        }
        else
        {
            lastMoveDir = moveDir;
            // playerMain.PlayerSwapAimNormal.PlayMoveAnim(moveDir);
        }

        /*      float moveX = 0f;
        float moveY = 0f;*/

        /*if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = +1f;
        }
        */
    }

    private void FixedUpdate()
    {
        playerMain.PlayerRigidbody2D.velocity = moveDir * SPEED;
    }

    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
        playerMain.PlayerRigidbody2D.velocity = Vector3.zero;
    }

    public Vector3 GetLastMoveDir()
    {
        return lastMoveDir;
    }
    public void ModifySpeed(float modifier)
    {
        if (!isSlowedDown && modifier < 1f)
        {
            originalSpeed = SPEED;
            SPEED *= modifier;
            isSlowedDown = true;
        }
        else if (isSlowedDown && modifier == 1f)
        {
            SPEED = originalSpeed;
            isSlowedDown = false;
        }
    }

    public void SetSpeedUpgrade(float upgradeValue)
    {
        SPEED += upgradeValue;
        PlayerPrefs.SetFloat("speed", SPEED);
    }

}