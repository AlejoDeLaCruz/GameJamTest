using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Array para los diferentes prefabs de objetos.
    public Transform[] spawnPoints; // Los puntos de aparición (los objetos vacíos en el mapa).
    public float spawnInterval = 15f; // Intervalo de tiempo para que el objeto aparezca de nuevo.

    private void Start()
    {
        // Iniciar la aparición periódica de objetos.
        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            // Esperar hasta que sea el momento de generar el objeto.
            yield return new WaitForSeconds(spawnInterval);

            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        // Elegir un punto de aparición aleatorio.
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Elegir un prefab aleatorio.
        GameObject selectedPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        // Instanciar el objeto en el punto elegido.
        GameObject item = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        // Hacer algo con el item si es necesario (por ejemplo, suscribir a eventos).
        Debug.Log($"Nuevo objeto {item.name} apareció en {spawnPoint.position}");
    }
}