using UnityEngine;
using UnityEngine.SceneManagement; // Required to load scenes

public class SceneLoader : MonoBehaviour
{
    // Public method to load a scene by name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Optional: Load scene by index
    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}
