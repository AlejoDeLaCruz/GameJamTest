using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public Transform firstCameraPosition;  // Posici�n de la c�mara la primera vez que el jugador pasa
    public Transform secondCameraPosition; // Posici�n de la c�mara la segunda vez que el jugador pasa
    private bool hasTriggeredOnce = false; // Verifica si ya se ha activado antes

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el jugador entra en el collider
        if (collision.CompareTag("Player"))
        {
            // Obt�n la referencia de la c�mara principal
            Camera mainCamera = Camera.main;

            // Cambia la posici�n de la c�mara seg�n el estado
            if (!hasTriggeredOnce)
            {
                // Primera vez que se activa: mueve la c�mara a `firstCameraPosition`
                mainCamera.transform.position = new Vector3(
                    firstCameraPosition.position.x,
                    firstCameraPosition.position.y,
                    mainCamera.transform.position.z
                );
                hasTriggeredOnce = true;
            }
            else
            {
                // Segunda vez que se activa: mueve la c�mara a `secondCameraPosition`
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