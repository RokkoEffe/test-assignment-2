using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggletest : MonoBehaviour
{
    [SerializeField] bool test;
    [SerializeField] Toggle toggle;

    void Update()
    {
        if (test) 
        {

            toggle.isOn = true;

            test = false;

            Debug.Log("selected");
        }
    }
}
