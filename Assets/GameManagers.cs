    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    public class GameManagers : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dieText;
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private MonsterSpawner _monsterSpawner;
        public string _textHr = "00";
        public string _textMin = "00";
        public string _textSec = "00";
        public string _textMinSec = "00";
        public int DieCount = 0;
        public int wave = 1;
        public bool IsStopGame = false;
        public int hr = 0;
        public int min = 0;
        public int sec = 0;
        public int minSec = 0;
        private float stackTimeBigBossSpawn;
        void Awake(){
            _dieText.text = "X0";
            _waveText.text = "WAVE "+wave+" - "+_textMin+":"+_textSec+":"+_textMinSec;
        }
        void Update()
        {
            _dieText.text = "X"+DieCount;
            if(IsStopGame == true) return;
            if(hr > 0){
            _waveText.text = "WAVE "+wave+" - "+_textHr+":"+_textMin+":"+_textSec+":"+_textMinSec;
            }else{
                _waveText.text = "WAVE "+wave+" - "+_textMin+":"+_textSec+":"+_textMinSec;
            }
            minSec++;
            if(wave >= 10 && wave < 20){
                if(minSec % 45 == 0){
                   
                }
            }else if(wave >= 20 && wave < 30){
                if(minSec % 30 == 0){
                   
                }
            }else if(wave >= 30){
                if(minSec % 15 == 0){
                   
                }
            }
            if(minSec < 10)
            {
                _textMinSec = "0" + minSec.ToString();
            }
            else
            {
                _textMinSec = minSec.ToString();
            }
            if(minSec == 60)
            {
                minSec = 0;
                sec++;
                if(wave < 5){
                    if(sec % 4 == 0){
                       
                    }
                }else if(wave >= 5 && wave < 10){
                    if(sec % 2 == 0){
                       
                    }
                }
                if(sec < 10)
                {
                    _textSec = "0" + sec.ToString();
                }
                else
                {
                    _textSec = sec.ToString();
                    }
                if(minSec < 10)
                {
                    _textMinSec = "0" + minSec.ToString();
                }
                else
                {
                    _textMinSec = minSec.ToString();
                }
            }
            if(sec == 60)
            {
                sec = 0;
                min++;
                if(min < 10)
                {
                    _textMin = "0" + min.ToString();
                }
                else
                {
                    _textMin = min.ToString();
                }
                if(sec < 10)
                {
                    _textSec = "0" + sec.ToString();
                }
                else
                {
                    _textSec = sec.ToString();
                }
            }
            if(min == 60)
            {
                min = 0;
                hr++;
                if(hr < 10)
                {
                    _textHr = "0" + hr.ToString();
                }
                else
                {
                    _textHr = hr.ToString();
                }
                if(min < 10)
                {
                    _textMin = "0" + min.ToString();
                }
                else
                {
                    _textMin = min.ToString();
                }
            }
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
