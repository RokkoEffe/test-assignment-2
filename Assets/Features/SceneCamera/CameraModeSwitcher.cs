using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Switches between different camera modes: Walking/Scene Overview
/// </summary>
public class CameraModeSwitcher : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputActionProperty switchModeAction;

    [Header("References")]
    [SerializeField] CharacterMovement characterMovement;
    [SerializeField] SceneCamera characterCamera;

    public UnityEvent WalkingModeEnabled;
    public UnityEvent SceneOverviewEnabled;

    void OnEnable()
    {
        switchModeAction.action.performed += OnModeSwitched;
    }

    void OnDisable()
    {
        switchModeAction.action.performed -= OnModeSwitched;
    }

    void OnModeSwitched(InputAction.CallbackContext context)
    {
        // Switch between two enum states
        SceneCamera.CameraMode newMode = characterCamera.cameraMode == SceneCamera.CameraMode.Walking 
            ? SceneCamera.CameraMode.SceneOverview 
            : SceneCamera.CameraMode.Walking;;

        // Enable new mode
        switch (newMode)
        {
            case SceneCamera.CameraMode.Walking:
                EnterWalkingMode();
                break;

            case SceneCamera.CameraMode.SceneOverview:
                EnterSceneOverviewMode();
                break;

            default:
                break;
        }
    }

    public void EnterSceneOverviewMode()
    {
        characterCamera.cameraMode = SceneCamera.CameraMode.SceneOverview;
        characterMovement.enabled = false;
        SceneOverviewEnabled?.Invoke();
    }

    public void EnterWalkingMode()
    {
        characterCamera.cameraMode = SceneCamera.CameraMode.Walking;
        characterMovement.enabled = true;
        WalkingModeEnabled?.Invoke();
    }
}
