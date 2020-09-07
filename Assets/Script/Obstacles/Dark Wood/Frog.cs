using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public Rigidbody2D rigid2D;

    public SpriteRenderer body;
    public Animator animator;

    public FrogBomb frogBombPrefab;

    [Header("- Pattern")]
    public float jumpChance = 50f;

    [Space(20)]
    [Range(0.2f, 0.5f)]
    public float maxJumpWidth = 0.4f;
    private float jumpWidth = 0.2f;

    [Range(0.5f, 5f)]
    public float maxJumpHeight = 1f;
    private float jumpHeight = 0.5f;


    Vector3 myStartPos = Vector3.zero;

    // Triggers
    [SerializeField]
    bool isJumping = false;
    bool isCreateBomb = false;

    private void Awake()
    {
        myStartPos = transform.localPosition;
        frogBombPrefab = Instantiate(frogBombPrefab, Vector3.zero, Quaternion.Euler(0,0, Random.Range(0,360)), transform.parent);
    }

    private void OnEnable()
    {
        transform.localPosition = myStartPos;
        StartCoroutine(CorIdle());
    }

    IEnumerator CorIdle()
    {
        float deltaTime = Time.fixedDeltaTime;
        isJumping = false;


        float resetTimer = 2.5f;
        float resetTimerProcess = 0;

        while (true)
        {
            if (isJumping == false)
            {
                if (Random.Range(0, 100) < jumpChance)
                {
                    animator.SetBool("Jump", true);
                    resetTimerProcess = 0;
                    isCreateBomb = false;
                }
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                resetTimerProcess += deltaTime;

                if(rigid2D.velocity.y < jumpHeight * 0.25f)
                {
                    animator.SetBool("JumpFall", true);
                    animator.enabled = true;
                }

                // Glitch Reset
                if(resetTimerProcess > resetTimer)
                {
                    isJumping = false;
                    animator.SetBool("JumpFall", false);
                    animator.SetBool("Jump", false);
                }

                yield return new WaitForSeconds(deltaTime);
            }
        }
    }

    private void SetAddForceForJump()
    {
        Vector2 jumpDirection =
            new Vector2(Random.Range(jumpWidth, maxJumpWidth),
                        Random.Range(jumpHeight, maxJumpHeight) * rigid2D.gravityScale);

        jumpDirection.x *= (Random.Range(0, 2) == 0) ? -1 : 1;
        if (jumpDirection.x > 0) body.flipX = true;
        else body.flipX = false;

        rigid2D.AddForce(jumpDirection, ForceMode2D.Impulse);
        isJumping = true;
    }

    private void SetBombActive()
    {
        if (frogBombPrefab.gameObject.activeSelf == false && isCreateBomb == false)
        {
            isCreateBomb = true;
            frogBombPrefab.transform.position = transform.position;
            frogBombPrefab.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (animator.GetBool("JumpFall") == true && collision.collider.CompareTag("Map"))
        {
            isJumping = false;
            animator.SetBool("JumpFall", false);
            animator.SetBool("Jump", false);
        }
    }
}
