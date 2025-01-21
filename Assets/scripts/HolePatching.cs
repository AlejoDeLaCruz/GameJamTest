using UnityEngine;

public class HolePatching : MonoBehaviour
{
    public Timer countdownTimer; // Referencia al script que controla el temporizador.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto que entra tiene el tag "Player".
        if (collision.CompareTag("Player"))
        {
            Inventory playerInventory = collision.GetComponent<Inventory>();
            Movement playerMovement = collision.GetComponent<Movement>(); // Referencia al script de movimiento del jugador.

            if (playerInventory != null && playerInventory.currentItem != null)
            {
                // Obtenemos una referencia al objeto actual en el inventario.
                Papers currentItem = playerInventory.currentItem;

                // Aumentamos el tiempo dependiendo de la layer del objeto.
                float timeToAdd = 0f;

                // Dependiendo de la layer del objeto, agregamos más tiempo.
                if (currentItem.gameObject.layer == LayerMask.NameToLayer("heavyPaper"))
                {
                    timeToAdd = 10f; // Añadir 10 segundos para "heavyPaper".
                    // Ajustamos la velocidad a una reducción significativa.

                }
                else if (currentItem.gameObject.layer == LayerMask.NameToLayer("midPaper"))
                {
                    timeToAdd = 7f; // Añadir 7 segundos para "midPaper".
                    // Ajustamos la velocidad a una reducción moderada.

                }
                else if (currentItem.gameObject.layer == LayerMask.NameToLayer("lightPaper"))
                {
                    timeToAdd = 5f; // Añadir 5 segundos para "lightPaper".
                    // Sin ajuste de velocidad.

                }

                // Aumentamos el tiempo en el temporizador.
                if (countdownTimer != null)
                {
                    countdownTimer.AddTime(timeToAdd);
                }

                // Quitamos el objeto del inventario.
                Debug.Log($"El objeto {currentItem.itemName} fue destruido al entrar en la zona vacía.");

                // Destruimos el objeto.
                Destroy(currentItem.gameObject);

                // Limpiamos el inventario.
                playerInventory.currentItem = null;

                // Restauramos la velocidad al valor base al salir de la zona.
                if (playerMovement != null)
                {
                    playerMovement.AdjustSpeed(0f); // Ajuste a velocidad base (sin ningún objeto).
                }
            }
            else
            {
                Debug.Log("El jugador no tiene ningún objeto en su inventario.");
            }
        }
    }
}