using UnityEngine;

public class TempBehavior : MonoBehaviour
{
    public float teleportInterval = 1f;  // Interval for teleporting
    public float flipSpeed = 50f;        // Speed at which the object flips
    public Vector2 teleportRange = new Vector2(10f, 10f);  // Max range for teleportation
    private float nextTeleportTime;

    void Start()
    {
        // Initial teleporting and flipping behavior
        nextTeleportTime = Time.time + teleportInterval;
    }

    void Update()
    {
        // Rotate the object continuously
        transform.Rotate(Vector3.up, flipSpeed * Time.deltaTime); // Rotating around the Y-axis (or any axis)

        // Teleport the object at regular intervals
        if (Time.time >= nextTeleportTime)
        {
            TeleportRandomly();
            nextTeleportTime = Time.time + teleportInterval;
        }
    }

    // Teleport the object to a random position within a defined range
    private void TeleportRandomly()
    {
        float randomX = Random.Range(-teleportRange.x, teleportRange.x);
        float randomY = Random.Range(-teleportRange.y, teleportRange.y);
        transform.position = new Vector3(randomX, randomY, transform.position.z);
    }
}
