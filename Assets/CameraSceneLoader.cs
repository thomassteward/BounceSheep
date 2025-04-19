using UnityEngine;
using UnityEngine.SceneManagement;  // For scene management
using System.Collections;

public class CameraSceneLoader : MonoBehaviour
{
    public float delayTime = 5f;  // Delay before loading the scene

    void Start()
    {
        // Start the coroutine to delay the scene transition
        StartCoroutine(DelayAndLoadMainMenu(delayTime));
    }

    // Coroutine to wait before loading the scene
    IEnumerator DelayAndLoadMainMenu(float delay)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // After the delay, load the Main Menu scene
        SceneManager.LoadScene("Main Menu");
    }
}
