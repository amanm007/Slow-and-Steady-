using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{

    [SerializeField] private Transform tvBody;
/*    [SerializeField] private float _topAngleLimit = 40f;
    [SerializeField] private float _bottomAngleLimit = -20f;*/
    [SerializeField] bool _flipTowardsMouse;

    private Vector2 _mouseWorldPos;
    private SpriteRenderer character;
    [SerializeField] private Sprite state;
    [SerializeField] private Sprite tvFront, tvBack, tvLeft, tvRight, tvBackLeft, tvBackRight;


    // Start is called before the first frame update
    void Awake()
    {
        character = GetComponent<SpriteRenderer>();
        state = character.sprite;
    }

    private void Update()
    {
        LookAtMouse();
    }

    private void OnLook(InputValue value)
    {
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
    }

    private void LookAtMouse()
    {
        var dir = (_mouseWorldPos - (Vector2)tvBody.position).normalized;
        if (state == tvBack || state == tvBackLeft || state == tvBackRight)
        {
            character.sortingOrder = 20;
        }
        else
        {
            character.sortingOrder = 9;
        }

        if (dir.x > -0.33 && dir.x < 0.33 && dir.y > 0)
        {
            state = tvBack;
        }
        else if (dir.x > -1 && dir.x < -0.33 && dir.y > 0)
        {
            state = tvBackLeft;
        }
        else if (dir.x > 0.33 && dir.x < 1 && dir.y > 0)
        {
            state = tvBackRight;
        }
        else if (dir.x > -0.33 && dir.x < 0.33 && dir.y < 0)
        {
            state = tvFront;
        }
        else if (dir.x > -1 && dir.x < -0.33 && dir.y < 0)
        {
            state = tvLeft;
        }
        else if (dir.x > 0.33 && dir.x < 1 && dir.y < 0)
        {
            state = tvRight;
        }

        character.sprite = state;
        /*
                tvBody.right = dir * Mathf.Sign(transform.localScale.x);
                var eulerDir = tvBody.localEulerAngles;
                eulerDir.z = Mathf.Clamp(eulerDir.z - (eulerDir.z > 180 ? 360 : 0), _bottomAngleLimit, _topAngleLimit);
                tvBody.localEulerAngles = eulerDir;*/
    }

    private void FlipX(float x)
    {
        if (x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(x), 1, 1);
        }
    }
}
