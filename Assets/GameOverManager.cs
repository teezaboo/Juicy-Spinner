using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private ScenceManagers _scenceManagers;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private TextMeshProUGUI _ScoreText;
    [SerializeField] private TextMeshProUGUI _KillText;
    [SerializeField] private TextMeshProUGUI _WAVEText;
    [SerializeField] private TextMeshProUGUI _timeSurvivledText;
    public void GameOverStart(){
        _ScoreText.text = "Score: "+ _resourceManager._sumScore.ToString();
        _WAVEText.text = "Wave: "+_resourceManager.wave;
        _KillText.text = "Fruit: " + _resourceManager._dieCount.ToString();
        if(_resourceManager.hr == 0){
            if(_resourceManager.min < 10){
                if(_resourceManager.sec < 10){
                    _timeSurvivledText.text = "Time: " + "0" + _resourceManager.min.ToString() + ":0" + _resourceManager.sec.ToString();
                }else{
                    _timeSurvivledText.text = "Time: " + "0" + _resourceManager.min.ToString() + ":" + _resourceManager.sec.ToString();
                }
            }else{
                if(_resourceManager.sec < 10){
                    _timeSurvivledText.text = "Time: " + _resourceManager.min.ToString() + ":0" + _resourceManager.sec.ToString();
                }else{
                    _timeSurvivledText.text = "Time: " + _resourceManager.min.ToString() + ":" + _resourceManager.sec.ToString();
                }
            }
        }else{
            if(_resourceManager.min < 10){
                if(_resourceManager.sec < 10){
                    _timeSurvivledText.text = "Time: " + "0" + _resourceManager.hr.ToString() + ":" + "0" + _resourceManager.min.ToString() + ":0" + _resourceManager.sec.ToString();
                }else{
                    _timeSurvivledText.text = "Time: " + "0" + _resourceManager.hr.ToString() + ":" + "0" + _resourceManager.min.ToString() + ":" + _resourceManager.sec.ToString();
                }
            }else{
                if(_resourceManager.sec < 10){
                    _timeSurvivledText.text = "Time: " + "0" + _resourceManager.hr.ToString() + ":"  + _resourceManager.min.ToString() + ":0" + _resourceManager.sec.ToString();
                }else{
                    _timeSurvivledText.text = "Time: " + "0" + _resourceManager.hr.ToString() + ":" + _resourceManager.min.ToString() + ":" + _resourceManager.sec.ToString();
                }
            }
        }
    }
    public void StartAniGotoGame(){
        _scenceManagers.StartAniGotoMenu();
    }

}
