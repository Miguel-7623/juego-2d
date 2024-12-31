using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balajefe : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float dan;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto con el que colisiona tiene la etiqueta "Player"
        if (collision.CompareTag("Jugador"))
        {
            Debug.Log("Jugador detectado en colisión (Trigger)");

            // Intenta obtener el componente PlayerMovement
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // Aplica daño al jugador
                player.TakeDamage(dan);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("El objeto con etiqueta 'Player' no tiene un componente PlayerMovement.");
            }
        }
    }
}
