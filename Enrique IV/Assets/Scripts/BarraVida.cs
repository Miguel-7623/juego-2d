using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();

    }
    public void CambiarVidaMax(float vidaMax)
    {
        slider.maxValue = vidaMax;
    }
    public void CambiarVidaAct(float Cantidadvida)
    {
        slider.value = Cantidadvida;
    }

    public void IniciaBarra(float Cantidadvida)
    {
        CambiarVidaMax(Cantidadvida);
        CambiarVidaAct(Cantidadvida);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
