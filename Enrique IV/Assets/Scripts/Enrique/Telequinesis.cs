using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telequinesis : MonoBehaviour
{
    public float radioDeteccion = 2f;
    public Transform puntoLevantar;
    public LayerMask capaObjetos;

    // esta variable es null si el objeto esta en el suelo, evita tener que tener un booleano para comprobar cosas.
    // en el editor este objeto tiene un tag especial, por si se llega o utilizar.
    private Rigidbody2D objetoLevantado;
    private Collider2D colisionJugador;
    private Collider2D colisionObjeto;

    // Start is called before the first frame update
    void Start()
    {
        colisionJugador = GetComponent<Collider2D>();
    }
        
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objetoLevantado == null) LevantarObjeto();
            else SoltarObjeto();
        }

        if (objetoLevantado != null)
        { 
            objetoLevantado.position = Vector2.Lerp(objetoLevantado.position, puntoLevantar.position, 5f*Time.deltaTime);
        }
    }

    void LevantarObjeto()
    {
        Collider2D objetoCercano = Physics2D.OverlapCircle(transform.position, radioDeteccion, capaObjetos);
        if (objetoCercano != null)
        {
            colisionObjeto = objetoCercano;
            objetoLevantado = objetoCercano.GetComponent<Rigidbody2D>();

            if (objetoLevantado != null)
            {
                objetoLevantado.gravityScale = 0;
                objetoLevantado.bodyType = RigidbodyType2D.Kinematic;

                Physics2D.IgnoreCollision(colisionJugador, colisionObjeto, true);
                //objetoLevantado.transform.SetParent(puntoLevantar);
            }
        }
    }

    // TODO: cuando se suelta debe lanzarse o algo asi para que haga daño a los enemigos.
    void SoltarObjeto()
    {
        if (objetoLevantado != null)
        {
            objetoLevantado.gravityScale = 1;
            objetoLevantado.bodyType = RigidbodyType2D.Dynamic;

            Physics2D.IgnoreCollision(colisionJugador, colisionObjeto, false);
            //objetoLevantado.transform.SetParent(null);
            objetoLevantado = null;
            colisionObjeto = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}
