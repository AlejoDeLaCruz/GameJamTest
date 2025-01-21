using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Papers> inventory = new List<Papers>();

    public void AddItem(Papers item)
    {
        // Agregar el objeto al inventario
        inventory.Add(item);
        Debug.Log($"Objeto recogido: {item.itemName}");
    }
}