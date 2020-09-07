using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownBlocks : MonoBehaviour
{
    [Header("- Original Prefab")]
    public Block blockPrefab;

    [Header("- Block spawn sector")]
    public GameObject start;
    public GameObject end;

    [Header("- Sprite")]
    public Sprite[] sprites;

    [Header("- Setting values")]
    public int blocks = 10;
    public float fallDownMaxSpeed = 3.0f;

    // private
    List<Block> blockList;

    // Start is called before the first frame update
    void Awake()
    {
        blockList = new List<Block>();
    }

    private void OnEnable()
    {
        StartCoroutine(CorResetBlocks());
    }
    IEnumerator CorResetBlocks()
    {
        if (blockList.Count != 0)
        {
            int idx = 0;
            Vector3 spawnPosition = Vector3.zero;
            spawnPosition.y = this.transform.position.y;
            while (idx < blockList.Count)
            {
                blockList[idx++].transform.localPosition = spawnPosition;
                yield return null;
            }
        }
        StartCoroutine(CorInitFallDownBlocks());        
    }
    IEnumerator CorInitFallDownBlocks()
    {
        int index = 0;

        Vector3 spawnPosition = Vector3.zero;
        Block block = null;
        Sprite sprite;

        spawnPosition.y = start.transform.position.y;

        WaitForSeconds wait = new WaitForSeconds(0.8f);
        // Create blocks
        while (index < blocks)
        {
            spawnPosition.x = Random.Range(start.transform.localPosition.x, end.transform.localPosition.x);

            if (blockList.Count < blocks)
            {
                block = Instantiate(blockPrefab, spawnPosition, Quaternion.identity, this.transform);
                blockList.Add(block);
            }
            else
            {
                block = blockList[index];
                block.transform.localPosition = spawnPosition;
            }
            sprite = sprites[Random.Range(0, sprites.Length)];
            block.Init(start.transform.localPosition.x, end.transform.localPosition.x, fallDownMaxSpeed, sprite);
            index++;

            yield return new WaitForSeconds(0.8f);
        }
    }
}
