using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System;

public class GameManager : NetworkBehaviour
{
    [SerializeField] Text _timerTxt;
    [SerializeField] Text _scoreP1Txt;
    [SerializeField] Text _scoreP2Txt;
    [SerializeField] int[] _players;
    [SerializeField] int _scoreP1;
    [SerializeField] int _scoreP2;

    float _minSeconds = 0;
    float _maxSeconds = 60;
    [Networked] [SerializeField] float _secondstimer { get; set; }
    [Networked] [SerializeField] int _minutesTimer { get; set; }

 
    public override void FixedUpdateNetwork()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        if (_players.Length <= 1) return;

        if(Object.HasStateAuthority) _secondstimer -= Runner.DeltaTime;

        if (_secondstimer <= _minSeconds)
        {
            _minutesTimer--;
            _secondstimer = _maxSeconds;
        }

        if (_minutesTimer <= 1) _timerTxt.color = Color.red;

        _timerTxt.text = _minutesTimer.ToString() + " : " + Mathf.Floor(_secondstimer).ToString("00");

    }

}
