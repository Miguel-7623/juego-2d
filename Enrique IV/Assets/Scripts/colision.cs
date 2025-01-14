using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class colision : MonoBehaviour
{
    public PlayerMovement Movimiento;
    private Rigidbody2D rbjugador;
    public float velocidad = 2f; // Velocidad del movimiento
    public float amplitud = 3f; // Distancia máxima desde el punto inicial
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Jugador"); // Asumiendo que el jugador tiene el tag "Jugador
        rbjugador = jugador.GetComponent<Rigidbody2D>(); // Obtener el Rigidbody2D del jugador

    }

    // Update is called once per frame
    void Update()
    {
        float nuevaPosicionY = Mathf.PingPong(Time.time * velocidad, amplitud);

        // Actualizamos la posición del objeto
        transform.position = new Vector3(transform.position.x, nuevaPosicionY, transform.position.z);
        // Verifica si el poder está activo y han pasado 5 segundos
        
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            Debug.Log("Chocó con crocs");
            gameObject.SetActive(false);

            // Cambiar la gravedad inicial al inicio del poder
            rbjugador.gravityScale = 0.1f;
            Debug.Log("Gravedad inicial: " + rbjugador.gravityScale);
            Invoke("CambiarGravedad", 10f);
        
            //Destroy(gameObject); // Destruir el objeto después de la colisión
        }
    }
    void CambiarGravedad()  {

       
       
        rbjugador.gravityScale = 2f;
        Debug.Log("Gravedad final: " + rbjugador.gravityScale);
    }




}
