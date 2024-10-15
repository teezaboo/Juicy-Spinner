using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _effectSlider;
    [SerializeField] private TextMeshProUGUI _textSliderMusic;
    [SerializeField] private TextMeshProUGUI _textSliderEffect;
    //[SerializeField] private GameObject _EnterName;
   // public TextMeshProUGUI myName;
  //  public TextMeshProUGUI myScore;

    public static AudioManager Instance { get; private set; }


    private void Awake()
    {/*
        _EnterName.SetActive(true);
        
        if ((PlayerPrefs.HasKey("NamePlayer"))){
            if(_EnterName != null){
                _EnterName.SetActive(false);
                myName.text = PlayerPrefs.GetString("NamePlayer");
                myScore.text = PlayerPrefs.GetInt("ScorePlayer").ToString();
            }
        }
        */
        
        if (!(PlayerPrefs.HasKey("First"))){
            PlayerPrefs.SetInt("First", 0);
            PlayerPrefs.SetFloat("SoundEffectValue", 0.5f);
            PlayerPrefs.SetFloat("SoundMusicValue", 0.5f);
            _musicSlider.value = 0.5f;
            _effectSlider.value = 0.5f;
            _textSliderMusic.text = "50%";
            _textSliderEffect.text = "50%";
        }
        else{
            _musicSlider.value = PlayerPrefs.GetFloat("SoundMusicValue");
            _effectSlider.value = PlayerPrefs.GetFloat("SoundEffectValue");
            _textSliderMusic.text = (PlayerPrefs.GetFloat("SoundMusicValue")*100f).ToString("0")+ " %";
            _textSliderEffect.text = (PlayerPrefs.GetFloat("SoundEffectValue")*100f).ToString("0")+ " %";
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _musicSlider.onValueChanged.AddListener((v) =>{
            _textSliderMusic.text = (v*100).ToString("0") + " %";    
            PlayerPrefs.SetFloat("SoundMusicValue", v);
        });
        _effectSlider.onValueChanged.AddListener((v) =>{
            _textSliderEffect.text = (v*100).ToString("0") + " %";    
            PlayerPrefs.SetFloat("SoundEffectValue", v);
        });
    }
}
