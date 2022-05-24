using Assets.Scripts.RuntimeChecks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        this.mainCamera = Camera.main;
        
        Field.AssertNotNull(this.mainCamera, nameof(this.mainCamera));
    }

    // Update is called once per frame
    void Update()
    {
        var previousRotation = this.transform.rotation;
        this.transform.LookAt(mainCamera.transform.position);
        this.transform.rotation = Quaternion.Euler(
            previousRotation.eulerAngles.x,
            this.transform.eulerAngles.y,
            previousRotation.eulerAngles.z);
    }
}
