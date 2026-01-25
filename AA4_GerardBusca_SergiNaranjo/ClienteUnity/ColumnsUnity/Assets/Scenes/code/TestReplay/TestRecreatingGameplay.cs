using UnityEngine;

public class TestRecreatingGameplay : MonoBehaviour
{
    public GameObject linePrefab;

    void Update()
    {
        // Por ejemplo, spawneamos al presionar la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnLine();
        }
    }

    void SpawnLine()
    {
        int randomNumber = Random.Range(-3, 3);
        // Puedes elegir la posición donde quieres spawnearlo
        Vector3 spawnPosition = new Vector3(randomNumber, 6.14f, 0f);

        // Instanciamos el prefab
        GameObject newLine = Instantiate(linePrefab, spawnPosition, Quaternion.identity);

        // Opcional: cambiar nombre del objeto instanciado
        newLine.name = "LineInstance";
    }
}
