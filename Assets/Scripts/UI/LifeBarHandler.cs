using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LifeBarHandler : NetworkBehaviour
{
    public static LifeBarHandler Instance { get; private set; }
    [SerializeField] LifeBar _lifeBarPrefab;

    List<LifeBar> _allLifeBars;

    void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;

        _allLifeBars = new List<LifeBar>();
    }

    public void CreateLifeBar(PlayerInputs target)
    {
        var newLifeBar = Instantiate(_lifeBarPrefab, transform).SetTarget(target);
        _allLifeBars.Add(newLifeBar);

        target.PlayerDead += () =>
        {
            _allLifeBars.Remove(newLifeBar);
            Destroy(newLifeBar.gameObject);
        };
    }
    void LateUpdate()
    {
        foreach (var bar in _allLifeBars) bar.UpdatePosition();  
    }
}
