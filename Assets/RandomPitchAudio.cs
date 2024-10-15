using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitchAudio : MonoBehaviour
{
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f;
    public void OnEnable()
    {
        GetComponent<AudioSource>().pitch = Random.Range(minPitch, maxPitch);
    }
}
