using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugBubble : MonoBehaviour
{   public float horizonSpeedMax = 2f;
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider2D;

    public float disappearDistance = 3.0f;

    private float horizonSpeedMin = 0.2f;
    private float horizonSpeed = 0.2f;

    private float gravity = 1;
    float deltaTime = 0.02f;

    bool startInvincible = false;
    //private void Start()
    //{
    //    StartCoroutine(CorScaleUp());
    //}

    public void InitBubble(float gravity)
    {
        circleCollider2D.enabled = true;

        spriteRenderer.color = Color.white;
        horizonSpeed = Random.Range(horizonSpeedMin, horizonSpeedMax);
        this.gravity = gravity;

        deltaTime = Time.fixedDeltaTime;
        gameObject.SetActive(true);
        StartCoroutine(CorScaleUp());
    }

    IEnumerator CorScaleUp()
    {
        transform.localScale = Vector3.zero;

        float scale = 0;
        float timer = 0;
        Vector3 scaleHolder = Vector3.zero;

        float maxSize = Random.Range(1.2f, 1.8f);
        while (timer < maxSize)
        {
            timer += deltaTime;
            scale = timer;
            scaleHolder.x = scaleHolder.y = scale;
            transform.localScale = scaleHolder;
            yield return new WaitForSeconds(deltaTime);
        }

        scaleHolder.x = scaleHolder.y = maxSize;
        transform.localScale = scaleHolder;

        StartCoroutine(CorMove());
    }

    IEnumerator CorMove()
    {
        Vector3 direction = Vector3.zero;
        direction.x = ((Random.Range(0, 2) == 0) ? -1 : 1) * horizonSpeed;
        direction.y = gravity;

        float xDirection = 1;

        float speedRivision = Random.Range(0.2f, 0.5f);

        float disappearTimer = 0;
        
        while (true)
        {
            transform.Translate(direction * deltaTime * speedRivision, Space.Self);

            if (direction.x > 1)
            {
                xDirection = -1;
            }
            else if (direction.x < -1)
            {
                xDirection = 1;
            }

            direction.x += deltaTime * xDirection;

            disappearTimer += deltaTime;
            if(disappearTimer > disappearDistance)
            {
               StartCoroutine(CorDying());
            }

            yield return new WaitForSeconds(deltaTime);
        }
    }

    IEnumerator CorDying()
    {
        Color color = spriteRenderer.color;
        float timer = 0;

        circleCollider2D.enabled = false;

        while (timer < 1.5f)
        {
            color.a -= deltaTime / 1.5f;
            spriteRenderer.color = color;
            timer += deltaTime;
            yield return new WaitForSeconds(deltaTime);
        }

        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);

    }
}
