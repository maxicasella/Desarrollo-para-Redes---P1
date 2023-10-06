using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Text _timerTxt;
    [SerializeField] Text _localScoreTxt;
    [SerializeField] Text _proxyScoreTxt;
    [SerializeField] Text _finishTxt;

    [SerializeField] GameObject _finishCanvas;

    [SerializeField] List<PlayerInputs> _actualPlayers;
    [SerializeField] List<WeaponController> _actualWeapons;

    float _minSeconds = 0;
    float _maxSeconds = 60;
    int _tempScore;

    [Networked] [SerializeField] int _localScore { get; set; }
    [Networked] [SerializeField] int _proxyScore { get; set; }
    [Networked] [SerializeField] float _secondstimer { get; set; }
    [Networked] [SerializeField] int _minutesTimer { get; set; }

    [Networked] [SerializeField] int _maxScore { get; set; }

    int _spawn;
    int _lastSpawn { get; set; }

    void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
    }

    public override void FixedUpdateNetwork()
    {
        UpdateTimer();
        PrintScore();
        EndGame();
    }

    public void AddPlayers(PlayerInputs player)
    {
        _actualPlayers.Add(player);
    }

    public List<PlayerInputs> Players()
    {
        return _actualPlayers;
    }

    public void AddWeapons(WeaponController player)
    {
        _actualWeapons.Add(player);
    }

    public bool CheckConnectedPlayers() 
    {
        var b = false;
        if (_actualPlayers.Count == 2) return b = true;
        else return b  = false;
    }

    public List<WeaponController> Weapons()
    {
        return _actualWeapons;
    }

    public void AddScore(PlayerInputs player, int score) //REVISAR
    {
        _tempScore += score;

        if (player.HasStateAuthority) _localScore += _tempScore;
        else _proxyScore += _tempScore;

        _tempScore = 0;
    }

    void PrintScore ()
    {
        _localScoreTxt.text = _localScore.ToString();
        _proxyScoreTxt.text = _proxyScore.ToString();
    }

    void UpdateTimer()
    {
        if (_actualPlayers.Count <= 1) return;

        if(Object.HasStateAuthority) _secondstimer -= Runner.DeltaTime;

        if (_secondstimer <= _minSeconds)
        {
            _minutesTimer--;
            _secondstimer = _maxSeconds;
        }

        if (_minutesTimer <= 1) _timerTxt.color = Color.red;

        _timerTxt.text = _minutesTimer.ToString() + " : " + Mathf.Floor(_secondstimer).ToString("00");

    }

    void EndGame()
    {
        if(_minutesTimer == 0 && _minSeconds == 0)
        {
            Time.timeScale = 0;
            _timerTxt.gameObject.SetActive(false);
            if (_localScore > _proxyScore)
            {
                _finishCanvas.SetActive(true);
                _finishTxt.text = "Team A Win";
                if (HasStateAuthority) _finishTxt.color = Color.green;
                else _finishTxt.color = Color.red;
            }
            else
            {
                _finishCanvas.SetActive(true);
                _finishTxt.text = "Team B Win";
                if (HasStateAuthority) _finishTxt.color = Color.red;
                else _finishTxt.color = Color.green;
            }
        }

        if (_localScore >= _maxScore && _proxyScore < _maxScore)
        {
            Time.timeScale = 0;
            _timerTxt.gameObject.SetActive(false);
            _finishCanvas.SetActive(true);
            _finishTxt.text = "Team A Win";
            if (HasStateAuthority) _finishTxt.color = Color.green;
            else _finishTxt.color = Color.red;
        }
        else if (_proxyScore >= _maxScore && _localScore < _maxScore)
        {
            Time.timeScale = 0;
            _timerTxt.gameObject.SetActive(false);
            _finishCanvas.SetActive(true);
            _finishTxt.text = "Team B Win";
            if (HasStateAuthority) _finishTxt.color = Color.red;
            else _finishTxt.color = Color.green;
        }
    }

    public int SpawnPoints()
    {
        Debug.Log("Last Init" + _lastSpawn);
        _spawn = UnityEngine.Random.Range(0, 2);
        Debug.Log("Spawn:" + _spawn);
        if (_spawn == _lastSpawn)
        {
            if (_spawn == 1) _spawn = 0;
            else _spawn = 1;
        }

        _lastSpawn = _spawn;
        Debug.Log("Last Finish" + _lastSpawn);
        return _spawn;
    }
}
