using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;
    private EnemyHealth _enemyhealth;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();  
        _enemy = GetComponent<Enemy>();
        _enemyhealth = GetComponent<EnemyHealth>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HurtAnimation()
    {
        _animator.SetTrigger("Hurt");
    }

    private void DieAnimation()
    {
        _animator.SetTrigger("Die");
    }

    private float GetCurrentAnimationLength()
    {
        float _animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        return _animationLength;
    }

    private IEnumerator PlayHurt()
    {
        _enemy.StopMovement();
        HurtAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLength() + 0.3f);
        _enemy.ResumeMovement();
    }

    private void EnemyHit(Enemy enemy)
    {
        if(enemy = _enemy)
        {
            StartCoroutine(PlayHurt());
        }
    }

    private IEnumerator PlayDie()
    {
        _enemy.StopMovement();
        DieAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLength() + 0.3f);
        _enemy.ResumeMovement();
        _enemyhealth.ResetHealth();
        ObjectPooler.ReturnToPool(_enemy.gameObject);
    }

    private void EnemyDeath(Enemy enemy)
    {
        if (enemy = _enemy)
        {
            StartCoroutine(PlayDie());
        }
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyHit += EnemyHit;
        EnemyHealth.OnEnemyKilled += EnemyDeath;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyHit -= EnemyHit;
        EnemyHealth.OnEnemyKilled -= EnemyDeath;
    }
}
