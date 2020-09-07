using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public Rigidbody2D rigid2D;
    public SpriteRenderer body;

    [Space(20)]
    public OctopusBullet bulletPrefab;
    private List<OctopusBullet> bullets;
    public int maxBullet = 4;

    [Space(20)]
    public GameObject gravityObject;
    private Vector3 gravity = Vector3.zero;
    [Space(20)]
    public Animator animator;

    [Space(20)]
    [Header("- Pattern Timer")]
    public float patternTimerMax = 2.5f;
    private float patternTimerMin = 1.0f;
    private float patternTimer = 1.0f;
    private float patternRivision = 1.0f;

    [Space(20)]
    [Header("- Bullet")]
    public float bulletSpeed = 1.0f;
    public float bulletDistance = 3.0f;

    private Vector3 myStartPos;
    private float deltaTime = 0.02f;

    private bool attackMode = false;
    private void Awake()
    {
        bullets = new List<OctopusBullet>();
        myStartPos = transform.localPosition;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        transform.localPosition = myStartPos;
        deltaTime = Time.fixedDeltaTime;

        patternRivision = (MapManager.Instance == null) ? 1.0f : MapManager.Instance.GetPatternRivision();

        if (bullets.Count < maxBullet)
        {
            StartCoroutine(CorCreateBullet(false));
        }
        else
        {
            StartCoroutine(CorCreateBullet(true));
        }
        StartCoroutine(CorGravity());
        StartCoroutine(CorWalk());
    }

    IEnumerator CorCreateBullet(bool isReset)
    {
        if (isReset == false)
        {
            while (bullets.Count < maxBullet)
            {
                OctopusBullet temp = Instantiate(bulletPrefab, transform.position, Quaternion.identity, transform.parent);
                temp.gameObject.SetActive(false);
                bullets.Add(temp);
                yield return null;
            }
        }
        else
        {
            int i = 0;
            while (i < bullets.Count)
            {
                bullets[i++].gameObject.SetActive(false);
                yield return null;
            }
        }
    }

    IEnumerator CorGravity()
    {
        gravity = gravityObject.transform.position - transform.position;
        gravity = gravity.normalized;

        WaitForSeconds wait = new WaitForSeconds(deltaTime);
        while (true)
        {
            rigid2D.AddForce(gravity, ForceMode2D.Force);
            yield return wait;
        }
    }

    IEnumerator CorWalk()
    {
        Vector3 walkDirection = Vector3.zero;
        walkDirection.x = (Random.Range(0,2) == 0) ? 1 : -1;
        body.flipX = (walkDirection.x == 1) ? true : false;

        patternTimer = Random.Range(patternTimerMin, patternTimerMax);

        float timer = 0;

        WaitForSeconds wait = new WaitForSeconds(deltaTime);

        animator.SetBool("Walk", true);
        while (true)
        {
            transform.Translate(walkDirection * deltaTime, Space.Self);
            timer += deltaTime;
            if(timer > patternTimer * patternRivision)
            {
                StartCoroutine(CorIdle());
                yield break;
            }

            yield return wait;
        }
    }

    IEnumerator CorIdle()
    {
        animator.SetBool("Walk", false);
        patternTimer = Random.Range(patternTimerMin, patternTimerMax);
        float timer = 0;

        WaitForSeconds wait = new WaitForSeconds(deltaTime);

        if(bullets.Exists(obj => obj.gameObject.activeSelf == true) == false)
        {
            attackMode = true;
        }
        else
        {
            attackMode = false;
        }

        while (true)
        {
            if (attackMode == true)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    OctopusBullet temp = bullets[i];
                    Vector3 direction = Quaternion.Euler(0, 0, (180 / (maxBullet + 1)) * (i + 1)) * Vector3.right;
                    direction *= gravity.y * -1;
                    temp.transform.position = transform.position;
                    temp.gameObject.SetActive(true);
                    temp.InitBullet(bulletSpeed, bulletDistance, direction);
                }
                attackMode = false;
            }

            timer += deltaTime;
            if (timer > patternTimer)
            {
                StartCoroutine(CorWalk());
                yield break;
            }
            yield return wait;
        }
    }
}
