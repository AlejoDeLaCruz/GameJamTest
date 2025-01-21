using UnityEngine;

public class HolePatching : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto que entra tiene el tag "Player".
        if (collision.CompareTag("Player"))
        {
            Inventory playerInventory = collision.GetComponent<Inventory>();

            if (playerInventory != null && playerInventory.currentItem != null)
            {
                // Quitamos el objeto del inventario.
                Debug.Log($"El inventario ha sido vaciado. Objeto soltado: {playerInventory.currentItem.itemName}");

                // Reactivamos el objeto en la posición del jugador.
                playerInventory.currentItem.gameObject.SetActive(true);
                playerInventory.currentItem.transform.SetParent(null); // Lo quitamos como hijo del jugador.
                playerInventory.currentItem.transform.position = collision.transform.position;

                // Limpiamos el inventario.
                playerInventory.currentItem = null;
            }
            else
            {
                Debug.Log("El jugador no tiene ningún objeto en su inventario.");
            }
        }
    }
}
