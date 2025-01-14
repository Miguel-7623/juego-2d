using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;         // La gallina que la cámara seguirá
    public Vector3 offset = new Vector3(0, 15, -50); // Posición relativa a la gallina
    public float followSpeed = 5f;  // Velocidad de seguimiento de la cámara
    public float rotationSpeed = 100f; // Velocidad de rotación con el mouse

    private float currentRotation = 0f; // Para almacenar la rotación actual de la cámara

    void LateUpdate()
    {
        // Obtener la rotación del mouse
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        // Rotar la cámara alrededor del objetivo con el mouse (solo rotación en el eje Y)
        currentRotation += horizontalInput * rotationSpeed * Time.deltaTime;

        // Calcular la nueva posición de la cámara, manteniendo la distancia detrás del objetivo
        Quaternion camRotation = Quaternion.Euler(0, currentRotation, 0); // Solo rotar en el eje Y
        offset = camRotation * new Vector3(0, 15, -30); // Mantener la posición relativa detrás del objetivo

        // Ajustar la posición de la cámara suavemente hacia la posición deseada
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Mirar al objetivo
        transform.LookAt(target.position);
    }
}
