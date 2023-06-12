using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows to change Toggle state without notifying
/// </summary>
[RequireComponent(typeof(Toggle))]
public class ToggleSwitch : MonoBehaviour
{
    Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void Check()
    {
        toggle.SetIsOnWithoutNotify(true);
    }

    public void Uncheck()
    {
        toggle.SetIsOnWithoutNotify(false);
    }
}
