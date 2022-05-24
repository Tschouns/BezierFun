using Assets.Scripts.RuntimeChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public static class CurveHelper
    {
        /// <summary>
        /// Generates a list of vertices along a Bezier curve, with the specified max distance in between each pair of vertices.
        /// </summary>
        /// <param name="controlPoints">
        /// The control points for the Bezier curve
        /// </param>
        /// <param name="maxDistance">
        /// The max distance between two vertices
        /// </param>
        /// <returns>
        /// The list of vertices
        /// </returns>
        public static Vector3[] GetCurveVertices(IReadOnlyList<Vector3> controlPoints, float maxDistance)
        {
            Argument.AssertNotNull(controlPoints, nameof(controlPoints));

            // Calculate the total number of segments.
            var totalNumberOfSegments = 0;
            Vector3? lastControlPoint = null;

            foreach (var controlPoint in controlPoints)
            {
                // First step: special case.
                if (lastControlPoint == null)
                {
                    lastControlPoint = controlPoint;
                    continue;
                }

                var numberOfSegments = CalculateNumberOfSegmentsBetween(controlPoint, lastControlPoint.Value, maxDistance);
                totalNumberOfSegments += numberOfSegments;

                lastControlPoint = controlPoint;
            }

            // Calculate discrete points along a Bezier curve.
            var vertices = new Vector3[totalNumberOfSegments + 1];
            for (var i = 0; i <= totalNumberOfSegments; i++)
            {
                var t = i * (1f / totalNumberOfSegments);
                vertices[i] = Bezier.Recursive(controlPoints.ToArray(), t);
            }

            return vertices;
        }

        /// <summary>
        /// Calculates the number of segments needed between two specified points, given a specified maximum segment length.
        /// </summary>
        /// <param name="pointA">
        /// Point A
        /// </param>
        /// <param name="pointB">
        /// Point B
        /// </param>
        /// <param name="maxSegmentLength">
        /// The maximum segment length
        /// </param>
        /// <returns>
        /// The number of segments
        /// </returns>
        public static int CalculateNumberOfSegmentsBetween(Vector3 pointA, Vector3 pointB, float maxSegmentLength)
        {
            var distance = GetDistance(pointA, pointB);
            var numberOfSegments = (int)Math.Ceiling(distance / maxSegmentLength);

            return numberOfSegments;
        }

        /// <summary>
        /// Calculates the total length of a curve along the specified vertices, i.e. the sum of the length of each segment.
        /// </summary>
        /// <param name="vertices">
        /// The vertices
        /// </param>
        /// <returns>
        /// The total length
        /// </returns>
        public static float CalculateTotalLength(Vector3[] vertices)
        {
            Argument.AssertNotNull(vertices, nameof(vertices));

            var totalLength = 0f;
            for (var i = 1; i < vertices.Length; i++)
            {
                totalLength += GetDistance(vertices[i - 1], vertices[i]);
            }

            return totalLength;
        }

        /// <summary>
        /// Gets the distance between the two specified points.
        /// </summary>
        /// <param name="pointA">
        /// Point A
        /// </param>
        /// <param name="pointB">
        /// Point B
        /// </param>
        /// <returns>
        /// The distance between the two points
        /// </returns>
        public static float GetDistance(Vector3 pointA, Vector3 pointB)
        {
            var distance = (pointB - pointA).magnitude;

            return distance;
        }
    }
}
