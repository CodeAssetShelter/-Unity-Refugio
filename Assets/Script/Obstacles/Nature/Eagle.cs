using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer body;

    [Space(20)]
    [Range(1.0f, 2.0f)]
    public float attackTimerMax = 2.0f;
    public float attackTimerMin = 1.0f;

    [Space(20)]
    [Range(0, 100)]
    public int attackChance = 50;

    [Space(20)]
    [Range(1.0f, 3.0f)]
    public float moveSpeedMax = 3.0f;
    public float moveSpeedMin = 1.0f;

    Vector3 myStartPos;
    Vector3 direction = Vector3.up;
    float rotation = 45;
    float patternRivision = 1.0f;


    void Awake()
    {
        myStartPos = transform.localPosition;
    }

    private void OnEnable()
    {
        transform.localPosition = myStartPos;
        patternRivision = (MapManager.Instance == null) ? 1.0f : MapManager.Instance.GetPatternRivision();
        StartCoroutine(CorWait());
    }

    IEnumerator CorWait()
    {
        float deltaTime = Time.deltaTime;
        float timerProcess = 0;
        while (timerProcess < 1.0f)
        {
            timerProcess += deltaTime;
            yield return null;
        }

        StartCoroutine(CorMove());
    }

    IEnumerator CorMove()
    {
        float deltaTime = Time.fixedDeltaTime;
        animator.SetBool("Attack", false);

        float attackTimer = Random.Range(attackTimerMin, attackTimerMax);
        float attackTimerProcess = 0;

        float moveSpeed = 1;


        bool isAttack = false;

        while (true)
        {
            if (isAttack == false)
            {
                attackTimerProcess += deltaTime;

                if (attackTimerProcess > attackTimer * patternRivision)
                {
                    if (Random.Range(0, 100) < attackChance)
                    {
                        isAttack = true;

                        rotation = Random.Range(35, 145) + (Random.Range(0, 2) + 180);
                        direction = Quaternion.Euler(0, 0, rotation) * Vector3.left;

                        body.flipY = (rotation > 90 && rotation < 270) ? true : false;

                        body.transform.rotation = Quaternion.Euler(0, 0, rotation);
                        moveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);

                        attackTimer = Random.Range(attackTimerMin, attackTimerMax); 

                        animator.SetBool("Attack", isAttack);
                    }
                    attackTimerProcess = 0;
                }
            }
            else // isAttack == true
            {
                attackTimerProcess += deltaTime;
                transform.Translate(direction * moveSpeed * deltaTime);

                if (attackTimerProcess > attackTimer)
                {
                    isAttack = false;

                    attackTimer = Random.Range(attackTimerMin, attackTimerMax);
                    attackTimerProcess = 0;

                    if (rotation > 90 && rotation < 270)
                    {
                        body.flipY = true;
                        body.transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    else
                    {
                        body.flipY = false;
                        body.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    animator.SetBool("Attack", isAttack);
                }

            }
            yield return new WaitForSeconds(deltaTime);
        }
    }

    int up = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Map") == true)
        {
            direction.y *= -1;
            float angle = Quaternion.FromToRotation(Vector3.left, direction).eulerAngles.z;
            body.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
