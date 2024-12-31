using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoIA : MonoBehaviour
{
    public float radioBusqueda;
    public LayerMask capajudador;
    [SerializeField] private Transform transformJugador;
    public float velocidadMovimento;
    public float distanciaMax;
    public Vector3 puntoInicial;
    public bool mirandoderecha;
    public EstadosMovimiento estadoActual;
    public Rigidbody2D rb2D;
    public Animator animator;
    [SerializeField] private bool muerto;
    [SerializeField] private float dano;
    [SerializeField] private Transform controladorduisparo;
    [SerializeField] private GameObject bala;
    private bool disparando;

    public enum EstadosMovimiento
    {
        Esperando,
        Siguiendo,
        Volviendo
    }

    private void Start()
    {
        puntoInicial = transform.position;
    }

    private void Update()
    {
        switch (estadoActual)
        {
            case EstadosMovimiento.Esperando:
                EstadoEsperando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiedo();
                break;
            case EstadosMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }

        if (muerto)
        {
            animator.SetTrigger("Muerte");
            Destroy(gameObject);
        }
    }

    private void EstadoEsperando()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capajudador);
        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
            estadoActual = EstadosMovimiento.Siguiendo;
        }
        
    }


    private void EstadoSiguiedo()
    {
        animator.SetBool("Corriendo", true);
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            return;
        }

        if (transform.position.x < transformJugador.position.x)
        {
            rb2D.velocity = new Vector2(velocidadMovimento, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(-velocidadMovimento, rb2D.velocity.y);
        }

        GiraObjetivo(transformJugador.position);

        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMax || Vector2.Distance(transform.position, transformJugador.position) > distanciaMax)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            //transformJugador = null;
            disparando = false;
            StopCoroutine(DispararCadaDosSegundos());
        }
        else if (!disparando)
        {
            disparando = true;
            StartCoroutine(DispararCadaDosSegundos());
        }
    }

    private void EstadoVolviendo()
    {
        if (transform.position.x < puntoInicial.x)
        {
            rb2D.velocity = new Vector2(velocidadMovimento, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(-velocidadMovimento, rb2D.velocity.y);
        }

        GiraObjetivo(puntoInicial);
        if (Vector2.Distance(transform.position, puntoInicial) < 0.6f)
        {
            rb2D.velocity = Vector2.zero;
            animator.SetBool("Corriendo", false);
            estadoActual = EstadosMovimiento.Esperando;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
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
                Debug.LogWarning("El objeto con etiqueta 'Player' no tiene un componente PlayerMovement.");
            }
        }
    }

    private void GiraObjetivo(Vector3 objetivo)
    {
        if (objetivo.x > transform.position.x && !mirandoderecha)
        {
            Girar();
        }
        else if (objetivo.x < transform.position.x && mirandoderecha)
        {
            Girar();
        }
    }

    private IEnumerator DispararCadaDosSegundos()
    {
        while (disparando)
        {
            Disparar();
            yield return new WaitForSeconds(2f);
        }
    }

    private void Disparar()
    {
        Instantiate(bala, controladorduisparo.position, controladorduisparo.rotation);
    }

    private void Girar()
    {
        mirandoderecha = !mirandoderecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMax);
    }
}
