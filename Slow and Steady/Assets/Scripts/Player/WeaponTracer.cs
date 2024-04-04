using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTracer
{

    public static void Create(Vector3 fromPosition, Vector3 targetPosition, Material tracerMaterial)
    {
        // Create a new GameObject to hold the LineRenderer component
        GameObject tracerGameObject = new GameObject("BulletTracer");
        LineRenderer lineRenderer = tracerGameObject.AddComponent<LineRenderer>();

        // Set the material to the LineRenderer
        lineRenderer.material = tracerMaterial;

        // Set the width of the LineRenderer
        lineRenderer.startWidth = 0.2f; // Start width of the line
        lineRenderer.endWidth = 0.2f;   // End width of the line

        // Set the positions
        lineRenderer.SetPosition(0, fromPosition);
        lineRenderer.SetPosition(1, targetPosition);

        lineRenderer.sortingLayerName = "BulletTraceLayer"; // Make sure this sorting layer exists
        lineRenderer.sortingOrder = 100; // Adjust this value to ensure it's rendered above other objects

        // Optional: Add a script to fade out and destroy the line after a short duration
        tracerGameObject.AddComponent<BulletTracerBehaviour>().BeginFadeOut(0.4f); // Adjust time as needed
    }
}

public class BulletTracerBehaviour : MonoBehaviour
{
    public float fadeOutTime = 0.1f;

    private LineRenderer lineRenderer;
    private float startTime;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        startTime = Time.time;
    }

    private void Update()
    {
        float elapsed = (Time.time - startTime) / fadeOutTime;

        // Fade out the line renderer over time
        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;
        startColor.a = Mathf.Lerp(1f, 0f, elapsed);
        endColor.a = startColor.a;

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        // Destroy the line after fadeOutTime has elapsed
        if (elapsed >= 1f)
        {
            Destroy(gameObject);
        }
    }

    public void BeginFadeOut(float fadeOutTime)
    {
        this.fadeOutTime = fadeOutTime;
    }
}