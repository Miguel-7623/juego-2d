using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoFlotanteIA : MonoBehaviour
{
    public float radioBusqueda;
    public LayerMask capajudador;
    public Transform transformJugador;
    public float velocidadMovimento;
    public float distanciaMax;
    public Vector3 puntoInicial;
    public bool mirandoderecha;
    public EstadosMovimiento estadoActual;
   
    public Animator animator;
    [SerializeField] private float dano;
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
    public void TomarDan(float dan)
    {


        animator.SetTrigger("Muerte");
        Destroy(gameObject);


    }
    //Por si le quuieren agregar salud al enemigo
    private void Muerte()
    {
        Destroy(gameObject);
    }

    private void EstadoSiguiedo()
    {
        animator.SetBool("Corriendo", true);
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimento * Time.deltaTime);
        GiraObjetivo(transformJugador.position);
        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMax 
            || Vector2.Distance(transform.position, transformJugador.position) > distanciaMax)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoVolviendo()
    {

        transform.position = Vector2.MoveTowards(transform.position, puntoInicial, velocidadMovimento * Time.deltaTime);
        GiraObjetivo(puntoInicial);
        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
           
            animator.SetBool("Corriendo", false);
            estadoActual = EstadosMovimiento.Esperando;
        }
    }
      public void OnCollisionEnter2D(Collision2D collision)
      {
          // Verifica si el objeto con el que colisiona tiene la etiqueta "Player"
          if (collision.collider.CompareTag("Jugador"))
          {
              Debug.Log("Jugador detectado en colisión");

              // Intenta obtener el componente PlayerMovement
              PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
              if (player != null)
              {
                  // Aplica daño al jugador
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
