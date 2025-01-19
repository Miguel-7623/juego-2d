using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPollo : MonoBehaviour
{
    public float moveSpeed = 5f;      // Velocidad de movimiento
    public float rotationSpeed = 360f; // Velocidad de rotaci�n
    public float jumpForce = 5f;      // Fuerza del salto
    public int maxJumpSteps = 20;     // N�mero m�ximo de movimientos del salto
    public int lives = 3;             // N�mero de vidas
    public Vector3 respawnPosition;   // Posici�n de reaparici�n
    public float fallThreshold = -10f; // Umbral para detectar ca�da

    private Rigidbody rb;            // Referencia al Rigidbody
    private Animator animator;       // Referencia al Animator
    private bool isJumping = false;  // Indica si est� en medio de un salto
    private int jumpStepCount = 0;   // Contador para controlar la duraci�n del salto
    private bool isGrounded = true;  // Indica si el objeto est� en el suelo

    void Start()
    {
        // Obtener el componente Rigidbody
        rb = GetComponent<Rigidbody>();

        // Obtener el componente Animator
        animator = GetComponent<Animator>();

        // Si no se asign� una posici�n de reaparici�n, usar la posici�n inicial del objeto
        if (respawnPosition == Vector3.zero)
        {
            respawnPosition = transform.position;
        }
    }

    void FixedUpdate()
    {
        // Movimiento horizontal
        Move();

        // Controlar el salto
        if (isJumping)
        {
            PerformJump();
        }

        // Verificar si el modelo se cay� del suelo
        CheckFall();
    }

    void Move()
    {
        // Obtener entradas de movimiento
        float horizontal = Input.GetAxis("Horizontal"); // Flechas izquierda/derecha o A/D
        float vertical = Input.GetAxis("Vertical");     // Flechas arriba/abajo o W/S

        // Calcular la direcci�n del movimiento
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Cambiar animaci�n a "Caminar"
            animator.SetBool("IsWalking", true);

            // Rotar hacia la direcci�n del movimiento
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Ajustar la rotaci�n de acuerdo con la entrada
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Cambiar la direcci�n del movimiento para que el pollo se mueva en la direcci�n correcta
            Vector3 moveDirection = transform.forward * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection);
        }
        else
        {
            // Cambiar animaci�n a "Idle" si no hay movimiento
            animator.SetBool("IsWalking", false);
        }
    }

    void PerformJump()
    {
        if (jumpStepCount < maxJumpSteps)
        {
            // Aplicar una fuerza hacia arriba (simula un salto continuo por unos pasos)
            rb.MovePosition(rb.position + new Vector3(0, jumpForce * Time.fixedDeltaTime, 0));
            jumpStepCount++;
        }
        else
        {
            // Finalizar el salto
            isJumping = false;
            jumpStepCount = 0;
        }
    }

    void Update()
    {
        // Detectar el salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            isJumping = true;
            isGrounded = false;

            // Cambiar animaci�n a "Salto"
            animator.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si est� tocando el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // Si cae al suelo, volver a "Idle" o "Caminar" seg�n corresponda
            if (!isJumping)
            {
                animator.SetBool("IsWalking", false);
            }
        }
    }

    void CheckFall()
    {
        // Verificar si el modelo ha ca�do por debajo del umbral
        if (transform.position.y < fallThreshold)
        {
            lives--;

            if (lives > 0)
            {
                // Regresar al punto de reaparici�n
                transform.position = respawnPosition;
                rb.velocity = Vector3.zero; // Detener cualquier movimiento residual
                Debug.Log($"Te ca�ste. Vidas restantes: {lives}");
            }
            else
            {
                // Si se acaban las vidas, pausar el juego e imprimir mensaje
                Time.timeScale = 0; // Pausar el juego
                Debug.Log("Muri�. Se acabaron las vidas.");
            }
        }
    }
}
