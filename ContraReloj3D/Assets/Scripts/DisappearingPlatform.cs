using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [Header("Tiempo antes de desaparecer")]
    public float delay = 2f; 

    private bool triggered = false;


    void OnCollisionEnter(Collision collision)
    {
        if (!triggered && collision.gameObject.CompareTag("Player"))
        {
            triggered = true;
            Invoke(nameof(Disappear), delay);
        }
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }

    public void ResetState()
    {
        triggered = false;
        CancelInvoke();
        gameObject.SetActive(true); 
    }
}