using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private float puntajeMaximo;

    void Awake()
    {
        // Aseguramos que solo haya una instancia del GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruirlo al cargar nuevas escenas
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destruir cualquier otra instancia
        }
    }

    // Método para actualizar el puntaje máximo
    public void ActualizarPuntajeMaximo(float puntaje)
    {
        Debug.Log("Actualizando puntaje máximo: " + puntaje);  // Verifica el puntaje que llega
        if (puntaje > puntajeMaximo)
        {
            puntajeMaximo = puntaje;
            Debug.Log("Nuevo puntaje máximo: " + puntajeMaximo);  // Verifica el nuevo puntaje máximo
        }
    }

    // Método para obtener el puntaje máximo
    public float ObtenerPuntajeMaximo()
    {
        Debug.Log("Obteniendo puntaje máximo: " + puntajeMaximo);  // Verifica que el puntaje esté siendo obtenido correctamente
        return puntajeMaximo;
    }
}
