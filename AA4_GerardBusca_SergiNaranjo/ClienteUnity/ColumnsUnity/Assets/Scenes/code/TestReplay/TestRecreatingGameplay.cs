using UnityEngine;

public class TestRecreatingGameplay : MonoBehaviour
{
    public GameObject linePrefab;


    void Start()
    {
        InvokeRepeating(nameof(SpawnLine), 0f, 2f);
    }

    void SpawnLine()
    {
        int randomNumber = Random.Range(-3, 3);
        Vector3 spawnPosition = new Vector3(randomNumber, 6.14f, 0f);

        GameObject newLine = Instantiate(linePrefab, spawnPosition, Quaternion.identity);

        newLine.name = "LineInstance";
    }
}
