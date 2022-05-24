using UnityEngine;

public class WalkInCircle : MonoBehaviour
{
    public float radius = 1f;
    public float speed = 100f;

    private float angle = 0;

    private void FixedUpdate()
    {
        var x = this.radius * Mathf.Cos(this.angle * Mathf.Deg2Rad);
        var y = this.radius * Mathf.Sin(this.angle * Mathf.Deg2Rad);

        this.transform.localRotation = Quaternion.Euler(0, 0 - this.angle, 0);
        this.transform.localPosition = new Vector3(x, this.transform.localPosition.y, y);

        this.angle += this.speed * Time.deltaTime / this.radius;
    }
}