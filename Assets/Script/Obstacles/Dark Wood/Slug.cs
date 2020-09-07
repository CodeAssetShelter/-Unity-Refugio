using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer body;
    public SlugBubble slugBubblePrefab;
    public Rigidbody2D rigid2D;

    [Space(20)]
    public float moveSpeedMax = 0.5f;
    private float moveSpeed = 0.2f;
    private float moveRivision = 1.0f;

    [Range(0, 10)]
    public int bubbleChance = 8;

    private int maxBubble = 2;
    private List<SlugBubble> bubbles;
    float deltaTime = 0.02f;
    float moveDirection = -1;

    Vector3 myStartPos = Vector3.zero;
    SlugBubble bubbleIndexHolder = null;
    float patternRivision = 1.0f;
    // Attack Triggers
    bool isAttack = false;

    private void Awake()
    {
        myStartPos = transform.localPosition;
        bubbles = new List<SlugBubble>();

        for(int i = 0; i < maxBubble; i++)
        {
            bubbles.Add(Instantiate(slugBubblePrefab, transform.position, Quaternion.identity, transform.parent));
        }
    }

    Coroutine corStopHolder;
    private void OnEnable()
    {
        animator.speed = Random.Range(0.7f, 1.2f);

        patternRivision = (MapManager.Instance == null) ? 1.0f : MapManager.Instance.GetPatternRivision();

        transform.localPosition = myStartPos;
        moveSpeed = Random.Range(0.2f, moveSpeedMax);
        deltaTime = Time.fixedDeltaTime;

        corStopHolder = StartCoroutine(CorMove());

        for (int i = 0; i < bubbles.Count; i++) bubbles[i].gameObject.SetActive(false);
    }

    IEnumerator CorMove()
    {
        float attackTimer = 0;
        animator.SetBool("Move", true);
        while (true)
        {
            if (isAttack == false)
            {
                transform.Translate(deltaTime * moveSpeed * moveRivision * moveDirection, 0, 0, Space.Self);
            }
            else 
            {
                attackTimer += deltaTime;
                if(attackTimer > 1.5f * patternRivision)
                {
                    isAttack = false;
                    attackTimer = 0;
                    animator.SetBool("Move", true);
                    moveSpeed = Random.Range(0.2f, moveSpeedMax);
                }
            }
            yield return new WaitForSeconds(deltaTime);
        }
    }

    private void SetMoveRivision(float rivision = 1.0f)
    {
        moveRivision = rivision;
    }

    private void SetMoveDirection()
    {
        moveDirection = (Random.Range(0, 2) == 0) ? -1 : 1;
        body.flipX = (moveDirection == -1) ? false : true;
    }

    private void EndAnimation()
    {
        for (int i = 0; i < bubbles.Count; i++)
        {
            bubbleIndexHolder = bubbles[i];
            if (bubbleIndexHolder.gameObject.activeSelf == false)
            {
                break;
            }
            else
            {
                if (i == bubbles.Count - 1)
                {
                    return;
                }
            }
        }
        if (Random.Range(0, 10) < bubbleChance)
        {
            animator.SetBool("Move", false);
            isAttack = true;

            // isAttack == true
            Vector3 vector = transform.localPosition;
            if(rigid2D.gravityScale > 0)
                vector.x += moveDirection * body.bounds.size.x;
            else
            {
                vector.x -= moveDirection * body.bounds.size.x;
            }
            bubbleIndexHolder.transform.localPosition = vector;
            bubbleIndexHolder.InitBubble(rigid2D.gravityScale);
        }
    } 
}

