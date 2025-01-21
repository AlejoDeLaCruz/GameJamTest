using UnityEngine;

public class Papers : MonoBehaviour
{
    public string itemName; // Nombre del objeto.
    public Sprite itemIcon; // Icono del objeto.

    private bool playerInRange = false; // Indica si el jugador est� en el rango del objeto.
    private Inventory playerInventory; // Referencia al inventario del jugador.
    private bool inEmptyZone = false; // Indica si el objeto est� en la zona que vac�a el inventario.

    private void Update()
    {
        // Verificamos si el jugador est� en rango y presiona la tecla E.
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null)
            {
                playerInventory.AddItem(this); // Agregamos el objeto al inventario.
                gameObject.SetActive(false); // Desactivamos el objeto en la escena.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true; // Jugador entra en el rango.
            playerInventory = collision.GetComponent<Inventory>(); // Guardamos la referencia al inventario.
        }

        // Detectamos si el objeto entra en la zona que vac�a el inventario.
        if (collision.GetComponent<HolePatching>() != null)
        {
            inEmptyZone = true;
            HandleEmptyZone(); // Manejar el comportamiento al entrar en la zona.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false; // Jugador sale del rango.
            playerInventory = null; // Eliminamos la referencia al inventario.
        }

        // Detectamos si el objeto sale de la zona que vac�a el inventario.
        if (collision.GetComponent<HolePatching>() != null)
        {
            inEmptyZone = false;
        }
    }

    private void HandleEmptyZone()
    {
        if (inEmptyZone)
        {
            Debug.Log($"El objeto {itemName} desapareci� al entrar en la zona que vac�a el inventario.");
            gameObject.SetActive(false); // Desactiva el objeto en la escena.
        }
    }
}