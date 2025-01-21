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
        // Verificar si la referencia a scoreText est� asignada
        if (scoreText == null)
        {
            Debug.LogError("El TextMeshPro no est� asignado en el inspector.");
            return;
        }

        // Si estamos en la escena "GameOverScene", mostrar el puntaje m�ximo y no el contador
        if (SceneManager.GetActiveScene().name == "GameOverScene")
        {
            float puntajeMaximo = GameManager.Instance.ObtenerPuntajeMaximo();
            scoreText.text = Mathf.Floor(puntajeMaximo).ToString("0"); // Mostrar el puntaje m�ximo sin contador
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
            // Aumentar el puntaje r�pidamente
            puntajeActual += incrementoPorSegundo * Time.deltaTime; // Aumenta el puntaje con el tiempo
            scoreText.text = Mathf.Floor(puntajeActual).ToString("0"); // Actualiza el texto con el puntaje actual
        }
    }

    // M�todo para guardar el puntaje m�ximo antes de cambiar de escena
    public void GuardarPuntajeMaximo()
    {
        Debug.Log("Guardando puntaje m�ximo: " + puntajeActual);
        GameManager.Instance.ActualizarPuntajeMaximo(puntajeActual); // Guarda el puntaje m�ximo
    }
}