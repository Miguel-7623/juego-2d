using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento del jugador
    [SerializeField] private BarraVida barraVida; // Referencia a la barra de vida
    [SerializeField] private float maxHealth ; // Salud m�xima del jugador
    [SerializeField] private float currentHealth;  // Salud actual del jugador
    [SerializeField] private float invulnerableDuration = 2f; // Duraci�n de la invulnerabilidad en segundos

    private bool isInvulnerable = false; // Bandera para determinar si el jugador es invulnerable
    private Vector2 movement; // Direcci�n del movimiento

    private void Start()
    {
        // Iniciar la salud del jugador
        currentHealth = maxHealth;

        // Inicializar la barra de vida
        if (barraVida != null)
        {
            barraVida.IniciaBarra(1f); // Normalizado a 1 (100%)
        }
        else
        {
            Debug.LogWarning("BarraVida no asignada en el inspector.");
        }
    }

    void Update()
    {
        // Leer la entrada del jugador
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D o Flechas izquierda/derecha
        movement.y = Input.GetAxisRaw("Vertical");   // W/S o Flechas arriba/abajo

        // Verificar si el jugador ha muerto
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        // Mover al jugador
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable)
        {
            Debug.Log("Jugador es invulnerable, no recibe da�o.");
            return; // Salir si el jugador es invulnerable
        }

        // Reducir la salud del jugador
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; // Evitar valores negativos
        Debug.Log("Jugador recibi� da�o. Salud restante: " + currentHealth);

        // Calcular el valor normalizado
        float healthNormalized = currentHealth / maxHealth;

        // Actualizar la barra de vida con el valor normalizado
        if (barraVida != null)
        {
            barraVida.CambiarVidaAct(healthNormalized);
        }

        // Iniciar invulnerabilidad
        StartCoroutine(InvulnerabilityCoroutine());
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true; // Activar invulnerabilidad
        Debug.Log("Jugador es invulnerable.");
        yield return new WaitForSeconds(invulnerableDuration); // Esperar duraci�n de invulnerabilidad
        isInvulnerable = false; // Desactivar invulnerabilidad
        Debug.Log("Jugador ya no es invulnerable.");
    }

    void Die()
    {
        Debug.Log("Jugador ha muerto.");
        // L�gica para la muerte del jugador (puedes personalizar)
        // Destroy(gameObject); // Opcional, destruye el jugador
    }
}
