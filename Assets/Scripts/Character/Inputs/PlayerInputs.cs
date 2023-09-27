using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerInputs : NetworkBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _shootSpeed;
    [SerializeField] float _cooldown;
    [SerializeField] float _life;
    [SerializeField] float _auraTime;
    [SerializeField] float _auraTimer;
    [SerializeField] float _auracooldown;

    [SerializeField] NetworkMecanimAnimator _myAnim;
    [SerializeField] NetworkRigidbody _myRb;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _projectileSpawnPoint;
    [SerializeField] ParticleSystem _shootParticles;
    [SerializeField] GameObject _auraObject;
    [SerializeField] Aura _aura;

    NetworkInputsData _inputs;
    public bool aura;

    [Networked(OnChanged = nameof(OnFiringChanged))] bool _isFiring { get; set; }

    [Networked(OnChanged = nameof(AuraChanged))] bool _isAura { get; set; }

    bool _isWalking;
    float _lastFiringTime;

    private void Start()
    {
        _isWalking = false;
    }
    public override void FixedUpdateNetwork()
    {
        if(GetInput(out _inputs))
        {
            if (_inputs.isFiring) Shoot();
            if (_inputs.isReloading) Reload();
            if (_inputs.isJumping) Jump();
            if (_inputs.auraOn) AuraShield();
        }

        if (aura)
        {
            _auraTimer += Time.deltaTime;

            if (_auraTimer >= _auraTime)
            {
               _aura.AuraOff();
                aura = false;
                AuraReload();
                _auraTimer = 0;
            }
        }

        if (_isWalking) _myAnim.Animator.SetBool("Run", true);
        else _myAnim.Animator.SetBool("Run", false);
       
        Movement(_inputs.xMovement,_inputs.yMovement);
    }

    IEnumerator FiringCooldown()
    {
        _isFiring = true;

        yield return new WaitForSeconds(_cooldown);

        _isFiring = false;
    }

    IEnumerator AuraCooldown()
    {
        _isAura = true;

        yield return new WaitForSeconds(_auracooldown);

        _isAura = false;
    }

    void AuraShield()
    {
        aura = true;
        _auraObject.SetActive(true);
    }

    void AuraReload()
    {
        StartCoroutine(AuraCooldown());
    }

    static void AuraChanged(Changed<PlayerInputs> changed)
    {
        var updateAura = changed.Behaviour._isAura = true;
        changed.LoadOld();

        var oldAura = changed.Behaviour._isAura;
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
    }

    void Shoot()
    {
        if (Time.time - _lastFiringTime < _cooldown) return;

        _lastFiringTime = Time.time;

        _myAnim.Animator.SetBool("Shoot", true);

        Runner.Spawn(_bulletPrefab, _projectileSpawnPoint.position, transform.rotation);

        StartCoroutine(FiringCooldown());
    }

    static void OnFiringChanged(Changed<PlayerInputs> changed)
    {
        var updateFiring = changed.Behaviour._isFiring = true;
        changed.LoadOld(); //Carga el valor anterior de la variable

        var oldFiring = changed.Behaviour._isFiring;

       //if (!oldFiring && oldFiring) changed.Behaviour._shootParticles.Play();
    }

    public void TakeDamage(float dmg)
    {
        RPC_TakeDamage(dmg);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_TakeDamage(float dmg)
    {
        if (aura) return;

        _life -= dmg;

        if (_life <= 0) Dead();
    }

    void Dead()
    {
        Runner.Shutdown();
    }
}
