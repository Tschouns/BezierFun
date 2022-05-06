
using Assets.Scripts.Curves;
using Assets.Scripts.RuntimeChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Generates a curve / spline along a list of child control points.
/// </summary>
public class CurveGenerator : MonoBehaviour
{
    [InspectorName("Max. Segment Length")]
    public float maxSegmentLength = 1;

    [InspectorName("Bubble Object")]
    public GameObject bubbleObject;

    private IReadOnlyList<ControlPoint> controlPoints;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        this.controlPoints = this.GetComponentsInChildren<ControlPoint>();
        this.lineRenderer = this.GetComponent<LineRenderer>();

        Field.AssertNotNull(this.controlPoints, nameof(this.controlPoints));
        Field.AssertNotNull(this.lineRenderer, nameof(this.lineRenderer));

        var vertices = CurveHelper.GetCurveVertices(this.controlPoints.Select(p => p.Position).ToArray(), this.maxSegmentLength);
        this.lineRenderer.positionCount = vertices.Length;
        this.lineRenderer.SetPositions(vertices);
    }
}
