using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoFlotanteIA : MonoBehaviour
{
    public float radioBusqueda;
    public LayerMask capajudador;
    public Transform transformJugador;
    public float velocidadMovimiento;
    public float distanciaPatrulla;
    public float distanciaMax;

    private Vector3 puntoInicial;
    private Vector3 puntoFinal;
    private bool patrullandoDerecha = true;

    public EstadosMovimiento estadoActual;

    public Animator animator;
    [SerializeField] private float dano;

    public enum EstadosMovimiento
    {
        Patrullando,
        Siguiendo,
        Volviendo
    }

    private void Start()
    {
        puntoInicial = transform.position;
        puntoFinal = puntoInicial + new Vector3(distanciaPatrulla, 0, 0);
        estadoActual = EstadosMovimiento.Patrullando;
    }

    private void Update()
    {
        switch (estadoActual)
        {
            case EstadosMovimiento.Patrullando:
                EstadoPatrullando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }
    }

    private void EstadoPatrullando()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capajudador);
        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
            estadoActual = EstadosMovimiento.Siguiendo;
            return;
        }

        // Movimiento entre los puntos de patrulla
        if (patrullandoDerecha)
        {
            transform.position = Vector2.MoveTowards(transform.position, puntoFinal, velocidadMovimiento * Time.deltaTime);
            GiraObjetivo(puntoFinal);

            if (Vector2.Distance(transform.position, puntoFinal) < 0.1f)
            {
                patrullandoDerecha = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, puntoInicial, velocidadMovimiento * Time.deltaTime);
            GiraObjetivo(puntoInicial);

            if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
            {
                patrullandoDerecha = true;
            }
        }
    }

    private void EstadoSiguiendo()
    {
        animator.SetBool("Corriendo", true);

        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);
        GiraObjetivo(transformJugador.position);

        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMax ||
            Vector2.Distance(transform.position, transformJugador.position) > distanciaMax)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoVolviendo()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntoInicial, velocidadMovimiento * Time.deltaTime);
        GiraObjetivo(puntoInicial);

        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
            animator.SetBool("Corriendo", false);
            estadoActual = EstadosMovimiento.Patrullando;
        }
    }

    private void GiraObjetivo(Vector3 objetivo)
    {
        if (objetivo.x > transform.position.x && !patrullandoDerecha)
        {
            Girar();
        }
        else if (objetivo.x < transform.position.x && patrullandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        patrullandoDerecha = !patrullandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Jugador"))
        {
            Debug.Log("Jugador detectado en colisión");

            PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(dano);
            }
            else
            {
                Debug.LogWarning("El objeto con etiqueta 'Jugador' no tiene un componente PlayerMovement.");
            }
        }
    }
    public void golpe()
    {
        animator.SetTrigger("Muerte");
        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(puntoInicial, puntoInicial + new Vector3(distanciaPatrulla, 0, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
    }
}
