using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Asigna el Panel de pausa desde el inspector
    public GameObject canvasBarraVida;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        if (canvasBarraVida != null) canvasBarraVida.SetActive(false);
        Time.timeScale = 0f; // Detiene el tiempo en el juego
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        if (canvasBarraVida != null) canvasBarraVida.SetActive(true);
        Time.timeScale = 1f; // Restaura el tiempo
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Asegúrate de restaurar el tiempo antes de cargar el menú
        SceneManager.LoadScene("MainMenu");
    }
}
