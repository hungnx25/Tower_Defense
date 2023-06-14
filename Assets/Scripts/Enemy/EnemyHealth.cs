using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit;


    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform barPosition;

    [SerializeField] private float initHealth = 100f;
    [SerializeField] private float maxHealth = 100f;

    public float CurrentHealth { get; set; }

    private Image _healthBar;
    private Enemy _enemy;

    // Start is called before the first frame update
    void Start()
    {
        CreateHealthBar();
        CurrentHealth = initHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.P))
        {
            Damage(50f);
        }

        _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount,
            CurrentHealth / maxHealth, Time.deltaTime * 10f);
    }

    //Tao healthBar cho enemy
    private void CreateHealthBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity); 
        newBar.transform.SetParent(transform);

        EnemyHealthController controller = newBar.GetComponent<EnemyHealthController>();
        _healthBar = controller.FillAmountImage;
    }

    //tinh damage 
    public void Damage(float damageReceived)
    {
        CurrentHealth -= damageReceived;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        else
        {
            OnEnemyHit?.Invoke(_enemy);
        }

    }

    public void ResetHealth()
    {
        CurrentHealth = initHealth;
        _healthBar.fillAmount = 1f;
    }

    //khi enemy die tra ve pool
    private void Die()
    {
        OnEnemyKilled?.Invoke(_enemy);
    }

                                                                                                                    
}
