using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using UnityEngine.UI; // Necesario para trabajar con UI
using TMPro; // Importar para trabajar con TextMeshPro
using System.Collections; // Necesario para trabajar con corutinas

public class GameOverManager : MonoBehaviour
{
    // Asignar los botones desde el Inspector
    public Button restartButton;
    public Button exitButton;

    [SerializeField] private Life playerLife; // Referencia al script Life del jugador
    [SerializeField] private TextMeshProUGUI scoreText; // Referencia al TextMeshPro para mostrar el puntaje máximo

    void Start()
    {
        // Verificar si los botones están asignados en el inspector antes de añadir los listeners
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogWarning("El botón de reinicio no está asignado.");
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogWarning("El botón de salida no está asignado.");
        }

        // Verificar si la referencia a playerLife está asignada
        if (playerLife == null)
        {
            Debug.LogError("No se ha asignado el script Life del jugador.");
        }
        else
        {
            // Suscribirse al evento OnGameOver del jugador
            playerLife.OnGameOver += ShowGameOverScreen;
        }

        // Verificar si la referencia al TextMeshPro está asignada
        if (scoreText == null)
        {
            Debug.LogError("El TextMeshPro para mostrar el puntaje no está asignado.");
        }
    }

    // Mostrar la pantalla de Game Over
    private void ShowGameOverScreen()
    {
        Debug.Log(scoreText);

        // Guardar puntaje máximo antes de mostrar el puntaje
        GameObject scoreCounterObject = GameObject.FindWithTag("ScoreCounter"); // Asegúrate de que el GameObject con el script ScoreCountingScript tenga un tag único
        ScoreCountingScript scoreScript = scoreCounterObject.GetComponent<ScoreCountingScript>();
        scoreScript.GuardarPuntajeMaximo();

        // Mostrar el puntaje máximo en el TextMeshPro
        if (scoreText != null)
        {
            float puntajeMaximo = GameManager.Instance.ObtenerPuntajeMaximo();
            puntajeMaximo = Mathf.Floor(puntajeMaximo);  // Redondear hacia abajo
            scoreText.text = puntajeMaximo.ToString("0"); // Mostrar sin decimales
            Debug.Log(puntajeMaximo);
        }

        // Cambiar a la escena de Game Over después de que el puntaje se muestre
        StartCoroutine(CargarEscenaConRetraso());
    }

    // Corutina para cargar la escena después de un pequeño retraso, asegurando que el texto se haya mostrado antes de cambiar de escena
    private IEnumerator CargarEscenaConRetraso()
    {
        yield return new WaitForSeconds(0.1f); // Ajusta el tiempo según lo necesario
        SceneManager.LoadScene("GameOverScene");
    }

    // Reinicia el juego cargando la escena principal
    void RestartGame()
    {
        SceneManager.LoadScene("SampleScene"); // Asegúrate de poner el nombre de tu escena principal
    }

    // Cierra el juego
    void ExitGame()
    {
        // Si estás en editor de Unity, esto solo detiene la reproducción, pero en el juego real cierra la aplicación.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Para detener en el editor
#else
            Application.Quit(); // Cierra el juego cuando no estás en el editor
#endif
    }

    // Asegúrate de cancelar la suscripción al evento cuando el objeto se destruya
    private void OnDestroy()
    {
        if (playerLife != null)
        {
            playerLife.OnGameOver -= ShowGameOverScreen;
        }
    }
}
