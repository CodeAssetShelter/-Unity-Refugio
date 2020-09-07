using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigid2D;

    [Space(20)]
    public float moveSpeed = 1.0f;

    [Space(20)]
    public float lookUpTimerMin = 1.0f;
    public float lookUpTimerMax = 3.0f;


    Vector3 myStartPos;
    float deltaTime = 0.02f;
    bool movable = true;
    float patternRivision = 1.0f;
    void Awake()
    {
        myStartPos = transform.localPosition;
        deltaTime = Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        transform.localPosition = myStartPos;
        patternRivision = (MapManager.Instance == null) ? 1.0f : MapManager.Instance.GetPatternRivision();
        StartCoroutine(CorMove());
    }

    IEnumerator CorMove()
    {
        float timer = Random.Range(lookUpTimerMin, lookUpTimerMax);
        float timerProcess = 0;

        float horizontal = 1;

        while (true)
        {
            if (movable == true)
            {
                transform.Translate(moveSpeed * horizontal * deltaTime, 0, 0, Space.Self);

                timerProcess += deltaTime;
                if (timerProcess > timer * patternRivision)
                {
                    animator.SetBool("LookUp", true);
                    timerProcess = 0;
                    movable = false;
                }
            }
            else
            {
                if (timerProcess > 1.0f)
                {
                    timerProcess = 0;
                    rigid2D.gravityScale *= -1;
                    spriteRenderer.flipY = !spriteRenderer.flipY;
                    timer = Random.Range(lookUpTimerMin, lookUpTimerMax);

                    if (Random.Range(0, 2) == 0)
                    {
                        horizontal = 1;
                        spriteRenderer.flipX = true;
                    }
                    else
                    {
                        horizontal = -1;
                        spriteRenderer.flipX = false;
                    }
                }
                else
                {
                    timerProcess += deltaTime;
                }
            }
            yield return new WaitForSeconds(deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Map"))
        {
            if (movable == false)
            {
                animator.SetBool("LookUp", false);
                movable = true;
            }
        }
    }
}
