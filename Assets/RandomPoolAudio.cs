using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPoolAudio : MonoBehaviour
{
    public void RandomPool(Vector3 position)
    {
        transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Pool>().GetPool(position);
    }
}
