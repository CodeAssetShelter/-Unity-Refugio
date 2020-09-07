using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gluplant : MonoBehaviour
{
    public OctopusBullet bulletPrefab;
    public SpriteRenderer body;

    private const int maxBullet = 2;
    private List<OctopusBullet> bullets;

    [Space(20)]
    public Animator animator;

    [Space(20)]
    [Range(1.0f, 2.0f)]
    public float attackDelayMax = 2.0f;
    private float attackDelayMin = 1.0f;

    [Range(0, 100)]
    public int attackChance = 70;

    [Space(20)]
    [Range(0.5f, 1.5f)]
    public float animationSpeedMax = 1.0f;
    private float animationSpeedMin = 0.5f;

    public float bulletSpeedMax = 2.0f;
    public float bulletSpeedMin = 1.0f;

    Vector3 myStartPos;
    bool isAttack = false;
    int flipValue = 0;
    float patternRivision = 1.0f;
    void Awake()
    {
        myStartPos = transform.localPosition;
        bullets = new List<OctopusBullet>();
    }

    private void OnEnable()
    {

        transform.localPosition = myStartPos;
        isAttack = false;
        flipValue = (body.flipY == true) ? 1 : -1;

        patternRivision = (MapManager.Instance == null) ? 1.0f : MapManager.Instance.GetPatternRivision();

        animator.speed = Random.Range(animationSpeedMin, animationSpeedMax);

        if (bullets.Count < maxBullet)
        {
            StartCoroutine(CorCreateBullet());
        }
        StartCoroutine(CorReset());
        StartCoroutine(CorMove());
    }

    IEnumerator CorCreateBullet()
    {
        while (bullets.Count < maxBullet)
        {
            OctopusBullet temp = Instantiate(bulletPrefab, transform);
            temp.gameObject.SetActive(false);

            bullets.Add(temp);
            yield return null;
        }
    }

    IEnumerator CorReset()
    {
        if (bullets.Count == 0) yield break;

        int i = 0;
        while (i < bullets.Count)
        {
            bullets[i++].gameObject.SetActive(false);
            yield return null;
        }
    }

    IEnumerator CorMove()
    {
        float deltaTime = Time.fixedDeltaTime;
        float timerProgress = 0;
        float attackTimer = Random.Range(attackDelayMin, attackDelayMax);

        while (true)
        {
            if (isAttack == false)
            {
                timerProgress += deltaTime;
                if (timerProgress > attackTimer * patternRivision)
                {
                    if (Random.Range(0, 100) < attackChance)
                    {
                        animator.SetBool("Attack", true);
                        isAttack = true;
                    }
                    timerProgress = 0;
                }
            }
            yield return new WaitForSeconds(deltaTime);
        }
    }

    private void SetAttack()
    {
        if (bullets.Count == 0) return;

        for (int i = 0; i < bullets.Count; i++)
        {
            OctopusBullet temp = bullets[i];
            if (temp.gameObject.activeSelf == false)
            {
                Vector3 tempPos = body.transform.localPosition;
                tempPos.x -= body.bounds.extents.x;
                temp.transform.localPosition = tempPos;
                temp.gameObject.SetActive(true);

                tempPos = Quaternion.Euler(0, 0, (Random.Range(0, 2) == 0 ? 45 * flipValue : 0)) * Vector3.left;
                temp.InitBullet(Random.Range(bulletSpeedMin, bulletSpeedMax), 4, tempPos);

                break;
            }
        }
    }

    private void ActiveStateIdle()
    {
        isAttack = false;
        animator.SetBool("Attack", false);
    }
}
