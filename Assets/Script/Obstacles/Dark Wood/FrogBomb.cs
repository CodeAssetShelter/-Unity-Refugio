using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBomb : MonoBehaviour
{
    public new CircleCollider2D collider2D;

    public void SetActiveCollider(int active)
    {
        collider2D.enabled = (active == 1) ? true : false;
    }

    private void SetActive(int active)
    {
        gameObject.SetActive((active == 1) ? true : false);
    }
}
