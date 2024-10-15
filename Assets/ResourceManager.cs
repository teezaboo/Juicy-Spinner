using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public GameOverManager GameOverManager;
    public GameObject settingGame;
    public List<Sprite> ListImgDiff;
    public Image ImgDiff;
    public GameObject gameover;
    public Image _bloodXScore;
        [SerializeField] private TextMeshProUGUI _dieText;
        [SerializeField] private List<TextMeshProUGUI> _waveText;
    public bool IsStopGame = false;
    [SerializeField] private Animator _animatorCountDie;
    [SerializeField] private TextMeshProUGUI _dieCountText;
    public float _dieCount = 0;
    [SerializeField] private TextMeshProUGUI _addScoreText;
    [SerializeField] private TextMeshProUGUI _sumScoreText;
    [SerializeField] private Image _xScoreBar;
    [SerializeField] private GameObject _xScoreObj;
    [SerializeField] private List<TextMeshProUGUI> _textXScore;
    public int _sumScore = 0;
    public int Score = 0;
        public int wave = 1;
        public int DieCount = 0;
    public int xScore = 0;
    [SerializeField] private Animator animatorAddScore;
    [SerializeField] private Animator animatorXScore;
    [SerializeField] private List<TextMeshProUGUI> _textHr;
    [SerializeField] private List<TextMeshProUGUI> _textMin;
    [SerializeField] private List<TextMeshProUGUI> _textSec;
    [SerializeField] private GameObject animatorBloodXScore;
    public int hr = 0;
    public int min = 0;
    public int sec = 0;
    public int minSec = 0;

    public int _EZinWave = 0;
    public int _NormalinWave = 0;
    public int _BadinWave = 0;
    public int _HardinWave = 0;
    public int _VeryHardinWave = 0;
    public int _SuperHardinWave = 0;

    void Awake(){
        _sumScoreText.text = _sumScore.ToString();
        _xScoreObj.SetActive(false);
        _dieCountText.text = "0";
        _dieText.text = "n0";
        foreach (var item in _waveText){
            item.text = "WAVE "+wave;
        }
    }
    public void AddWave(){
        wave++;
        foreach (var item in _waveText){
            item.text = "WAVE "+wave;
        }
        if(wave < _NormalinWave){
            ImgDiff.sprite = ListImgDiff[0];
        }else if(wave < _BadinWave){
            ImgDiff.sprite = ListImgDiff[1];
        }else if(wave < _HardinWave){
            ImgDiff.sprite = ListImgDiff[2];
        }else if(wave < _VeryHardinWave){
            ImgDiff.sprite = ListImgDiff[3];
        }else if(wave < _SuperHardinWave){
            ImgDiff.sprite = ListImgDiff[4];
        }else if(wave >= _SuperHardinWave){
            ImgDiff.sprite = ListImgDiff[5];
        }
    }
    public void AddDieCount(){
        _dieCount++;
        _dieText.text = "n" + _dieCount;
    }

    public void GameOver(){
        _xScoreObj.SetActive(false);
        StartCoroutine(GameOver2());
    }
    IEnumerator GameOver2(){
        yield return new WaitForSeconds(2f);
        gameover.gameObject.SetActive(true);
        IsStopGame = true;
        GameOverManager.GameOverStart();
    }
    public void SettingGame(){
            IsStopGame = !IsStopGame;
            if (IsStopGame)
            {
                settingGame.SetActive(true);
            }
            else
            {
                settingGame.SetActive(false);
            }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SettingGame();
        }
        if(IsStopGame == true) return;
        if(_xScoreObj.activeSelf){
            _xScoreBar.fillAmount -= 0.009f;
            if(_xScoreBar.fillAmount <= 0){
                
                xScore = 0;
                Score = 0;
                _xScoreObj.SetActive(false);
            }
        }

        minSec++;
        if(minSec == 60)
        {
            minSec = 0;
            sec++;
            if(sec < 10)
            {
                foreach (var item in _textSec){
                    item.text = "0" + sec.ToString();
                }
            }
            else
            {
                foreach (var item in _textSec){
                    item.text = sec.ToString();
                }
            }
        }
        if(sec == 60)
        {
            sec = 0;
            min++;
            if(min < 10)
            {
                foreach (var item in _textMin){
                    item.text = "0" + min.ToString();
                }
            }
            else
            {
                foreach (var item in _textMin){
                    item.text = min.ToString();
                }
            }
            if(sec < 10)
            {
                foreach (var item in _textSec){
                    item.text = "0" + sec.ToString();
                }
            }
            else
            {
                foreach (var item in _textSec){
                    item.text = sec.ToString();
                }
            }
        }
        if(min == 60)
        {
            min = 0;
            hr++;
            if(hr < 10)
            {
                foreach (var item in _textHr){
                    item.text = "0" + hr.ToString();
                }
            }
            else
            {
                foreach (var item in _textHr){
                    item.text = hr.ToString();
                }
            }
            if(min < 10)
            {
                foreach (var item in _textMin){
                    item.text = "0" + min.ToString();
                }
            }
            else
            {
                foreach (var item in _textMin){
                    item.text = min.ToString();
                }
            }
        }
    }    public int AddScore(int addScore)
    {
        Score += addScore * (xScore);
        int iScore = Score;
        _sumScore += Score;
        _sumScoreText.text = _sumScore.ToString();
        animatorAddScore.Play("addScore", -1, 0);
        animatorAddScore.Play("addScore");
        Score = 0;
        return iScore;
    }
    public void AddXScore()
    {
        Debug.Log("xScore: " + xScore);
        _xScoreBar.fillAmount = 1f;
        xScore++;
        foreach (var item in _textXScore){
            item.text = xScore.ToString();
        }
        _xScoreObj.SetActive(true);
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        animatorBloodXScore.transform.rotation = randomRotation;
        animatorBloodXScore.GetComponent<Animator>().Play("bloodXScore", -1, 0);
        animatorBloodXScore.GetComponent<Animator>().Play("bloodXScore");
        animatorXScore.Play("xScore_shake", -1, 0);
        animatorXScore.Play("xScore_shake");
        // Create a random rotation for the game object
    }
    public void PlayGame(){
        IsStopGame = false;
        Time.timeScale = 1;
    }
    public void StopGame(){
        IsStopGame = true;
        Time.timeScale = 0;
    }
}
