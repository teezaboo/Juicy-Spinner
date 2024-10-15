using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClick : MonoBehaviour
{
    private Pool _poolAudio;
    public void Awake()
    {
        _poolAudio = GetComponent<Pool>();
    }
    public void PlayAudio()
    {
        _poolAudio.GetPool(transform.position);
    }

}
