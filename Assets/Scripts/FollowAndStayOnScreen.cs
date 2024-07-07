using UnityEngine;

public class FollowAndStayOnScreen : MonoBehaviour
{
    private Transform target; // The target the object is trying to reach
    public float speed = 100f; // Movement speed towards the target

    private Camera mainCamera;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        mainCamera = Camera.main;
        // Calculate object bounds
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    void Update()
    {
        // Find all GameObjects with the specified tag
        GameObject[] dockObjects = GameObject.FindGameObjectsWithTag("Dock");

        if (dockObjects.Length > 0)
        {
            Transform closestDock = dockObjects[0].transform;
            float closestDistance = Vector3.Distance(transform.position, closestDock.position);

            // Iterate through all dock objects to find the closest one
            foreach (var dock in dockObjects)
            {
                float distance = Vector3.Distance(transform.position, dock.transform.position);
                if (distance < closestDistance)
                {
                    closestDock = dock.transform;
                    closestDistance = distance;
                }
            }

            target = closestDock;
        }

        // Move towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        // Calculate screen bounds in world coordinates
        Vector3 minScreenBounds = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
        Vector3 maxScreenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Clamp the new position to keep the object within screen bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minScreenBounds.x + objectWidth, maxScreenBounds.x - objectWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, minScreenBounds.y + objectHeight, maxScreenBounds.y - objectHeight);

        // Update the object's position
        transform.position = newPosition;
    }
}
