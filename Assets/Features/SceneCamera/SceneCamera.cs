using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls camera movement depending on mode (Walking/Scene Overview)
/// </summary>
public class SceneCamera : MonoBehaviour
{
    [SerializeField] public CameraMode cameraMode = CameraMode.Walking;

    [Header("Character Eyes Mode")]
    [SerializeField] Transform characterEyes;

    [Header("Scene Overview Mode")]
    // Inputs
    [SerializeField] InputActionProperty rotateCameraAction;
    [SerializeField] InputActionProperty zoomAction;

    // Scroll settings
    [SerializeField] float scrollSensitivity = 0.02f;
    [SerializeField] float minZoomDistance = 8f;
    [SerializeField] float maxZoomDistance = 40f;

    // Rotate settings
    [SerializeField] float sensitivity = 0.2f;
    [SerializeField] float minYAngle = -5f;
    [SerializeField] float maxYAngle = 85f;

    float currentX = 45f;
    float currentY = 45f;

    float currentZoomDistance = 8f;
    Vector2 currentRotateInput;

    public enum CameraMode
    {
        Walking,
        SceneOverview
    }

    void OnEnable() => EnableInput();
    void OnDisable() => DisableInput();

    public void EnableInput()
    {
        zoomAction.action.performed += OnZoom;
        rotateCameraAction.action.performed += OnRotateCamera;
        rotateCameraAction.action.canceled += OnCancelledRotateCamera;
    }

    public void DisableInput()
    {
        zoomAction.action.performed -= OnZoom;
        rotateCameraAction.action.performed -= OnRotateCamera;
        rotateCameraAction.action.canceled -= OnCancelledRotateCamera;

        // Clear inputs
        currentRotateInput = Vector2.zero;
    }

    void Start()
    {
        currentZoomDistance = maxZoomDistance;
    }

    void OnZoom(InputAction.CallbackContext context)
    {
        // Do not handle inputs when cursor is visible
        if (Cursor.visible) return;

        currentZoomDistance -= context.ReadValue<float>() * scrollSensitivity;
        currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);
    }

    void OnRotateCamera(InputAction.CallbackContext context)
    {
        // Do not handle inputs when cursor is visible
        if (Cursor.visible) return;

        currentRotateInput = rotateCameraAction.action.ReadValue<Vector2>();
    }

    void OnCancelledRotateCamera(InputAction.CallbackContext context)
    {
        currentRotateInput = Vector2.zero;
    }

    void RotateAroundScene()
    {
        currentX += currentRotateInput.x * sensitivity;
        currentY -= currentRotateInput.y * sensitivity;

        // Clamp the vertical angle between the minimum and maximum values
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        Vector3 direction = new Vector3(0, 0, -currentZoomDistance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        transform.position = rotation * direction;
        transform.LookAt(Vector3.zero);
    }

    void MoveToWalkingPosition()
    {
        transform.position = characterEyes.transform.position;
        transform.rotation = characterEyes.transform.rotation;
    }

    private void LateUpdate()
    {
        switch (cameraMode)
        {
            case CameraMode.Walking:
                MoveToWalkingPosition();
                break;
            case CameraMode.SceneOverview:
                RotateAroundScene();
                break;
            default:
                break;
        }
    }
}
