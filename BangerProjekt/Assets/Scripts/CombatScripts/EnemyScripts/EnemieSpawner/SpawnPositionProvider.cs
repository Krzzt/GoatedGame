using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionProvider : MonoBehaviour
{
    // Reference to the player transform
    [Header("Reference")]
    // player transform to spawn around
    [SerializeField] private Transform playerTransform;

    // Spawn distance settings
    [Header("Spawn Distance")]
    [SerializeField] private float minSpawnDistance = 6f;
    [SerializeField] private float maxSpawnDistance = 12f;

    // Get a random spawn position around the player
    public Vector2 GetSpawnPosition()
    {
        // Generate a random direction and distance
        Vector2 direction = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Calculate spawn position
        Vector2 spawnPosition = (Vector2)playerTransform.position + direction * distance;
        return spawnPosition;
    }


    // Missing things are
    // - Kamera-Check
    // - Obstacle-Check
    // - Map-Boundary-Check
    // - repeat until valid position found or so......?!
    // - Gizmo drawing for spawn area visualization(debuging?..)
}
