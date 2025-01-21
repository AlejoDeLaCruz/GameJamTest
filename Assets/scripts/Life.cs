using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] private bool modoPeligro = false; // Variable para activar el modo peligro
    [SerializeField] private float velocidadReduccion = 0.7f; // Velocidad con la que se reducirá la escala en Y
    [SerializeField] private SpriteRenderer spriteRenderer; // SpriteRenderer para modificar la escala
    [SerializeField] private float vidaMaxima = 100f; // Vida máxima del jugador
    [SerializeField] private float vidaActual = 100f; // Vida actual del jugador
    [SerializeField] private Movement playerMovement; // Referencia al script de movimiento
    [SerializeField] private Timer countdownTimer; // Referencia al script Timer

    public delegate void GameOverAction(); // Definimos un delegado para el evento de Game Over
    public event GameOverAction OnGameOver; // Evento que se activará cuando la escala Y sea 0

    private Vector3 escalaOriginal;

    void Start()
    {
        // Si el SpriteRenderer no está asignado, lo asignamos automáticamente
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("No se encontró el componente SpriteRenderer en este GameObject.");
            }
        }

        // Guardamos la escala original del sprite
        if (spriteRenderer != null)
        {
            escalaOriginal = spriteRenderer.transform.localScale;
        }

        // Verificamos si el countdownTimer está asignado correctamente
        if (countdownTimer == null)
        {
            Debug.LogError("El Timer no está asignado.");
        }
    }

    void Update()
    {
        if (spriteRenderer == null || countdownTimer == null) return; // Evitar errores si no se encuentra el SpriteRenderer o Timer

        // Verificar si el temporizador ha llegado a 0 y activar el modo peligro
        if (countdownTimer != null && countdownTimer.currentTime == 0f)
        {
            CambiarModoPeligro(true); // Activar el modo peligro cuando el temporizador llegue a 0
        }
        else
        {
            CambiarModoPeligro(false); // Desactivar el modo peligro cuando el temporizador no está en 0
        }

        if (modoPeligro)
        {
            // Reducir la escala en el eje Y (permitiendo que se desaparezca completamente)
            if (spriteRenderer.transform.localScale.y > 0f) // No queremos que la escala sea negativa
            {
                spriteRenderer.transform.localScale -= new Vector3(0, velocidadReduccion * Time.deltaTime, 0);
                //Debug.Log("Reduciendo escala: " + spriteRenderer.transform.localScale);
            }
            else
            {
                // Cuando la escala Y llegue a 0 (lo que implica que el jugador ha perdido)
                OnGameOver?.Invoke(); // Invocar el evento de Game Over
            }
        }

        // No restaurar la escala cuando el modo peligro está desactivado
        // Así el jugador no volverá a la escala original cuando el modoPeligro sea false.

        // Ajustar la velocidad del jugador en función de la vida restante
        if (playerMovement != null)
        {
            // A medida que la vida disminuye, aumenta la velocidad
            float porcentajeVida = vidaActual / vidaMaxima;
            float velocidadExtra = (1f - porcentajeVida) * 5f; // Cuanto menos vida, más velocidad (hasta un máximo de 5)
            playerMovement.AdjustSpeed(velocidadExtra);
        }
    }

    // Función para activar o desactivar el modo peligro
    public void CambiarModoPeligro(bool activar)
    {
        modoPeligro = activar;
        Debug.Log("Modo peligro cambiado: " + modoPeligro);
    }

    // Método para reducir la vida, pero el movimiento sigue activo
    public void ReducirVida(float amount)
    {
        vidaActual -= amount; // Reducimos la vida
        if (vidaActual < 0f) vidaActual = 0f; // Aseguramos que la vida no sea menor que 0
        CambiarModoPeligro(true); // Activamos el modo peligro al reducir vida
    }
}