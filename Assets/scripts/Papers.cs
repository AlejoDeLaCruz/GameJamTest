using UnityEngine;

public class Papers : MonoBehaviour
{
    public string itemName; // Nombre del objeto.
    public Sprite itemIcon; // Icono del objeto.

    private bool playerInRange = false; // Indica si el jugador está en el rango del objeto.
    private Inventory playerInventory; // Referencia al inventario del jugador.
    private Movement playerMovement; // Referencia al script de movimiento.
    private bool inEmptyZone = false; // Indica si el objeto está en la zona que vacía el inventario.

    // Variables para ajustes de velocidad.
    private float speedAdjustment = 0f; // Ajuste por defecto.

    private void Start()
    {
        // Determinamos el ajuste de velocidad según la layer del objeto.
        if (gameObject.layer == LayerMask.NameToLayer("heavyPaper"))
        {
            speedAdjustment = -3f; // Reducir significativamente la velocidad.
        }
        else if (gameObject.layer == LayerMask.NameToLayer("midPaper"))
        {
            speedAdjustment = -2f; // Reducir moderadamente la velocidad.
        }
        else if (gameObject.layer == LayerMask.NameToLayer("lightPaper"))
        {
            speedAdjustment = 0f; // Sin ajuste.
        }
    }

    private void Update()
    {
        // Verificamos si el jugador está en rango y presiona la tecla E.
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null)
            {
                // Si ya hay un objeto en el inventario, lo soltamos.
                if (playerInventory.currentItem != null)
                {
                    DropItem(playerInventory.currentItem);
                }

                // Agregamos el nuevo objeto al inventario.
                playerInventory.AddItem(this);

                // Ajustamos la velocidad del jugador según el tipo de objeto.
                if (playerMovement != null)
                {
                    playerMovement.AdjustSpeed(speedAdjustment);
                }

                gameObject.SetActive(false); // Desactivamos este objeto en la escena.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true; // Jugador entra en el rango.
            playerInventory = collision.GetComponent<Inventory>(); // Guardamos la referencia al inventario.
            playerMovement = collision.GetComponent<Movement>(); // Guardamos la referencia al movimiento.
        }

        // Detectamos si el objeto entra en la zona que vacía el inventario.
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
            playerMovement = null; // Eliminamos la referencia al movimiento.
        }

        // Detectamos si el objeto sale de la zona que vacía el inventario.
        if (collision.GetComponent<HolePatching>() != null)
        {
            inEmptyZone = false;
        }
    }

    private void HandleEmptyZone()
    {
        if (inEmptyZone && playerInventory != null && playerInventory.currentItem == this)
        {
            Debug.Log($"El objeto {itemName} fue destruido al entrar en la zona que vacía el inventario.");
            playerInventory.RemoveCurrentItem(); // Removemos el objeto del inventario.

            Destroy(gameObject); // Destruimos el objeto en la escena.
        }
    }

    private void DropItem(Papers itemToDrop)
    {
        Debug.Log($"Soltaste: {itemToDrop.itemName}");

        // Activamos el objeto soltado y lo colocamos en la posición del jugador.
        itemToDrop.gameObject.SetActive(true);
        itemToDrop.transform.position = playerInventory.transform.position;
        itemToDrop.transform.SetParent(null); // Lo desacoplamos del jugador.

    }
}
