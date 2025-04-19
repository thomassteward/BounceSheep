using UnityEngine;

public class ToggleCameraSize : MonoBehaviour
{
    public Camera cameraToToggle; // The camera you want to toggle
    public float size1 = 5f;      // First camera size (for orthographic) or field of view (for perspective)
    public float size2 = 10f;     // Second camera size (for orthographic) or field of view (for perspective)

    private bool isSize1 = true;  // Track the current state of the camera size

    void Start()
    {
        if (cameraToToggle == null)
        {
            cameraToToggle = Camera.main; // Default to the main camera if none is assigned
        }
    }

    // This function is called when the button is pressed
    public void ToggleSize()
    {
        if (cameraToToggle.orthographic)
        {
            // For orthographic cameras, change the orthographic size
            cameraToToggle.orthographicSize = isSize1 ? size2 : size1;
        }
        else
        {
            // For perspective cameras, change the field of view
            cameraToToggle.fieldOfView = isSize1 ? size2 : size1;
        }

        // Toggle the state
        isSize1 = !isSize1;
    }
}
