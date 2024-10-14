using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToPool : MonoBehaviour
{
    private GameObject _pool;
    public void timeToBack()
    {
        StartCoroutine(BackToPoolCoroutine());
    }
    private IEnumerator BackToPoolCoroutine()
    {
        yield return new WaitForSeconds(_pool.GetComponent<Pool>().GetTimeToBack());
        gameObject.SetActive(false);
    }
    public void SetPool(GameObject pool)
    {
        _pool = pool;
    }
}
