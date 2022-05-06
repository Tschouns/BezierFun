using Assets.Scripts.RuntimeChecks;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    /// <summary>
    /// Calculates Bezier curves.
    /// </summary>
    public static class Bezier
    {
        /// <summary>
        /// Calculates a point along a first order Bezier curve (a.k.a. line).
        /// </summary>
        /// <param name="p0">
        /// Control point 0
        /// </param>
        /// <param name="p1">
        /// Control point 1
        /// </param>
        /// <param name="t">
        /// Parameter t
        /// </param>
        /// <returns>
        /// The resulting point on the curve
        /// </returns>
        public static Vector3 Linear(Vector3 p0, Vector3 p1, float t)
        {
            var v = t * (p1 - p0);
            var b = p0 + v;
            
            return b;
        }

        /// <summary>
        /// Calculates a point along a second order Bezier curve.
        /// </summary>
        /// <param name="p0">
        /// Control point 0
        /// </param>
        /// <param name="p1">
        /// Control point 1
        /// </param>
        /// <param name="p2">
        /// Control point 2
        /// </param>
        /// <param name="t">
        /// Parameter t
        /// </param>
        /// <returns>
        /// The resulting point on the curve
        /// </returns>
        public static Vector3 Quadratic(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            var q0 = Linear(p0, p1, t);
            var q1 = Linear(p1, p2, t);

            var b = Linear(q0, q1, t);

            return b;
        }

        /// <summary>
        /// Calculates a point along a third order Bezier curve.
        /// </summary>
        /// <param name="p0">
        /// Control point 0
        /// </param>
        /// <param name="p1">
        /// Control point 1
        /// </param>
        /// <param name="p2">
        /// Control point 2
        /// </param>
        /// <param name="p3">
        /// Control point 3
        /// </param>
        /// <param name="t">
        /// Parameter t
        /// </param>
        /// <returns>
        /// The resulting point on the curve
        /// </returns>
        public static Vector3 Cubic(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            var q0 = Linear(p0, p1, t);
            var q1 = Linear(p1, p2, t);
            var q2 = Linear(p2, p3, t);

            var b = Quadratic(q0, q1, q2, t);

            return b;
        }

        /// <summary>
        /// Calculates a point along a Bezier curve of any order, based on the specified control points.
        /// </summary>
        /// <param name="p">
        /// The control points
        /// </param>
        /// <param name="t">
        /// Parameter t
        /// </param>
        /// <returns>
        /// The resulting point on the curve
        /// </returns>
        public static Vector3 Recursive(Vector3[] p, float t)
        {
            Argument.AssertNotNull(p, nameof(p));

            if (!p.Any())
            {
                return Vector3.zero;
            }

            if (p.Length == 1)
            {
                return p.Single();
            }

            if (p.Length == 2)
            {
                return Linear(p[0], p[1], t);
            }

            // Prepare the control points for the next lower order curve.
            var q = new Vector3[p.Length - 1];

            for (var i = 0; i < q.Length; i++)
            {
                q[i] = Linear(p[i], p[i + 1], t);
            }

            // Call recursively.
            return Recursive(q, t);
        }
    }
}
