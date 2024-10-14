using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb_Controller : MonoBehaviour
{
    public bool IsBoombBody = false;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private float knockbackForce = 1;
    void OnEnable()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        StartCoroutine(endAttack());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("monster") && other.gameObject.GetComponent<MonsterController>() != null)
        {
            other.gameObject.GetComponent<MonsterController>().TakeDamage();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().enemyAttack();
        }
    }
    IEnumerator endAttack()
    {
        yield return new WaitForSeconds(0.05f);
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
