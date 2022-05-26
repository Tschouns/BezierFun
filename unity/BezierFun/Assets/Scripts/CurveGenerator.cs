
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
        Field.AssertNotNull(this.lineRenderer, nameof(this.lineRenderer));

        this.vertices = CurveHelper.GetCurveVertices(this.controlPoints.Select(p => p.Position).ToArray(), this.maxSegmentLength);
        this.lineRenderer.positionCount = this.vertices.Length;
        this.lineRenderer.SetPositions(this.vertices);
    }
}
