using UnityEngine;
using System.Collections;

public class oaky : MonoBehaviour
{
    private Animator animator;
    public float delay = 2f; // X seconds delay, you can change this in the Inspector

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void yes()
    {
        if (animator != null)
        {
            animator.SetBool("ready", false);
            StartCoroutine(SetReadyFalseAfterDelay());
        }
    }

    private IEnumerator SetReadyFalseAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("ready", true);
    }
}
