using UnityEngine;

/// <summary>
/// Represents a control point.
/// </summary>
public class ControlPoint : MonoBehaviour
{
    /// <summary>
    /// Gets the control point's position.
    /// </summary>
    public Vector3 Position => this.transform.position;

    /// <summary>
    /// Removes the control point object from the scene.
    /// </summary>
    public void Remove()
    {
        DestroyImmediate(this.gameObject);
    }
}
