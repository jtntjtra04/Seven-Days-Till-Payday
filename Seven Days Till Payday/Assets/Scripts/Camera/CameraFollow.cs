using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cameraTransform;
    public float orthoSize = 7f; // Camera size
    public float aspectRatio = 16f / 9f; // Screen aspect ratio
    public Collider2D confinerCollider;

    private float camHeight;
    private float camWidth;
    private Bounds mapBounds;

    void Start()
    {
        // Calculate camera viewport extents
        camHeight = orthoSize;
        camWidth = orthoSize * aspectRatio;

        // Get bounds from collider
        if (confinerCollider != null)
            mapBounds = confinerCollider.bounds;
    }

    void LateUpdate()
    {
        Vector3 camPosition = cameraTransform.position;

        // Clamp camera position to stay within bounds
        float minX = mapBounds.min.x + camWidth;
        float maxX = mapBounds.max.x - camWidth;
        float minY = mapBounds.min.y + camHeight;
        float maxY = mapBounds.max.y - camHeight;

        camPosition.x = Mathf.Clamp(camPosition.x, minX, maxX);
        camPosition.y = Mathf.Clamp(camPosition.y, minY, maxY);

        cameraTransform.position = camPosition;
    }
}