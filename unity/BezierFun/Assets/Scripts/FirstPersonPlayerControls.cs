using Assets.Scripts.RuntimeChecks;
using UnityEngine;

/// <summary>
/// Processes player input and controls the <see cref="CharacterController"/> and first person <see cref="Camera"/>.
/// </summary>
public class FirstPersonPlayerControls : MonoBehaviour
{
    [InspectorName("Player Body")]
    public Transform playerBody;

    [InspectorName("First Person Camera")]
    public Camera firstPersonCamera;

    [InspectorName("Movement Speed")]
    public float movementSpeed = 10f;

    [InspectorName("Mouse Sensitivity")]
    public float mouseSensitivity = 1f;

    private CharacterController characterController;
    private float verticalCameraRotation = 0f;

    private void Awake()
    {
        Field.AssertNotNull(this.firstPersonCamera, nameof(this.firstPersonCamera));
        Field.AssertNotNull(this.mouseSensitivity, nameof(this.mouseSensitivity));

        this.characterController = this.GetComponent<CharacterController>();
        this.verticalCameraRotation = this.firstPersonCamera.transform.localEulerAngles.x;

        Field.AssertNotNull(this.characterController, nameof(this.characterController));
    }

    private void Update()
    {
        var mouseX = Input.GetAxis("Mouse X") * this.mouseSensitivity * 1000 * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * this.mouseSensitivity * 1000 * Time.deltaTime;

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        // Rotate player body horizontally.
        this.playerBody.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically.
        this.verticalCameraRotation = Mathf.Clamp(this.verticalCameraRotation - mouseY, -90f, 90f);
        this.firstPersonCamera.transform.localRotation = Quaternion.Euler(this.verticalCameraRotation, 0, 0);

        // Move character controller.
        var move = (this.playerBody.transform.right * x) + (this.playerBody.transform.forward * z);
        this.characterController.Move(move * this.movementSpeed * Time.deltaTime);
    }
}
