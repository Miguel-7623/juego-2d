using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carros : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 20f;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.position += Vector3.right * speed * Time.deltaTime; // Eje global
        float distance = Vector3.Distance(startPosition, transform.position);

        // Si la distancia supera el límite, reiniciar posición
        if (distance >= maxDistance)
        {
            ResetCar();
        }

    }
    void ResetCar()
    {
        // Volver a la posición inicial
        transform.position = startPosition;
    }
}
