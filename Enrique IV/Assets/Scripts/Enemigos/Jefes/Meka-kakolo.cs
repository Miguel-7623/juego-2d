using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeAI : MonoBehaviour
{
    public LayerMask capaJugador;
    public Vector3 puntoInicial;
    public bool mirandoDerecha;
    public Rigidbody2D rb2D;
    public Animator animator;
    public Transform Player;

    [Header("Vida")]
    [SerializeField] private float vida;
    //[SerializeField] private BarraVida barraVida;

    [Header("Ataque")]
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float dano;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        //barraVida.IniciaBarra(vida);
        Player = GameObject.FindGameObjectWithTag("Jugador").GetComponent<Transform>();
       
        Debug.Log("Jugador detectado: " + Player.gameObject.name);
    }
    /*public void OnCollisionEnter2D(Collision2D collision)
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
    }*/

    public void TomarDan(float dan)
    {
        vida -= dan;
       // barraVida.CambiarVidaAct(vida);
        if (vida <= 0)
        {
            animator.SetTrigger("Muerte");
           
        }
    }

    private void Muerte()
    {
        Destroy(gameObject);
    }

    public void MiraJugador()
    {
        if ((Player.position.x > transform.position.x && !mirandoDerecha) ||
            (Player.position.x < transform.position.x && mirandoDerecha))
        {
            Girar();
        }
    }

    public void Ataque()
    {
       // Debug.Log("Ejecutando ataque...");
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque, capaJugador);
        foreach (Collider2D colision in objetos)
        {
            Debug.Log(colision);
            if (colision.CompareTag("Jugador"))
            {
              
               colision.GetComponent<PlayerMovement>().TakeDamage(dano);
             
                
            }
        }
    }
    public void nose()
    {
        Debug.Log("cdcdcdc");
    }
    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void Update()
    {
       
        float distanciaj = Vector2.Distance(transform.position, Player.position);
        animator.SetFloat("Distanciaj", distanciaj);
        //Debug.Log(distanciaj);
       
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
    }
}
