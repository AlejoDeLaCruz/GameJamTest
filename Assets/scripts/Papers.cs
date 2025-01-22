using UnityEngine;

public class Papers : MonoBehaviour
{
    public string itemName; // Nombre del objeto.
    public Sprite itemIcon; // Icono del objeto.
    public Transform attachPoint; // Punto donde se adherirá el objeto.

    private bool playerInRange = false; // Indica si el jugador está en el rango del objeto.
    private Inventory playerInventory; // Referencia al inventario del jugador.
    private Movement playerMovement; // Referencia al script de movimiento.
    private bool inEmptyZone = false; // Indica si el objeto está en la zona que vacía el inventario.

    // Variables para ajustes de velocidad.
    private float speedAdjustment = 0f; // Ajuste por defecto.

    private void Start()
    {
        // Determina el ajuste de velocidad según el layer del objeto.
        switch (LayerMask.LayerToName(gameObject.layer))
        {
            case "heavyPaper":
                speedAdjustment = -1.5f;
                break;
            case "midPaper":
                speedAdjustment = -1f;
                break;
            case "lightPaper":
                speedAdjustment = 0f;
                break;
            default:
                Debug.LogWarning($"El objeto {gameObject.name} tiene un layer desconocido.");
                break;
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
                playerMovement?.AdjustSpeed(speedAdjustment);

                // Adjuntamos el objeto al punto de agarre.
                AttachToPlayer();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = collision.GetComponent<Inventory>();
            playerMovement = collision.GetComponent<Movement>();

            // Validar si el punto de agarre está asignado.
            if (playerInventory != null && attachPoint == null)
            {
                attachPoint = playerInventory.transform.Find("AttachPoint");
                if (attachPoint == null)
                {
                    Debug.LogWarning("El punto de agarre no está configurado en el jugador.");
                }
            }
        }

        // Detectamos si el objeto entra en la zona que vacía el inventario.
        if (collision.GetComponent<HolePatching>() != null)
        {
            inEmptyZone = true;
            HandleEmptyZone();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
            playerMovement = null;
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

            // Removemos el objeto del inventario.
            playerInventory.RemoveCurrentItem();

            // Destruimos el objeto en la escena.
            Destroy(gameObject);
        }
    }

    private void AttachToPlayer()
    {
        if (attachPoint != null)
        {
            // Mueve el objeto al punto de agarre y lo hace hijo de este.
            transform.SetParent(attachPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            Debug.Log($"El objeto {itemName} se ha adherido al jugador.");
        }
        else
        {
            Debug.LogWarning("El punto de agarre no está asignado.");
        }
    }

    private void DropItem(Papers itemToDrop)
    {
        Debug.Log($"Soltaste: {itemToDrop.itemName}");

        // Desacopla el objeto y lo coloca en la posición actual del jugador.
        itemToDrop.transform.SetParent(null);
        itemToDrop.transform.position = playerInventory.transform.position;
    }
}