using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private float moveSpeed; // Remove initial value
    private Vector3 moveDirection = Vector3.forward;
    private Vector3 endPosition;
    private bool isInitialized = false;

    // The speed parameter is no longer needed here if always randomized
    public void Initialize(Vector3 direction, Vector3 targetEndPosition)
    {
        moveDirection = direction.normalized;
        // Assign a random speed between 1 and 5
        moveSpeed = Random.Range(1f, 5f);
        endPosition = targetEndPosition;
        isInitialized = true;
    }

    void FixedUpdate()
    {
        if (!isInitialized) return;

        // Move the object
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Check if the object has passed the end position
        float dotProduct = Vector3.Dot(endPosition - transform.position, moveDirection);
        if (dotProduct <= 0)
        {
            // Object has passed the end position - destroy it
            Destroy(gameObject);
        }
    }
}