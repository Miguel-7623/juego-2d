using System.Collections;
using UnityEngine;

public class Jefeplanta : MonoBehaviour
{
    [SerializeField] private float vidaMaxima = 100f;
    private float vidaActual;
    [SerializeField] private Transform controladorduisparo;
    [SerializeField] private GameObject bala;
    [SerializeField] private Animator animator;
    private bool estaVivo = true;

    void Start()
    {
        vidaActual = vidaMaxima;
        StartCoroutine(DispararConTiempoAleatorio());
    }

    void Update()
    {
        if (!estaVivo)
        {
            return; // Detener todas las acciones si el jefe está muerto
        }
    }

    private IEnumerator DispararConTiempoAleatorio()
    {
        while (estaVivo)
        {
            float tiempoEspera = Random.Range(1f, 3f); // Genera un tiempo entre 1 y 3 segundos
            yield return new WaitForSeconds(tiempoEspera);
            // Activa la animación de daño
            animator.SetTrigger("Daño");

           
        }
    }

    private void Disparar()
    {
        Debug.Log("Disparando");
        if (!estaVivo)
        {
            return;
        }

        // Dispara el proyectil
        Instantiate(bala, controladorduisparo.position, controladorduisparo.rotation);



    }

    public void TomarDano(float dano)
    {
        if (!estaVivo)
        {
            return;
        }

        vidaActual -= dano;

        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        estaVivo = false;
        animator.SetTrigger("Muerte");
        // Destruir el objeto después de un tiempo para permitir que la animación de muerte se complete
        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmos()
    {
        // Dibuja una línea que representa el punto de disparo en el editor
        if (controladorduisparo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(controladorduisparo.position, 0.1f);
        }
    }
}
