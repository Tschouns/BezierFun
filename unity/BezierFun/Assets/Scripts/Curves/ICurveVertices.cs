using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    /// <summary>
    /// Represents a curve as a bunch of vertices.
    /// </summary>
    public interface ICurveVertices
    {
        /// <summary>
        /// Gets the curve vertices.
        /// </summary>
        IReadOnlyList<Vector3> Vertices { get; }
    }
}
