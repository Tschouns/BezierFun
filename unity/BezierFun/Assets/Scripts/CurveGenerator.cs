
using Assets.Scripts.Curves;
using Assets.Scripts.RuntimeChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Generates a curve / spline along a list of child control points.
/// </summary>
public class CurveGenerator : MonoBehaviour, ICurveVertices
{
    [InspectorName("Max. Segment Length")]
    public float maxSegmentLength = 1;

    [InspectorName("Render Line")]
    public bool renderLine = true;

    [InspectorName("Spawn Bubbles")]
    public bool spawnBubbles = true;

    [InspectorName("Bubble Object")]
    public GameObject bubbleObject;

    private IReadOnlyList<ControlPoint> controlPoints;
    private LineRenderer lineRenderer;

    private Vector3[] vertices;

    public IReadOnlyList<Vector3> Vertices
    {
        get
        {
            if (this.vertices == null)
            {
                throw new InvalidOperationException("The curve vertices have not been initialized.");
            }

            return this.vertices;
        }
    }

    private void Awake()
    {
        this.controlPoints = this.GetComponentsInChildren<ControlPoint>();
        this.lineRenderer = this.GetComponent<LineRenderer>();

        Field.AssertNotNull(this.controlPoints, nameof(this.controlPoints));

        this.vertices = CurveHelper.GetCurveVertices(this.controlPoints.Select(p => p.Position).ToArray(), this.maxSegmentLength);

        // Spawn bubbles.
        if (this.spawnBubbles &&
            this.bubbleObject != null)
        {
            foreach (var vertex in vertices)
            {
                Instantiate(bubbleObject, vertex, Quaternion.identity);
            }
        }

        // Draw line.
        if (this.renderLine &&
            this.lineRenderer != null)
        {
            this.lineRenderer.positionCount = this.vertices.Length;
            this.lineRenderer.SetPositions(this.vertices);
        }

        // Deactivate the control points.
        foreach (var point in this.controlPoints)
        {
            point.Remove();
        }
    }
}
