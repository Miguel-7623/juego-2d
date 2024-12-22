
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento del jugador

    private Vector2 movement; // Dirección del movimiento

    void Update()
    {
        // Leer la entrada del jugador
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D o Flechas izquierda/derecha
        movement.y = Input.GetAxisRaw("Vertical");   // W/S o Flechas arriba/abajo
    }

    void FixedUpdate()
    {
        // Mover al jugador
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
    }
}
