using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colision_monster : MonoBehaviour
{
    public PlayerMovement Movimiento;
  
    public float velocidad = 2f; // Velocidad del movimiento
    public float amplitud = 3f; // Distancia máxima desde el punto inicial

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float nuevaPosicionY = Mathf.PingPong(Time.time * velocidad, amplitud);

        // Actualizamos la posición del objeto
        transform.position = new Vector3(transform.position.x, nuevaPosicionY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            Debug.Log("choco con monster");

            gameObject.SetActive(false);
            Movimiento.moveSpeed = 20f;
            Debug.Log("velocidad inicial: " + Movimiento.moveSpeed);
            //Destroy(gameObject);

            Invoke("CambiarVelocidad", 10f);

        }
    }
    void CambiarVelocidad()
    {



        Movimiento.moveSpeed = 10f;
        Debug.Log("velocidad final: " + Movimiento.moveSpeed);
    }

}
