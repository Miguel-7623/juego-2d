using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float dano;

    public GameObject jugador;
    private Transform jugadorTransf;
    public AudioClip musicClip; // Arrastra aquí tu archivo de música desde el inspector
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        jugadorTransf = jugador.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= tiempoUltimoAtaque + tiempoEntreAtaques)
        {
            StartCoroutine(Atacar());
            tiempoUltimoAtaque = Time.time;
        }


        if (jugadorTransf != null)
        {
            Vector3 direccion = jugadorTransf.localScale.x > 0 ? Vector3.right : Vector3.left;
            transform.localScale = new Vector3(direccion.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator Atacar()
    {
        animator.SetBool("atacandoConEspada", true);
        Awake();
        PlayMusic();
        Debug.Log("ataque con espada");
        Collider2D[] objetos = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, capaEnemigos);

        foreach (Collider2D colision in objetos)
        {
            if (colision.CompareTag("Jefe"))
            {
                JefeAI jefe = colision.GetComponent<JefeAI>();
                if (jefe != null) jefe.TomarDan(vidaReducida);
            }

            if (colision.CompareTag("Enemigo"))
            {
                EnemigoPendejo enemigo = colision.GetComponent<EnemigoPendejo>();
                if (enemigo != null) enemigo.golpe();
            }

            if (colision.CompareTag("JefePlanta"))
            {
                Jefeplanta jefePlanta = colision.GetComponent<Jefeplanta>();
                if (jefePlanta != null) jefePlanta.TomarDano(vidaReducida);
            }
        }

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("atacandoConEspada", false);
    }
    void Awake()
    {
        // Asegúrate de que este GameObject no se destruya al cambiar de escena
        DontDestroyOnLoad(gameObject);

        // Configura el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = false; // Hacer que la música se reproduzca en bucle
        audioSource.playOnAwake = false; // No iniciar automáticamente

        // Inicia la reproducción
        // PlayMusic();
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
