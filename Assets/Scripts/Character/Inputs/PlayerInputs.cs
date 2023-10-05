using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System;

public class PlayerInputs : NetworkBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _shootSpeed;
    [SerializeField] float _cooldown;
    [SerializeField] float _auraTime;
    [SerializeField] float _auraTimer;
    [SerializeField] float _auracooldown;
    [SerializeField] NetworkMecanimAnimator _myAnim;
    [SerializeField] NetworkRigidbody _myRb;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _projectileSpawnPoint;
    [SerializeField] GameObject _auraObject;
    [SerializeField] Aura _aura;
    [SerializeField] WeaponController _weapons;
    [SerializeField] ParticleSystem _damageParticles;
    [SerializeField] Transform _damagePoint;
    [SerializeField] AudioSource _damageAudio;
    [SerializeField] AudioSource _shootAudio;
    [SerializeField] AudioSource _auraAudio;
    [SerializeField] AudioSource _reloadAudio;
    [SerializeField] ParticleSystem _shootParticles;

    NetworkInputsData _inputs;
    public NetworkBool aura;
   

    public event Action<float> OnLifeUpdate = delegate { };
    public event Action PlayerDead = delegate { };

    [Networked(OnChanged = nameof(LifeChanged))]
    [SerializeField] float _life { get; set; }
    [SerializeField] float _maxLife { get; set; }

    [Networked(OnChanged = nameof(OnFiringChanged))] 
    bool _isFiring { get; set; }

    [Networked(OnChanged = nameof(AuraChanged))] 
    bool _isAura { get; set; }

    [Networked(OnChanged = nameof(OnDamage))]
    bool _isDamage { get; set; }

    bool _isWalking;
    float _lastFiringTime;

    void Start()
    {
        _isWalking = false;
    }

    public override void Spawned()
    {
        GameManager.Instance.AddPlayers(this);
        _maxLife = 200;
        _life = _maxLife;
        LifeBarHandler.Instance.CreateLifeBar(this);
    }
    public override void FixedUpdateNetwork()
    {
        if(GetInput(out _inputs))
        {
            if (_inputs.isFiring) Shoot();
            if (_inputs.isReloading) Reload();
            if (_inputs.isJumping) Jump();
            /*if(!aura)*/ if (_inputs.auraOn) AuraShield();
        }

        if (_isAura)
        {
             _auraTimer += Time.deltaTime;

            if (_auraTimer >= _auraTime)
            {
               _aura.AuraOff();
                _isAura = false;
                _auraTimer = 0;
                AuraReload();
            }
        }

        if (_isWalking) _myAnim.Animator.SetBool("Run", true);
        else _myAnim.Animator.SetBool("Run", false);
       
        Movement(_inputs.xMovement,_inputs.yMovement);
    }


    void AuraShield()
    {
        _isAura = true;
         aura = true;
        _auraAudio.Play();
    }
    static void AuraChanged(Changed<PlayerInputs> changed)
    {
        var updateAura = changed.Behaviour._isAura = true;
        changed.LoadOld();

        var oldAura = changed.Behaviour._isAura;

        if (!oldAura && updateAura) changed.Behaviour._auraObject.SetActive(true);
        else changed.Behaviour._auraObject.SetActive(false);
    }
    void AuraReload()
    {
        _auraTimer += Time.deltaTime;
        if (_auraTimer >= _auracooldown)
        {
            aura = false;
            _auraTimer = 0;
        }
    }

    void Movement(float verticalInput, float horizontalInput)
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement.Normalize();

        _myRb.Rigidbody.velocity = movement * _movementSpeed;

        if (movement.magnitude > 0)
        {
            _isWalking = true;
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360f);
        }
        else _isWalking = false;
    }

    void Jump()
    {
        _myRb.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        _myAnim.Animator.SetTrigger("Jump");
    }

    void Reload()
    {
        _myAnim.Animator.SetTrigger("Reload");
        _reloadAudio.Play();
        _weapons.ReloadAmmo();
    }

    void Shoot()
    {
        if (Time.time - _lastFiringTime < _cooldown) return;

        if (_weapons.currentAmmo <= 0) return;

        _lastFiringTime = Time.time;

        _weapons.UpdateAmo();

        _myAnim.Animator.SetBool("Shoot", true);
        _shootAudio.Play();

        Runner.Spawn(_bulletPrefab, _projectileSpawnPoint.position, transform.rotation);
        
        StartCoroutine(FiringCooldown());
    }
    IEnumerator FiringCooldown()
    {
        _isFiring = true;

        yield return new WaitForSeconds(_cooldown);

        _isFiring = false;
    }
    static void OnFiringChanged(Changed<PlayerInputs> changed)
    {
        var updateFiring = changed.Behaviour._isFiring = true;
        changed.LoadOld(); //Carga el valor anterior de la variable

        var oldFiring = changed.Behaviour._isFiring;

        if (!oldFiring && updateFiring) changed.Behaviour._shootParticles.Play();
    }

    public void TakeDamage(float dmg)
    {
        RPC_TakeDamage(dmg);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_TakeDamage(float dmg)
    {
        //if (aura) return;
        _isDamage = true;
        _life -= dmg;
        _damageAudio.Play();

        OnLifeUpdate(_life / _maxLife);
        _isDamage = false;
        if (_life <= 0) Dead();
    }

    static void OnDamage(Changed<PlayerInputs> changed)
    {
        var updateDamage = changed.Behaviour._isDamage = true;
        changed.LoadOld(); //Carga el valor anterior de la variable

        var oldDamage = changed.Behaviour._isDamage;

        if (!oldDamage && updateDamage) changed.Behaviour._damageParticles.Play();
    }

    static void LifeChanged(Changed<PlayerInputs> changed)
    {
        changed.Behaviour.OnLifeUpdate(changed.Behaviour._life / changed.Behaviour._maxLife);
    }

    void Dead()
    {
        Runner.Shutdown();
    }

    public float LocalLife()
    {
         return _life;
    }

    public float AuraFill()
    {
        float fill = 0f;

        if (!aura) return fill = 1;

        return fill;
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        PlayerDead();
    }
}
