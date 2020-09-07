using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All Direction base is Look Rightside
public class BigFish : MonoBehaviour
{
    enum SpriteDirection { Up = 45, Down = -45, Forward = 0}
    private SpriteDirection spriteDirection;

    enum MoveDirection { Left = -1, Right = 1 }
    private MoveDirection moveDirection = MoveDirection.Right;
    private float moveDir = 1;
    private float angle = 0;

    public GameObject body;
    public Animator animator;
    public float animationSpeedMax = 0.6f;

    private Vector3 axisDirection = Vector3.right;
    private Vector3 swimDirection = Vector3.zero;

    public float moveSpeed = 0.5f;

    private float moveSpeedIdleMin = 0.5f;
    private float moveSpeedIdleMax = 1.0f;

    private float moveSpeedBoostMin = 1.5f;
    private float moveSpeedBoostMax = 2.5f;

    private Vector3 initPosition;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        initPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        animator.speed = Random.Range(0.2f, animationSpeedMax);
        transform.localPosition = initPosition;
        StartCoroutine(CorMove());
    }

    
    IEnumerator CorMove()
    {
        float myDeltaTime = Time.fixedDeltaTime;
        while (true)
        {
            //swimDirection = Quaternion.Euler(0, 0, angle) * axisDirection;
            transform.Translate(swimDirection * moveSpeed * myDeltaTime * moveDir, Space.Self);
            yield return new WaitForSeconds(myDeltaTime);
        }
    }

    public void SetMoveSpeed(int isBoost)
    {
        float spriteDir = 0;


        if (isBoost == 1)
        {
            angle = Random.Range(0, 360);

            // Sprite Direction
            if ((angle >= 0 && angle <= 25) || (angle >= 335 && angle <= 360) || (angle >= 155 && angle <= 225))
            {
                spriteDirection = SpriteDirection.Forward;
            }
            if (angle > 25 && angle < 155)
            {
                spriteDirection = SpriteDirection.Up;
            }
            if (angle > 225 && angle < 335)
            {
                spriteDirection = SpriteDirection.Down;
            }


            // Move Direction;
            if ((angle >= 0 && angle <= 90) || (angle >= 270 && angle <= 360))
            {
                moveDirection = MoveDirection.Right;
                moveDir = (float)moveDirection;
            }
            else
            {
                moveDirection = MoveDirection.Left;
                moveDir = (float)moveDirection;
            }

            moveSpeed = Random.Range(moveSpeedBoostMin, moveSpeedBoostMax);
        }
        else
        {
            spriteDirection = SpriteDirection.Forward;
            moveDir = (float)moveDirection;
            switch (moveDirection)
            {
                case MoveDirection.Left:
                    moveDir = (float)moveDirection;
                    angle = 180f;
                    break;
                case MoveDirection.Right:
                    moveDir = (float)moveDirection;
                    angle = 0f;
                    break;
            }
            moveSpeed = Random.Range(moveSpeedIdleMin, moveSpeedIdleMax);
        }

        spriteDir = (float)spriteDirection;

        swimDirection = Quaternion.Euler(0, 0, angle) * axisDirection;
        swimDirection *= moveDir * animator.speed;
        body.transform.localScale = new Vector3(moveDir, 1);
        body.transform.localRotation = Quaternion.Euler(0, 0, spriteDir * moveDir);
    }
}
