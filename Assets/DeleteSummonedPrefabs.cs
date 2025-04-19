using UnityEngine;
using System.Collections.Generic;

public class DeleteSummonedPrefabs : MonoBehaviour
{
    public List<GameObject> objectsToKeep = new List<GameObject>(); // List of objects to keep
    public AudioClip deleteSound;
    public AudioSource audioSource;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // This function is called when the button is pressed
    public void DeleteAllPrefabs()
    {
        // Play sound effect
        if (deleteSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deleteSound);
        }

        // Get all objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            // Only destroy objects that are NOT in the keep list (or their parents aren't)
            if (!IsInKeepList(obj))
            {
                Destroy(obj);
            }
        }
    }

    // Check if the object or any of its parents are in the "objectsToKeep" list
    private bool IsInKeepList(GameObject obj)
    {
        Transform currentTransform = obj.transform;

        while (currentTransform != null)
        {
            if (objectsToKeep.Contains(currentTransform.gameObject))
            {
                return true;
            }
            currentTransform = currentTransform.parent;
        }

        return false;
    }
}
