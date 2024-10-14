using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceManager : MonoBehaviour
{
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
    [SerializeField] private int xScore = 0;
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

    void Awake(){
        _sumScoreText.text = _sumScore.ToString();
        _xScoreObj.SetActive(false);
        _dieCountText.text = "0";
        _dieText.text = "n0";
        foreach (var item in _waveText){
            item.text = "WAVE "+wave;
        }
    }
    public void AddDieCount(){
        _dieCount++;
        _dieText.text = "n" + _dieCount;
    }

    public void GameOver(){
        IsStopGame = true;
        _xScoreObj.SetActive(false);
        StartCoroutine(GameOver2());
    }
    IEnumerator GameOver2(){
        yield return new WaitForSeconds(2f);
        gameover.gameObject.SetActive(true);
    }
    void Update()
    {
        if(IsStopGame == true) return;
        if(_xScoreObj.activeSelf){
            _xScoreBar.fillAmount -= 0.006f;
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
    }
    public void AddSumScore()
    {
        if(Score == 0) return;
        _sumScore += Score;
        Score = 0;
        _sumScoreText.text = _sumScore.ToString();
        animatorAddScore.Play("addScore", -1, 0);
        animatorAddScore.Play("addScore");
        _xScoreObj.SetActive(false);
    }

    public void AddScore(int addScore = 0)
    {
        if(addScore == 0) return;
        Score += addScore * xScore;
        AddSumScore();
    }
    public void AddXScore()
    {
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
