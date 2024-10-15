using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] private string Sound_Type; // SoundEffectValue or SoundMusicValue
    [SerializeField] private float softValue = 1f;
    AudioSource _sound;
     
    private float _volume = 1f;
    private void Awake() {
        _sound = transform.GetComponent<AudioSource>();
    } 
    public void ChangeValueAudio(){
        float Offset = 1f*PlayerPrefs.GetFloat(Sound_Type);
        _sound.volume = (Offset/softValue)*_volume;
    }
    void Update()
    {
        ChangeValueAudio();
    }
    public void PlayAduio(){
        _sound.Stop();
        _sound.Play();
    }
    public void StopAduio(){
        _sound.Stop();
    }
}