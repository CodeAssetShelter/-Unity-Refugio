using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartFishCreator : MonoBehaviour
{
    public GameObject dartFishPrefab;
    public GameObject checkDistance;
    public int dartFishMax = 5;
    public float spawnDelayMax = 3.0f;
    private float spawnDelayMin = 1.0f;
    private float spawnDelay = 1.0f;

    public GameObject start, end;

    private List<GameObject> dartFishes;
    private float deltaTime = 0.02f;
    private float patternRivision = 1.0f;
    // TEMP
    private GameObject player;
    [SerializeField]
    private float distance = 3;
    private void Awake()
    {
        dartFishes = new List<GameObject>();
    }

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player");

        patternRivision = (MapManager.Instance == null) ? 1.0f : MapManager.Instance.GetPatternRivision();

        deltaTime = Time.fixedDeltaTime;
        if (dartFishes.Count < dartFishMax)
        {
            StartCoroutine(CorCreateDartFish(false));
        }
        else
        {
            StartCoroutine(CorCreateDartFish(true));
        }
        StartCoroutine(CorLaunchDartFish());
    }

    IEnumerator CorCreateDartFish(bool isFinished)
    {
        WaitForSeconds wait = new WaitForSeconds(deltaTime);
        if (isFinished == false)
        {
            Vector3 temp = dartFishPrefab.transform.localPosition;
            temp.y = 15;
            //temp.y = Random.Range(start.transform.localPosition.y, end.transform.localPosition.y);
            while (dartFishes.Count < dartFishMax)
            {
                GameObject dart = Instantiate(dartFishPrefab, temp, Quaternion.identity, transform);
                //temp = dartFishPrefab.transform.localPosition;
                //temp.y = Random.Range(start.transform.localPosition.y, end.transform.localPosition.y);
                //dart.transform.localPosition = temp;
                dart.gameObject.SetActive(false);
                dartFishes.Add(dart);
                yield return wait;
            }
        }
        else
        {
            for (int i = 0; i < dartFishes.Count; i++)
            {
                dartFishes[i].SetActive(false);
            }
            yield break;
        }
    }

    IEnumerator CorLaunchDartFish()
    {
        spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);

        WaitForSeconds wait = new WaitForSeconds(spawnDelay * patternRivision);

        yield return wait;
        while (true)
        {
            if(Vector3.Distance(checkDistance.transform.position, player.transform.position) <= distance)
            {
                //Debug.Log("Too close");
                yield break;
            }

            spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);

            for (int i = 0; i < dartFishes.Count; i++)
            {
                GameObject temp = dartFishes[i];
                if (temp.activeSelf == false)
                {
                    Vector3 pos = dartFishPrefab.transform.localPosition;
                    pos.y = Random.Range(start.transform.localPosition.y, end.transform.localPosition.y);
                    temp.transform.localPosition = pos;
                    temp.SetActive(true);
                    break;
                }
            }
            yield return wait;
        }
    }
}
