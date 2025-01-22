using UnityEngine;

public class Papers : MonoBehaviour
{
    public string itemName; // Nombre del objeto.
    public Sprite itemIcon; // Icono del objeto.
    public Transform attachPoint; // Punto donde se adherirá el objeto.
    public delegate void ItemDestroyed(Vector3 spawnPosition); // Evento con la posición del spawn.
    public event ItemDestroyed onItemDestroyed; // Evento que se llama cuando el objeto se destruye.
    public AudioSource audioSource; // Componente de AudioSource para reproducir sonidos.

    private bool playerInRange = false;
    private Inventory playerInventory;
    private Movement playerMovement;
    private bool inEmptyZone = false;

    private float speedAdjustment = 0f;

    private void Start()
    {
        // Ajuste de velocidad según el layer del objeto.
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

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogWarning("No se encontró un AudioSource en el objeto.");
            }
        }
    }

    private void OnDestroy()
    {
        // Llamar al evento cuando el objeto sea destruido.
        if (onItemDestroyed != null)
        {
            onItemDestroyed.Invoke(transform.position); // Pasar la posición del objeto al invocar el evento.
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null)
            {
                if (playerInventory.currentItem != null)
                {
                    DropItem(playerInventory.currentItem);
                }

                playerInventory.AddItem(this);
                playerMovement?.AdjustSpeed(speedAdjustment);
                AttachToPlayer();

                if (audioSource != null)
                {
                    audioSource.Play();
                }
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

            if (playerInventory != null && attachPoint == null)
            {
                attachPoint = playerInventory.transform.Find("AttachPoint");
                if (attachPoint == null)
                {
                    Debug.LogWarning("El punto de agarre no está configurado en el jugador.");
                }
            }
        }

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
            playerInventory.RemoveCurrentItem();
            Destroy(gameObject);
        }
    }

    private void AttachToPlayer()
    {
        if (attachPoint != null)
        {
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
        itemToDrop.transform.SetParent(null);
        itemToDrop.transform.position = playerInventory.transform.position;
    }
}
