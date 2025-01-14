using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueEspada : MonoBehaviour
{
    public float radioAtaque = 2f;
    public LayerMask capaEnemigos;
    public int vidaReducida = 20;
    public Transform puntoAtaque;
    public float tiempoEntreAtaques = 1;

    private float tiempoUltimoAtaque;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= tiempoUltimoAtaque + tiempoEntreAtaques)
        {
            Atacar();
            tiempoUltimoAtaque = Time.time;
        }
    }

    void Atacar()
    {
        Debug.Log("Enrique ataca.");
        Collider2D[] enemigosEnRadio = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, capaEnemigos);
        foreach (Collider2D enemigo in enemigosEnRadio)
        {
            VidaEnemigo vidaEnemigo = enemigo.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null) vidaEnemigo.RecibirDano(vidaReducida);
        }
    }
}
