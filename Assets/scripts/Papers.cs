using UnityEngine;

public class Papers : MonoBehaviour
{
    public string itemName; // Nombre del objeto para identificarlo
    public Sprite itemIcon; // Icono del objeto, opcional (útil para un inventario visual)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Intentamos acceder al inventario del jugador
            Inventory playerInventory = collision.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(this); // Agregamos el objeto al inventario
                Destroy(gameObject); // Destruimos el objeto en la escena
            }
        }
    }
}
