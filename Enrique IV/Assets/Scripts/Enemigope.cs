using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform target; // El objetivo (por ejemplo, el jugador)
    public float speed = 2f; // Velocidad de movimiento
    public float attackRange = 1f; // Distancia para atacar
    public float attackCooldown = 1f; // Tiempo entre ataques

    private float attackTimer; // Temporizador para ataques

    void Update()
    {
        // Calcular la distancia al objetivo
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Si el objetivo está fuera del rango de ataque, perseguir
        if (distanceToTarget > attackRange)
        {
            MoveTowardsTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    void MoveTowardsTarget()
    {
        // Mover al enemigo hacia el objetivo
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void AttackTarget()
    {
        if (attackTimer <= 0f)
        {
            // Aquí iría la lógica de ataque (restar vida, animaciones, etc.)
            Debug.Log("Atacando al objetivo!");
            attackTimer = attackCooldown; // Reiniciar el temporizador
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }
}
