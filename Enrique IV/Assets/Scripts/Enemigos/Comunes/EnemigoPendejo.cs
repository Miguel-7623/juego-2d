using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoPendejo : MonoBehaviour
{
    public float radioBusqueda;
    public LayerMask capaJugador;
    public Transform transformJugador;
    public float velocidadMovimiento;
    public Animator animator;
    public float distanciaDetenerse = 10f; // Distancia mínima para detenerse

    private Rigidbody2D rb2D;
    private bool mirandoDerecha = true;

    [Header("Ataque")]
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float dano;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Buscar al jugador dentro del radio
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capaJugador);
        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
            float distanciaj = Vector2.Distance(transform.position, transformJugador.position);

            if (distanciaj > distanciaDetenerse)
            {
                SeguirJugador();
            }
            else
            {
                DetenerMovimiento();
            }

            // Actualizar la distancia en el Animator
            animator.SetFloat("Distanciaj", distanciaj);
        }
        else
        {
            DetenerMovimiento();
        }
    }

    private void SeguirJugador()
    {
        animator.SetBool("Corriendo", true);

        // Mover hacia el jugador
        float direccion = transformJugador.position.x - transform.position.x;
        rb2D.velocity = new Vector2(Mathf.Sign(direccion) * velocidadMovimiento, rb2D.velocity.y);

        // Girar si es necesario
        if ((direccion > 0 && !mirandoDerecha) || (direccion < 0 && mirandoDerecha))
        {
            Girar();
        }
    }

    private void DetenerMovimiento()
    {
        rb2D.velocity = Vector2.zero;
        animator.SetBool("Corriendo", false);
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, mirandoDerecha ? 0 : 180, 0);
    }

    public void Ataque()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque, capaJugador);
        foreach (Collider2D colision in objetos)
        {
            if (colision.CompareTag("Jugador"))
            {
                PlayerMovement player = colision.GetComponent<PlayerMovement>();
                if (player != null) player.TakeDamage(dano);
            }
        }
    }

     public void golpe()
    {
        animator.SetTrigger("Muerte");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        // Dibujar el radio de búsqueda en la escena
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
    }
}
