using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Mine : MonoBehaviour
{
    public PathCreator myPathCreator;
    public GameObject movableObject;

    private float moveSpeedMin = 0.5f;
    [SerializeField]
    private float moveSpeedMax = 3.0f;
    private float moveSpeed = 1.0f;
    private Vector3 initLocalPosition;

    private void Awake()
    {
        myPathCreator = GetComponent<PathCreator>();
        initLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        transform.localPosition = initLocalPosition;
        moveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);
        StartCoroutine(CorMove());   
    }

    IEnumerator CorMove()
    {
        float deltaTime = Time.fixedDeltaTime;
        float moveProcess = 0;
        while (true)
        {
            moveProcess += deltaTime * moveSpeed;
            movableObject.transform.position =
                myPathCreator.path.GetPointAtDistance(moveProcess, EndOfPathInstruction.Reverse);
             
            yield return new WaitForSeconds(deltaTime);
        }
    }
}
