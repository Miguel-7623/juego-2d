using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;         // La gallina que la c�mara seguir�
    public Vector3 offset = new Vector3(0, 15, -50); // Posici�n relativa a la gallina
    public float followSpeed = 5f;  // Velocidad de seguimiento de la c�mara
    public float rotationSpeed = 100f; // Velocidad de rotaci�n con el mouse

    private float currentRotation = 0f; // Para almacenar la rotaci�n actual de la c�mara

    void LateUpdate()
    {
        // Obtener la rotaci�n del mouse
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        // Rotar la c�mara alrededor del objetivo con el mouse (solo rotaci�n en el eje Y)
        currentRotation += horizontalInput * rotationSpeed * Time.deltaTime;

        // Calcular la nueva posici�n de la c�mara, manteniendo la distancia detr�s del objetivo
        Quaternion camRotation = Quaternion.Euler(0, currentRotation, 0); // Solo rotar en el eje Y
        offset = camRotation * new Vector3(0, 15, -30); // Mantener la posici�n relativa detr�s del objetivo

        // Ajustar la posici�n de la c�mara suavemente hacia la posici�n deseada
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Mirar al objetivo
        transform.LookAt(target.position);
    }
}
