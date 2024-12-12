using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [Header("Building Settings")]
    [SerializeField] private GameObject[] buildingPrefabs; // Array of building prefabs to spawn
    [SerializeField] private int numberOfBuildings = 10;   // Number of buildings to spawn

    [Header("Spawn Area")]
    [SerializeField] private Vector2 spawnAreaMin; // Bottom-left corner of the spawn area
    [SerializeField] private Vector2 spawnAreaMax; // Top-right corner of the spawn area

    [Header("Spacing Settings")]
    [SerializeField] private float minSpacing = 5f; // Minimum distance between buildings

    private void Start()
    {
        SpawnBuildings();
    }

    private void SpawnBuildings()
    {
        int spawnedCount = 0;
        int maxAttempts = numberOfBuildings * 10; // Prevent infinite loops
        int attempts = 0;

        while (spawnedCount < numberOfBuildings && attempts < maxAttempts)
        {
            attempts++;

            // Generate a random position within the spawn area
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            // Check if the position is valid (sufficiently spaced from other buildings)
            if (IsPositionValid(randomPosition))
            {
                // Choose a random building prefab
                GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

                // Spawn the building
                GameObject newBuilding = Instantiate(buildingPrefab, randomPosition, Quaternion.identity);

                // Randomize rotation in 90-degree increments
                float randomRotation = Random.Range(0, 4) * 90f;
                newBuilding.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

                spawnedCount++;
            }
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Could not place all buildings due to spacing constraints.");
        }
    }

    private bool IsPositionValid(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, minSpacing);

        // Check if there are any colliders within the minimum spacing
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Building"))
            {
                return false; // Too close to another building
            }
        }

        return true; // Position is valid
    }

    private void OnDrawGizmos()
    {
        // Draw the spawn area in the Scene view
        Gizmos.color = Color.green;
        Vector3 bottomLeft = new Vector3(spawnAreaMin.x, spawnAreaMin.y, 0);
        Vector3 bottomRight = new Vector3(spawnAreaMax.x, spawnAreaMin.y, 0);
        Vector3 topLeft = new Vector3(spawnAreaMin.x, spawnAreaMax.y, 0);
        Vector3 topRight = new Vector3(spawnAreaMax.x, spawnAreaMax.y, 0);

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
