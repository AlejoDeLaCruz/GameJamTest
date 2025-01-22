using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public Transform firstCameraPosition;  // Posición de la cámara la primera vez que el jugador pasa
    public Transform secondCameraPosition; // Posición de la cámara la segunda vez que el jugador pasa
    private bool hasTriggeredOnce = false; // Verifica si ya se ha activado antes

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el jugador entra en el collider
        if (collision.CompareTag("Player"))
        {
            // Obtén la referencia de la cámara principal
            Camera mainCamera = Camera.main;

            // Cambia la posición de la cámara según el estado
            if (!hasTriggeredOnce)
            {
                // Primera vez que se activa: mueve la cámara a `firstCameraPosition`
                mainCamera.transform.position = new Vector3(
                    firstCameraPosition.position.x,
                    firstCameraPosition.position.y,
                    mainCamera.transform.position.z
                );
                hasTriggeredOnce = true;
            }
            else
            {
                // Segunda vez que se activa: mueve la cámara a `secondCameraPosition`
                mainCamera.transform.position = new Vector3(
                    secondCameraPosition.position.x,
                    secondCameraPosition.position.y,
                    mainCamera.transform.position.z
                );
                hasTriggeredOnce = false; // Resetea para alternar al cruzar de nuevo
            }
        }
    }
}