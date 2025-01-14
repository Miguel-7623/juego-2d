using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    public int vida = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecibirDano(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Morir();

        Debug.Log("Se reduce vida el enemigo.");
    }

    void Morir()
    {
        Destroy(gameObject);
        Debug.Log("El enemigo murio.");
    }
}
