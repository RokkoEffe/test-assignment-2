using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Moves CharacterController movement on inputs
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputActionProperty moveAction;
    [SerializeField] InputActionProperty lookAction;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] Transform eyes;

    CharacterController characterController;

    Vector2 movementInput;
    Vector2 lookInput;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void OnEnable() => EnableInput();
    void OnDisable() => DisableInput();

    void EnableInput()
    {
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMoveCancelled;
        lookAction.action.performed += OnLook;
        lookAction.action.canceled += OnLookCancelled;
    }

    void DisableInput()
    {
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMoveCancelled;
        lookAction.action.performed -= OnLook;
        lookAction.action.canceled -= OnLookCancelled;

        // Clear inputs
        movementInput = Vector2.zero;
        lookInput = Vector2.zero;
    }

    void Update()
    {
        // Do not handle movement if cursor is visible (user could use UI or something else)
        if (Cursor.visible) return;

        Move();
        Look();
        // Apply gravity
        characterController.SimpleMove(Vector3.zero);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void OnMoveCancelled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }

    void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void OnLookCancelled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    void Move()
    {
        Vector3 movement = transform.forward * movementInput.y + transform.right * movementInput.x;

        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    void Look()
    {
        transform.Rotate(0, lookInput.x * mouseSensitivity, 0);

        float eyesPitch = eyes.transform.localRotation.eulerAngles.x + -1 * lookInput.y * mouseSensitivity;

        // Convert angle from 0..360 to -180..180 range
        eyesPitch = eyesPitch > 180f ? eyesPitch - 360f : eyesPitch;

        // Clamp angle between -90 and 90 to prevent over-rotation
        eyesPitch = Mathf.Clamp(eyesPitch, -90f, 90f);

        eyes.transform.localRotation = Quaternion.Euler(eyesPitch, 0, 0);
    }
}
