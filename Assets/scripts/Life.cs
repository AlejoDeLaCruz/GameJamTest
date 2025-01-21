using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] private bool modoPeligro = false; // Variable para activar el modo peligro
    [SerializeField] private float velocidadReduccion = 0.7f; // Velocidad con la que se reducirá la escala en Y
    [SerializeField] private SpriteRenderer spriteRenderer; // SpriteRenderer para modificar la escala

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
    }

    void Update()
    {
        if (spriteRenderer == null) return; // Evitar errores si no se encuentra el SpriteRenderer

        // Mostrar en consola si el modo peligro está activado
        Debug.Log("Modo peligro: " + modoPeligro);

        if (modoPeligro)
        {
            // Reducir la escala en el eje Y
            if (spriteRenderer.transform.localScale.y > escalaOriginal.y * 0.1f) // Limitar para evitar que se reduzca demasiado
            {
                spriteRenderer.transform.localScale -= new Vector3(0, velocidadReduccion * Time.deltaTime, 0);
                Debug.Log("Reduciendo escala: " + spriteRenderer.transform.localScale);
            }
        }
        else
        {
            // Volver a la escala original
            spriteRenderer.transform.localScale = Vector3.Lerp(spriteRenderer.transform.localScale, escalaOriginal, Time.deltaTime * velocidadReduccion);
            Debug.Log("Restaurando escala: " + spriteRenderer.transform.localScale);
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
        // Aquí, podrías reducir la vida y activar el modo peligro
        CambiarModoPeligro(true); // Activamos el modo peligro al reducir vida
    }

    // Método para restaurar la vida
    public void RestaurarVida(float amount)
    {
        CambiarModoPeligro(false); // Desactivamos el modo peligro cuando se restaura vida
    }
}
