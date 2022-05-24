using Nanolod;
using Nanomesh;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DecimateSlider : MonoBehaviour
{
    public GameObject original;

    public Text polycountLabel;

    private Slider _slider;
    private List<Mesh> _originalMeshes;
    private List<Mesh> _meshes;
    private int _polycount;

    private void Start()
    {
        this._originalMeshes = new List<Mesh>();
        this._meshes = new List<Mesh>();

        this._slider = this.GetComponent<Slider>();
        this._slider.onValueChanged.AddListener(this.OnValueChanged);

        var renderers = this.original.GetComponentsInChildren<Renderer>();

        var uniqueMeshes = new HashSet<Mesh>();

        foreach (var renderer in renderers)
        {
            if (renderer is MeshRenderer meshRenderer)
            {
                var meshFilter = renderer.gameObject.GetComponent<MeshFilter>();
                if (!meshFilter)
                {
                    continue;
                }

                var mesh = meshFilter.sharedMesh;

                this._polycount += mesh.triangles.Length;

                if (!uniqueMeshes.Add(mesh))
                {
                    continue;
                }

                this._originalMeshes.Add(mesh);

                // Clone mesh
                mesh = meshFilter.sharedMesh = UnityConverter.ToSharedMesh(mesh).ToUnityMesh();
                this._meshes.Add(mesh);
            }
            else if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
            {
                var mesh = skinnedMeshRenderer.sharedMesh;

                this._polycount += mesh.triangles.Length;

                if (!uniqueMeshes.Add(mesh))
                {
                    continue;
                }

                this._originalMeshes.Add(mesh);

                // Clone mesh
                mesh = skinnedMeshRenderer.sharedMesh = mesh.ToSharedMesh().ToUnityMesh();
                this._meshes.Add(mesh);
            }
        }

        this.OnValueChanged(1);
    }

    private void OnValueChanged(float value)
    {
        this.polycountLabel.text = $"{Math.Round(100 * value)}% ({Math.Round(value * this._polycount)}/{this._polycount} triangles)";

        Profiling.Start("Convert");

        var connectedMeshes = this._originalMeshes.Select(x => UnityConverter.ToSharedMesh(x).ToConnectedMesh()).ToArray();

        Debug.Log(Profiling.End("Convert"));
        Profiling.Start("Clean");

        foreach (var connectedMesh in connectedMeshes)
        {
            // Important step :
            // We merge positions to increase chances of having correct topology information
            // We merge attributes in order to make interpolation properly operate on every face
            connectedMesh.MergePositions(0.0001f);
            connectedMesh.MergeAttributes();
            connectedMesh.Compact();
        }

        Debug.Log(Profiling.End("Clean"));
        Profiling.Start("Decimate");

        var sceneDecimator = new SceneDecimator();
        sceneDecimator.Initialize(connectedMeshes);

        sceneDecimator.DecimateToRatio(value);

        Debug.Log(Profiling.End("Decimate"));
        Profiling.Start("Convert back");

        for (var i = 0; i < connectedMeshes.Length; i++)
        {
            this._meshes[i].Clear();
            connectedMeshes[i].ToSharedMesh().ToUnityMesh(this._meshes[i]);
            this._meshes[i].bindposes = this._originalMeshes[i].bindposes;
        }

        Debug.Log(Profiling.End("Convert back"));
    }
}