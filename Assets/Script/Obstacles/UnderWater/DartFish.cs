using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartFish : MonoBehaviour
{
    public CircleCollider2D body;
    public LineRenderer line;

    public float lineSpeed = 20.0f;
    public float moveSpeed = 10.0f;
    public float speedRivisionMax = 1.5f;
    private float speedRivisionMin = 1.0f;
    private float speedRivision = 1.0f;

    float deltaTime = 0.02f;

    private int lineEndIndex = 0;
    private Vector3 myLinePos;
    private Vector3 myFishPos;
    private SpriteRenderer spriteRenderer;


    void Awake()
    {
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        lineEndIndex = line.positionCount - 1;
        myLinePos = line.GetPosition(lineEndIndex);
        myFishPos = body.transform.localPosition;
    }
    private void OnEnable()
    {
        speedRivision = Random.Range(speedRivisionMin, speedRivisionMax);
        awakeDartFish();
    }

    void awakeDartFish()
    {
        deltaTime = Time.fixedDeltaTime;
        body.transform.localPosition = myFishPos;
        line.SetPosition(lineEndIndex, myLinePos);
        line.gameObject.SetActive(true);
        body.enabled = false;

        line.startColor = Color.yellow;
        line.endColor = Color.white;

        gameObject.SetActive(true);
        StartCoroutine(CorFadeFish(true));
    }

    IEnumerator CorLineMove()
    {
        Vector3 start = line.GetPosition(lineEndIndex);        
        spriteRenderer.color = Color.white;

        while (true)
        {
            start = line.GetPosition(lineEndIndex);
            start.x -= lineSpeed * deltaTime * speedRivision;
            line.SetPosition(lineEndIndex, start);
            if (line.GetPosition(lineEndIndex).x < line.GetPosition(0).x)
            {
                line.SetPosition(lineEndIndex, myLinePos);
                StartCoroutine(CorFlashLine());
                yield break;
            }
            yield return new WaitForSeconds(deltaTime);
        }
    }

    IEnumerator CorFlashLine()
    {
        float myWidth = line.startWidth;
        float width = 0;
        float time = 0;
        while (true)
        {
            if (width <= 0)
            {
                width = myWidth;
                line.endWidth = width;
                line.startWidth = width;
            }
            else
            {
                width = 0;
                line.endWidth = width;
                line.startWidth = width;
            }

            time += deltaTime;

            if (time > 0.5f)
            {
                line.endWidth = myWidth;
                line.startWidth = myWidth;
                line.startColor = Color.red;
                line.endColor = Color.red;
                StartCoroutine(CorFishMove());
                yield break;
            }

            yield return new WaitForSeconds(deltaTime);
        }
    }

    IEnumerator CorFishMove()
    {
        float deltaTime = Time.fixedDeltaTime;
        Vector3 myPos = body.transform.localPosition;
        body.enabled = true;
        while (true)
        {
            if (body.transform.localPosition.x < 0)
            {
                body.enabled = false;
                line.gameObject.SetActive(false);
                StartCoroutine(CorFadeFish(false));
            }
            else
            {
                body.transform.Translate(-moveSpeed * deltaTime * speedRivision, 0, 0, Space.Self);
                line.SetPosition(lineEndIndex, body.transform.localPosition);
            }
            yield return new WaitForSeconds(deltaTime);
        }
    }

    IEnumerator CorFadeFish(bool isCreate)
    {
        if (isCreate == false)
        {
            Color color = spriteRenderer.color;
            while (true)
            {
                color.a -= deltaTime / 2;
                spriteRenderer.color = color;
                if (spriteRenderer.color.a <= 0)
                {
                    gameObject.SetActive(false);
                    yield break;
                }
                yield return new WaitForSeconds(deltaTime);
            }
        }
        else
        {
            Color color = spriteRenderer.color;
            while (true)
            {
                color.a += deltaTime / 2;
                spriteRenderer.color = color;
                if (spriteRenderer.color.a >= 1)
                {
                    StartCoroutine(CorLineMove());
                    yield break;
                }
                yield return new WaitForSeconds(deltaTime);
            }
        }
    }
}
