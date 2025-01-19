using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class ejecutacinamatica : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayableDirector playableDirector;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador"))
        {
            Debug.Log("Cinematica");
            gameObject.SetActive(false);
            playableDirector.Play();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
