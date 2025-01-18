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
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= tiempoUltimoAtaque + tiempoEntreAtaques)
        {
            StartCoroutine(Atacar());
            tiempoUltimoAtaque = Time.time;
        }
    }

    private IEnumerator Atacar()
    {
        animator.SetBool("atacandoConEspada", true);
        Debug.Log("ataque con espada");
        Collider2D[] enemigosEnRadio = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, capaEnemigos);
        foreach (Collider2D enemigo in enemigosEnRadio)
        {
            VidaEnemigo vidaEnemigo = enemigo.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null) vidaEnemigo.RecibirDano(vidaReducida);
        }
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("atacandoConEspada", false);
    }
}
