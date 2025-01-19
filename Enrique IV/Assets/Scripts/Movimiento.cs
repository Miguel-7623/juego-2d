using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Velocidad de movimiento del jugador
    [SerializeField] private BarraVida barraVida; // Referencia a la barra de vida
    [SerializeField] private float maxHealth = 100; // Salud m�xima del jugador
    [SerializeField] private float currentHealth;  // Salud actual del jugador
    [SerializeField] private float invulnerableDuration = 2f; // Duraci�n de la invulnerabilidad en segundos

    private bool isInvulnerable = false; // Bandera para determinar si el jugador es invulnerable
    private Vector2 movement; // Direcci�n del movimiento

    public int direccion = 1;
    private Animator animator;

    // ATAQUES CON LAS PATAS.
    public float radioAtaque = 3f;
    public LayerMask capaEnemigos;
    public int vidaReducida = 20;
    public Transform puntoAtaque;
    public float tiempoEntreAtaques = 1;
    public AudioClip musicClip; // Arrastra aquí tu archivo de música desde el inspector
    private AudioSource audioSource;
    public AudioClip musicClip2;


    private float tiempoUltimoAtaque;

    private void Start()
    {
        moveSpeed = 10f; // Velocidad de movimiento del jugador

        // Iniciar la salud del jugador
        currentHealth = maxHealth;

        // Inicializar la barra de vida
        if (barraVida != null)
        {
            barraVida.IniciaBarra(1f); // Normalizado a 1 (100%)
        }
        else
        {
            Debug.LogWarning("BarraVida no asignada en el inspector.");
        }

        animator = GetComponentInChildren<Animator>(); 
    }

    void Update()
    {
        // Leer la entrada del jugador
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D o Flechas izquierda/derecha
        movement.y = Input.GetAxisRaw("Vertical");   // W/S o Flechas arriba/abajo


        Debug.Log(movement.x +  " " + movement.y);
        animator.SetBool("estaCorriendo", movement.x != 0);

        // Verificar si el jugador ha muerto
        if (currentHealth <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= tiempoUltimoAtaque + tiempoEntreAtaques)
        {
            StartCoroutine(AtaqueBasico());
            tiempoUltimoAtaque = Time.time;
        }

        if (movement.x > 0 && direccion == -1)
        {
            Girar();
        }
        else if (movement.x < 0 && direccion == 1)
        {
            Girar();
        }
    }

    private void Girar()
    {
        direccion *= -1; // Cambia la direcci�n (1 o -1)
        Vector3 escala = transform.localScale; // Obtiene la escala actual
        escala.x *= -1; // Invierte el eje X
        transform.localScale = escala; // Asigna la nueva escala
    }

    private IEnumerator AtaqueBasico()
    {

        animator.SetBool("estaAtacando", true);
        Debug.Log("ataque basico (con las patas).");
        Awake();
        PlayMusic();

        Collider2D[] enemigosEnRadio = Physics2D.OverlapCircleAll(puntoAtaque.position, capaEnemigos);
        foreach (Collider2D enemigo in enemigosEnRadio)
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
            
            if (colision.CompareTag("Jefeplanta"))
            {
                Jefeplanta jefePlanta = colision.GetComponent<Jefeplanta>();
                if (jefePlanta != null) jefePlanta.TomarDano(vidaReducida);
            }
        }
        yield return new WaitForSeconds(0.5f);
        
        animator.SetBool("estaAtacando", false);
    }

    void FixedUpdate()
    {
        // Mover al jugador
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable)
        {
            Debug.Log("Jugador es invulnerable, no recibe da�o.");
            return; // Salir si el jugador es invulnerable
        }

        // Reducir la salud del jugador
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; // Evitar valores negativos
        Debug.Log("Jugador recibi� da�o. Salud restante: " + currentHealth);

        // Calcular el valor normalizado
        float healthNormalized = currentHealth / maxHealth;

        // Actualizar la barra de vida con el valor normalizado
        if (barraVida != null)
        {
            barraVida.CambiarVidaAct(healthNormalized);
        }

        // Iniciar invulnerabilidad
        StartCoroutine(InvulnerabilityCoroutine());
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true; // Activar invulnerabilidad
        Debug.Log("Jugador es invulnerable.");
        yield return new WaitForSeconds(invulnerableDuration); // Esperar duraci�n de invulnerabilidad
        isInvulnerable = false; // Desactivar invulnerabilidad
        Debug.Log("Jugador ya no es invulnerable.");
    }

    void Die()
    {
        Debug.Log("Jugador ha muerto.");
        // L�gica para la muerte del jugador (puedes personalizar)
        Destroy(gameObject); // Opcional, destruye el jugador
        SceneManager.LoadScene("PantallaMuerte");
    }

    public void Pollo(float damage)
    {


        // Reducir la salud del jugador
        currentHealth -= damage;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // Evitar valores fuera de rango
        Debug.Log("Jugador recibi� salud. Vida restante: " + currentHealth);

        // Calcular el valor normalizado
        float healthNormalized = currentHealth / maxHealth;

        // Actualizar la barra de vida con el valor normalizado
        if (barraVida != null)
        {
            barraVida.CambiarVidaAct(healthNormalized);
        }
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
