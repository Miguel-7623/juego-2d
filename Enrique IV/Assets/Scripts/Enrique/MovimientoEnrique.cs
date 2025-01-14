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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movimiento * velocidad, rb.velocity.y);

        estaEnSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);
        if (estaEnSuelo && Input.GetButtonDown("Jump")) 
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }
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
