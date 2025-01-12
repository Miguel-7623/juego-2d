using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colision_pollo : MonoBehaviour
{
    public BarraVida barraVida;
    public float sanar = 10f;
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
            Debug.Log("choco con pollo");
            PlayerMovement jugador = other.GetComponent<PlayerMovement>();
            gameObject.SetActive(false);

            jugador.Pollo(-sanar); // Llama al método

           // Destroy(gameObject);

        }
    }
}
