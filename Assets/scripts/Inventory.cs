using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Papers currentItem; // El objeto actualmente en el inventario.

    public void AddItem(Papers newItem)
    {
        // Si ya hay un objeto en el inventario, lo soltamos antes de agregar el nuevo.
        if (currentItem != null)
        {
            Debug.Log($"Ya tienes un objeto en el inventario: {currentItem.itemName}. Será reemplazado por {newItem.itemName}.");
            DropCurrentItem();
        }

        // Agregamos el nuevo objeto al inventario.
        currentItem = newItem;
        currentItem.transform.SetParent(transform); // Hacemos al objeto hijo del jugador.
        Debug.Log($"Nuevo objeto en el inventario: {currentItem.itemName}");
    }

    private void DropCurrentItem()
    {
        // Reactivamos el objeto actual y lo removemos del inventario.
        currentItem.gameObject.SetActive(true);
        currentItem.transform.SetParent(null); // Lo quitamos como hijo del jugador.

        // Posicionamos el objeto actual donde está el jugador.
        currentItem.transform.position = transform.position;

        Debug.Log($"Objeto soltado: {currentItem.itemName}");

        // Quitamos el objeto actual del inventario.
        currentItem = null;
    }

    private void Update()
    {
        // Debug adicional para verificar la cantidad de objetos en el inventario.
        if (currentItem != null)
        {
            Debug.Log($"Inventario: 1 objeto ({currentItem.itemName})");
        }
        else
        {
            Debug.Log("Inventario: Vacío");
        }
    }
}