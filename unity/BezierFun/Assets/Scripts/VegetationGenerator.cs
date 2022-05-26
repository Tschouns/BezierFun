using Assets.Scripts.RuntimeChecks;
using System;
using UnityEngine;

/// <summary>
/// Spawns in a bunch of grass objects on plane. Assumes that it is attached to a plane, with normal = global Y axis.
/// </summary>
public class VegetationGenerator : MonoBehaviour
{
    [InspectorName("Grass Object")]
    public GameObject grassObject;

    [InspectorName("Patch Size")]
    public float patchSize = 100f;

    [InspectorName("Distance in Between Objects")]
    public float distanceInBetweenObjects = 1f;

    private void Awake()
    {
        Field.AssertNotNull(this.grassObject, nameof(this.grassObject));

        if (distanceInBetweenObjects <= 0f)
        {
            throw new InvalidOperationException("The \"distance in between objects\" must be greater than 0.");
        }

        var numberOfObjects = (int)Math.Ceiling(this.patchSize / this.distanceInBetweenObjects);
        var positions = new Vector3[numberOfObjects, numberOfObjects];
        var offset = this.patchSize / 2;

        UnityEngine.Random.InitState(37);
        var d = this.distanceInBetweenObjects / 2;

        for (var i = 0; i < numberOfObjects; i++)
        {
            for (var j = 0; j < numberOfObjects; j++)
            {
                var xRand = UnityEngine.Random.Range(-d, d);
                var zRand = UnityEngine.Random.Range(-d, d);

                positions[i, j] = new Vector3(
                    this.transform.position.x + (i * this.distanceInBetweenObjects) - offset + xRand,
                    this.transform.position.y,
                    this.transform.position.z + (j * this.distanceInBetweenObjects) - offset + zRand);
            }
        }

        foreach (var position in positions)
        {
            Instantiate(this.grassObject, position, Quaternion.identity);
        }
    }
}

