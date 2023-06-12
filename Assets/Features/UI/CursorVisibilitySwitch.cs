using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Handles cursor visibility state
/// </summary>
public class CursorVisibilitySwitch : MonoBehaviour
{
    [SerializeField] InputActionProperty showCursorAction;

    void OnEnable()
    {
        showCursorAction.action.performed += OnShowCursor;
    }

    void OnDisable()
    {
        showCursorAction.action.performed -= OnShowCursor;
    }

    void OnShowCursor(InputAction.CallbackContext context)
    {
        ShowCursor();
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
