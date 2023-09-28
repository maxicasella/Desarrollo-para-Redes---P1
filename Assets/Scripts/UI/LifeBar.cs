using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class LifeBar : NetworkBehaviour
{
    [SerializeField] Image _lifeBarFill;
    [SerializeField] float _yOffset;

    Transform _target;

    public LifeBar SetTarget(PlayerInputs player)
    {
        _target = player.transform;

        player.OnLifeUpdate += UpdateLifeBar;

        return this;
    }

    void UpdateLifeBar(float amount)
    {
        _lifeBarFill.fillAmount = amount;
    }

    public void UpdatePosition()
    {
        transform.position = _target.position + Vector3.up * _yOffset;
    }
}
