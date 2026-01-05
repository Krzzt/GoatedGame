using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnValidator : MonoBehaviour
{
    [Header("Validation")]
    [SerializeField] private LayerMask obstacleLayer;

    // Radius for checking obstacles at spawn position
    [SerializeField] private float cheackRadius = 0.5f;

    public bool IsSpawnPositionValid(Vector2 position)
    {
        // Check for obstacles at the spawn position
        Collider2D hitCollider = Physics2D.OverlapCircle(position, cheackRadius, obstacleLayer);
        return hitCollider == null;

    }
}
