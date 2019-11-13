using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    public enum AttackType { Punch, Kick }

    public AttackType SelectedAttackType;

    public PlayerCharacter playerCharacter;
    public EnemyCharacter enemyCharacter;

    [SerializeField]
    float damage;

    void Attack()
    {
        if (SelectedAttackType == AttackType.Punch)
        {
            damage = 10;
        }
        else if (SelectedAttackType == AttackType.Kick)
        {
            damage = 15;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Attack();
            enemyCharacter.TakeDamage(damage);
            gameObject.GetComponent<Collider2D>().enabled = false;

            Debug.Log("Enemy was attacked");
        }

        if (collision.gameObject.tag == "Player")
        {
            Attack();
            playerCharacter.TakeDamage(damage);
            gameObject.GetComponent<Collider2D>().enabled = false;

            Debug.Log("Player was attacked");
        }
    }

    private void Awake()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

}
