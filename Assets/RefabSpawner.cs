using UnityEngine;

public class RefabSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PrefabProbability
    {
        public GameObject prefab;
        public float probability; // Probability of this prefab being chosen (0 to 1)
    }

    public PrefabProbability[] prefabsWithProbabilities; // Array of prefabs with associated probabilities
    public float spawnAngle = 45f; // The angle at which the prefabs will be fired (in degrees)
    public float spawnForce = 20f; // Increased force with which the prefabs will be fired
    public float spawnInterval = 2f; // Time in seconds between each spawn
    public float destroyAfterSeconds = 3f; // Time in seconds after which the prefabs will be destroyed
    public Transform spawnPoint; // The point from which prefabs will be spawned

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform; // Use the object's position if no spawn point is specified
        }
        // Start spawning prefabs
        InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
    }

    // Spawns a prefab and applies force to it
    void SpawnPrefab()
    {
        // Pick a prefab based on probabilities
        GameObject selectedPrefab = SelectPrefabBasedOnProbability();

        // Instantiate the selected prefab at the spawn point
        GameObject newPrefab = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        // Get the Rigidbody2D component of the spawned prefab (assuming it's a 2D game)
        Rigidbody2D rb = newPrefab.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Convert spawn angle to radians and apply force at that angle
            float angleInRadians = spawnAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

            // Apply force to launch the prefab at the specified angle with increased force
            rb.AddForce(direction * spawnForce, ForceMode2D.Impulse);
        }

        // Destroy the prefab after 'destroyAfterSeconds' time
        Destroy(newPrefab, destroyAfterSeconds);
    }

    // Selects a prefab based on the set probabilities
    GameObject SelectPrefabBasedOnProbability()
    {
        float totalProbability = 0f;

        // Sum up all the probabilities
        foreach (var item in prefabsWithProbabilities)
        {
            totalProbability += item.probability;
        }

        // Pick a random value between 0 and the total probability
        float randomValue = Random.value * totalProbability;

        float cumulativeProbability = 0f;

        // Select the prefab based on the random value
        foreach (var item in prefabsWithProbabilities)
        {
            cumulativeProbability += item.probability;

            if (randomValue <= cumulativeProbability)
            {
                return item.prefab;
            }
        }

        // Default return the first prefab if no match (should not happen if probabilities are correct)
        return prefabsWithProbabilities[0].prefab;
    }
}
