using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minSpawnTime = 1.5f;
    [SerializeField] private float maxSpawnTime = 3.5f;
    
    [Header("Obstacle Settings")]
    [SerializeField] private GameObject plane;
    [SerializeField] private Vector3 moveDirection = Vector3.forward;
    [SerializeField] private float moveSpeed = 5f;
    
    private bool isSpawning = false;
    private Vector3 endPosition;
    private Vector3 startPosition; // Add this field
    
    void Start()
    {
        // If no spawn point is set, use this object's position
        if (spawnPoint == null)
            spawnPoint = transform;
            
        // Calculate end position from plane
        CalculateEndPosition();
        
        // Start spawning obstacles automatically
        StartSpawning();
    }
    
    private void CalculateEndPosition()
    {
        if (plane == null)
        {
            plane = GameObject.FindGameObjectWithTag("Ground");
            if (plane == null)
            {
                Debug.LogError("No plane assigned and couldn't find one with 'Ground' tag!");
                return;
            }
        }
        
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        if (planeRenderer != null)
        {
            // Calculate boundary
            Bounds planeBounds = planeRenderer.bounds;
            
            // Calculate half length in the direction of movement
            float halfLength = Vector3.Scale(planeBounds.size, moveDirection.normalized).magnitude / 2;
            
            // Calculate start position at beginning edge of the plane
            startPosition = plane.transform.position - moveDirection.normalized * halfLength;
            
            // Set end position at the far edge of the plane
            endPosition = plane.transform.position + moveDirection.normalized * halfLength;
            
            // Optionally visualize in debug mode
            Debug.DrawLine(startPosition, endPosition, Color.yellow, 2.0f);
        }
        else
        {
            Debug.LogError("No renderer component found on the plane!");
        }
    }
    
    // Start the spawning process
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }
    
    // Stop the spawning process
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
    
    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            SpawnObstacle();
            
            // Wait for random time before next spawn
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    private void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Obstacle prefab is missing!");
            return;
        }
        
        // Use the calculated start position instead of spawn point
        GameObject obstacle = Instantiate(obstaclePrefab, startPosition, Quaternion.identity);
        
        // Configure the obstacle's movement properties
        ObstacleMove obstacleMove = obstacle.GetComponent<ObstacleMove>();
        if (obstacleMove != null)
        {
            // Remove the moveSpeed argument as it's now randomized within ObstacleMove
            obstacleMove.Initialize(moveDirection, endPosition);
        }
    }
}
