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
    [SerializeField] Text _scoreP1Txt;
    [SerializeField] Text _scoreP2Txt;
    [SerializeField] int _scoreP1;
    [SerializeField] int _scoreP2;

    [SerializeField] List<PlayerInputs> _actualPlayers;
    [SerializeField] List<WeaponController> _actualWeapons;

    float _minSeconds = 0;
    float _maxSeconds = 60;
    [Networked] [SerializeField] float _secondstimer { get; set; }
    [Networked] [SerializeField] int _minutesTimer { get; set; }


    void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
    }

    public override void FixedUpdateNetwork()
    {
        UpdateTimer();
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

    public List<WeaponController> Weapons()
    {
        return _actualWeapons;
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

}
