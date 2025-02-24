using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoEnrique : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    public LayerMask capaSuelo;
    public Transform puntoSuelo;
    public float radioSuelo = 0.25f;

    private Rigidbody2D rb;
    private bool estaEnSuelo;
    private Animator animator; 
    
    public int direccion = 1;


    // ATAQUES CON LAS PATAS.
    public float radioAtaque = 1f;
    public LayerMask capaEnemigos;
    public int vidaReducida = 20;
    public Transform puntoAtaque;
    public float tiempoEntreAtaques = 1;
   

    private float tiempoUltimoAtaque;
    //public Transform espada;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        animator = GetComponentInChildren<Animator>();

        //espada = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movimiento * velocidad, rb.velocity.y);

        animator.SetBool("estaCorriendo", movimiento != 0);

        estaEnSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);
        if (estaEnSuelo && Input.GetButtonDown("Jump")) 
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }

        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= tiempoUltimoAtaque + tiempoEntreAtaques)
        {
            StartCoroutine(AtaqueBasico());
            tiempoUltimoAtaque = Time.time;
        }


        if (movimiento > 0 && direccion == -1)
        {
            Girar();
        }
        else if (movimiento < 0 && direccion == 1)
        {
            Girar();
        }
        
        //if (espada != null) espada.position = transform.position + (direccion*new Vector3(1f, 0f, 0f));
    }
    private void Girar()
    {
        direccion *= -1; // Cambia la direcci�n (1 o -1)
        Vector3 escala = transform.localScale; // Obtiene la escala actual
        escala.x *= -1; // Invierte el eje X
        transform.localScale = escala; // Asigna la nueva escala
    }
    private IEnumerator AtaqueBasico() { 
    
       
        animator.SetBool("estaAtacando", true);
        Debug.Log("ataque basico (con las patas).");
        Collider2D[] enemigosEnRadio = Physics2D.OverlapCircleAll(puntoAtaque.position, capaEnemigos);
        foreach (Collider2D enemigo in enemigosEnRadio)
        { 
            VidaEnemigo vidaEnemigo = enemigo.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null) vidaEnemigo.RecibirDano(vidaReducida);
        }
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("estaAtacando", false);
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
        }
    }
}
