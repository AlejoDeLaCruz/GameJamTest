using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

public class ScoreCountingScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Referencia al TextMeshPro del canvas
    [SerializeField] private float incrementoPorSegundo = 10f; // Cantidad que aumenta por segundo
    private float puntajeActual = 0f; // Puntaje actual

    void Start()
    {
        // Verificar si la referencia a scoreText está asignada
        if (scoreText == null)
        {
            Debug.LogError("El TextMeshPro no está asignado en el inspector.");
            return;
        }

        // Si estamos en la escena "GameOverScene", mostrar el puntaje máximo y no el contador
        if (SceneManager.GetActiveScene().name == "GameOverScene")
        {
            float puntajeMaximo = GameManager.Instance.ObtenerPuntajeMaximo();
            scoreText.text = Mathf.Floor(puntajeMaximo).ToString("0"); // Mostrar el puntaje máximo sin contador
        }
        else
        {
            scoreText.text = puntajeActual.ToString("0"); // Inicializar el texto con 0 en otras escenas
        }
    }

    void Update()
    {
        // Si estamos en la escena "GameOverScene", no hacer nada
        if (SceneManager.GetActiveScene().name != "GameOverScene")
        {
            // Aumentar el puntaje rápidamente
            puntajeActual += incrementoPorSegundo * Time.deltaTime; // Aumenta el puntaje con el tiempo
            scoreText.text = Mathf.Floor(puntajeActual).ToString("0"); // Actualiza el texto con el puntaje actual
        }
    }

    // Método para guardar el puntaje máximo antes de cambiar de escena
    public void GuardarPuntajeMaximo()
    {
        Debug.Log("Guardando puntaje máximo: " + puntajeActual);
        GameManager.Instance.ActualizarPuntajeMaximo(puntajeActual); // Guarda el puntaje máximo
    }
}