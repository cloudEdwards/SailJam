using UnityEngine;

public class WindTracker : MonoBehaviour
{
    public Transform targetObject; // The object to track

    void Update()
    {
        if (targetObject != null)
        {
            // Update this GameObject's position to match the targetObject's position
            transform.position = targetObject.position;
        }
    }
}