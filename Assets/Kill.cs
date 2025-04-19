using UnityEngine;
using UnityEngine.SceneManagement;  // For scene management
using System.Collections;         // For IEnumerator

public class Kill : MonoBehaviour
{
    public AudioClip spawnSound;

    private string myObjectTag = "KillScriptObject"; // Tag for the object

    void Start()
    {
        // Assign a unique tag to the object for later exclusion
        this.gameObject.tag = myObjectTag;

        kill();
    }

    void Update()
    {
        kill();
    }

    public void kill()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            // Skip deleting if:
            // - It's the game object this script is attached to (using the tag)
            // - It's named "iselotelf"
            // - It has a Camera component (e.g. the Main Camera)
            if (
                obj.CompareTag(myObjectTag) || // Check for the tag
                obj.name == "iselotelf" ||
                obj.GetComponent<Camera>() != null
            )
            {
                continue;
            }

            // Play spawn sound if specified
            if (spawnSound != null)
            {
                AudioSource audio = obj.GetComponent<AudioSource>();
                if (audio == null) audio = obj.AddComponent<AudioSource>();
                audio.PlayOneShot(spawnSound);
            }

            // Add teleport + auto-destroy behavior
            if (obj.GetComponent<TempBehavior>() == null)
            {
                obj.AddComponent<TempBehavior>();
            }

            Destroy(obj, 10f); // Delete after 10 seconds
        }

        // Start the coroutine to delay the scene transition
        StartCoroutine(DelayAndLoadMainMenu(15f));
    }

    // This is the coroutine with a delay
    IEnumerator DelayAndLoadMainMenu(float delayTime)
    {
        // Wait for the delay time before transitioning to the next scene
        yield return new WaitForSeconds(delayTime);

        // After the delay, load the Main Menu scene
        SceneManager.LoadScene("Main Menu");
    }
}
