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
    private bool facingRight = true; // Dirección inicial del enemigo
    private Animator animator; // Componente Animator para controlar las animaciones

    void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el componente Animator
    }

    void Update()
    {
        // Calcular la distancia al objetivo solo en el eje X
        float distanceToTarget = Mathf.Abs(target.position.x - transform.position.x);

        // Si el objetivo está fuera del rango de ataque, perseguir
        if (distanceToTarget > attackRange)
        {
            MoveTowardsTarget();
            animator.Play("Caminar"); // Animación de caminar
        }
        else
        {
            AttackTarget();
            animator.Play("Ataque"); // Animación de ataque
        }

        // Voltear al enemigo según la posición del objetivo
        FlipTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        // Mover al enemigo solo en el eje X hacia el objetivo
        float direction = target.position.x - transform.position.x > 0 ? 1f : -1f;
        transform.position = new Vector2(
            transform.position.x + direction * speed * Time.deltaTime,
            transform.position.y
        );
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

    void FlipTowardsTarget()
    {
        // Determinar si el enemigo debe voltearse
        if ((target.position.x > transform.position.x && !facingRight) ||
            (target.position.x < transform.position.x && facingRight))
        {
            facingRight = !facingRight; // Cambiar la dirección
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; // Invertir el eje X
            transform.localScale = localScale;
        }
    }
}
