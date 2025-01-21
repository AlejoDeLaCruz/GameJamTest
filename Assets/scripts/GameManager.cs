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

    // M�todo para actualizar el puntaje m�ximo
    public void ActualizarPuntajeMaximo(float puntaje)
    {
        Debug.Log("Actualizando puntaje m�ximo: " + puntaje);  // Verifica el puntaje que llega
        if (puntaje > puntajeMaximo)
        {
            puntajeMaximo = puntaje;
            Debug.Log("Nuevo puntaje m�ximo: " + puntajeMaximo);  // Verifica el nuevo puntaje m�ximo
        }
    }

    // M�todo para obtener el puntaje m�ximo
    public float ObtenerPuntajeMaximo()
    {
        Debug.Log("Obteniendo puntaje m�ximo: " + puntajeMaximo);  // Verifica que el puntaje est� siendo obtenido correctamente
        return puntajeMaximo;
    }
}
