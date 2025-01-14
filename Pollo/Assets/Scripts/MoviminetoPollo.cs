using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviminetoPollo : MonoBehaviour
{
    public float moveSpeed = 5f;      // Velocidad de movimiento
    public float rotationSpeed = 360f; // Velocidad de rotaci�n

    private Rigidbody rb;            // Referencia al Rigidbody

    void Start()
    {
        // Obtener el componente Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Obtener entradas de movimiento
        float horizontal = Input.GetAxis("Horizontal"); // Flechas izquierda/derecha o A/D
        float vertical = Input.GetAxis("Vertical");     // Flechas arriba/abajo o W/S

        // Calcular la direcci�n del movimiento
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Rotar hacia la direcci�n del movimiento
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Ajustar la rotaci�n de acuerdo con la entrada
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Cambiar la direcci�n del movimiento para que el pollo se mueva en la direcci�n correcta
            Vector3 moveDirection = transform.forward * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection);
        }
    }
}
