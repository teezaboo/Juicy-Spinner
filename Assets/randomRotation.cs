using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomRotation : MonoBehaviour
{
    [SerializeField] private float _maxRotation = 141f;
    [SerializeField] private float _minRotation = 4f;
    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(_maxRotation, _minRotation));
    }
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(_maxRotation, _minRotation));
    }
}
