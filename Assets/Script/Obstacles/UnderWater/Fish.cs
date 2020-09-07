using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public SpriteRenderer body;
    public float moveSpeedRivision = 0.5f;
    public float angle = 0;
    private float startAngle = 0;

    [SerializeField]
    float deltaTime = 0.02f;

    private bool isStuck = false;

    private Vector3 myStartPos = Vector3.zero;

    private void Awake()
    {
        myStartPos = transform.localPosition;
        startAngle = angle;
    }

    private void OnEnable()
    {
        transform.localPosition = myStartPos;
        angle = startAngle;
        deltaTime = Time.fixedDeltaTime;
        StartCoroutine(CorMove());
    }


    [SerializeField]
    Vector3 direction = Vector3.up;
    IEnumerator CorMove()
    {
        StartCoroutine(CorRotateAngle());

        float stuckTimer = 2.0f;
        float stuckTimerProcess = 0;

        float slow = 0f;

        stuckTimer = Random.Range(2.0f, 4.0f);

        while (true)
        {
            if (angle % 360 > 180)
            {
                body.flipX = false;
            }
            else
            {
                body.flipX = true;
            }

            if (isStuck == false)
            {
                slow = (stuckTimer - stuckTimerProcess) * moveSpeedRivision;
                direction = Quaternion.Euler(0, 0, angle) * Vector3.up * deltaTime * slow;
                transform.Translate(direction, Space.Self);

                stuckTimerProcess += deltaTime;

                if(stuckTimerProcess > stuckTimer)
                {
                    isStuck = true;
                }

                yield return new WaitForSeconds(deltaTime);
            }
            else
            {
                isStuck = false;
                stuckTimer = Random.Range(2.0f, 4.0f);
                stuckTimerProcess = 0;
                angle = Random.Range(0, 360f);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
    IEnumerator CorRotateAngle()
    {
        bool constAngle = false;
        float angleDirection = 1f;
        float angleRivision = 1f;

        float timer = Random.Range(0.2f, 0.5f);
        float timerProcess = 0;

        while (true)
        {
            timerProcess += deltaTime;
            if (timerProcess > timer)
            {
                timer = Random.Range(0.2f, 0.5f);
                constAngle = true;
                timerProcess = 0;
            } 

            if (constAngle == false)
            {
                angle += angleDirection * angleRivision;
                yield return new WaitForSeconds(deltaTime);
            }
            else
            {
                angleDirection = (Random.Range(0, 2) == 0) ? 1 : -1;
                angleRivision = Random.Range(0.2f, 1f);                
                constAngle = false;
                yield return new WaitForSeconds(deltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Map") == true)
        {
            // Stop
            if (isStuck == false)
            {
                if (angle % 360 >= 180)
                {
                    angle -= 180 + (Random.Range(-45f, 45f));
                }
                else
                {
                    angle += 180 + (Random.Range(-45f, 45f));
                }
                isStuck = true;
            }
        }
    }
}
