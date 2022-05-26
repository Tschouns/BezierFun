using Assets.Scripts.Curves;
using Assets.Scripts.RuntimeChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Moves the object it is attached to along one or several consecutive curve objects.
    /// </summary>
    public class CurveMover : MonoBehaviour
    {
        [InspectorName("Curves")]
        public GameObject[] curveObjects;

        [InspectorName("Moving Speed")]
        public float movingSpeed = 1f;

        private IReadOnlyList<ICurveVertices> curves;
        private float averageVertexDistance;

        private int nextCurve = 0;
        private int nextVertex = 0;
        private Vector3 currentDestination;

        private Vector3 previousPosition;
        private Quaternion previousRotation;

        private void Awake()
        {
            Field.AssertNotNull(this.curveObjects, nameof(curveObjects));
            if (!this.curveObjects.Any())
            {
                throw new InvalidOperationException("The curve mover needs at least one curve.");
            }

            this.curves = this.curveObjects.Select(c => c.GetComponent<ICurveVertices>()).ToList();

        }

        private void Start()
        {
            if (this.curves.Any(c => c.Vertices.Count < 2))
            {
                throw new InvalidOperationException("At least one curve is not a curve. Each curve must have at least two vertices.");
            }

            this.averageVertexDistance = CalcAverageDistance(this.curves.SelectMany(c => c.Vertices).ToList());
            this.NextDestination();

            this.previousPosition = this.transform.position;
            this.previousRotation = this.transform.rotation;
        }

        private void Update()
        {
            // Move.
            var targetOffset = this.currentDestination - this.transform.position;
            var velocity = targetOffset.normalized * this.movingSpeed * Time.deltaTime;

            // Look-rotate.
            var lookRotation = Quaternion.LookRotation(this.currentDestination - this.previousPosition, Vector3.up);

            var currentTargetDistance = Vector3.Distance(this.transform.position, this.currentDestination) - this.averageVertexDistance;
            var originalDistance = Vector3.Distance(this.previousPosition, this.currentDestination) - this.averageVertexDistance;
            var t = Mathf.Clamp(1 - (currentTargetDistance / originalDistance), 0, 1);

            this.transform.rotation = Quaternion.Lerp(this.previousRotation, lookRotation, t);

            // Special case: must not overshoot the target! That would break the whole thing.
            if (velocity.magnitude > targetOffset.magnitude)
            {
                velocity = targetOffset;
            }

            this.transform.position = this.transform.position + velocity;

            // Close to destination.
            if (Vector3.Distance(this.transform.position, this.currentDestination) < this.averageVertexDistance)
            {
                this.NextDestination();

                this.previousPosition = this.transform.position;
                this.previousRotation = this.transform.rotation;
            }
        }

        private static float CalcAverageDistance(IReadOnlyList<Vector3> vertices)
        {
            if (!vertices.Any())
            {
                return 0;
            }

            var total = 0f;
            var previous = vertices[0];
            for (var i = 1; i < vertices.Count; i++)
            {
                total += Vector3.Distance(vertices[i], previous);
                previous = vertices[i];
            }

            return total / vertices.Count;
        }

        private void NextDestination()
        {
            // Set current destination.
            this.currentDestination = this.curves[this.nextCurve].Vertices[this.nextVertex];

            // Iterate.
            this.nextVertex++;
            
            if (this.nextVertex < this.curves[this.nextCurve].Vertices.Count)
            {
                return;
            }
            
            this.nextVertex = 0;
            this.nextCurve++;
            
            if (this.nextCurve < this.curves.Count)
            {
                return;
            }

            this.nextCurve = 0;
        }
    }
}
