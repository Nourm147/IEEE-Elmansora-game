using System.Collections.Generic;
using UnityEngine;

public class LightGroupManager : MonoBehaviour
{
    [Tooltip("Drag the GameObjects with the LightIntensityController script here.")]
    public List<LightIntensityController> lightControllers;

    // Invokes FadeToTarget on every script in the list simultaneously
    public void FadeAllToTarget()
    {
        foreach (var controller in lightControllers)
        {
            if (controller != null)
            {
                controller.FadeToTarget();
            }
        }
    }

    // Invokes FadeToZero on every script in the list simultaneously
    public void FadeAllToZero()
    {
        foreach (var controller in lightControllers)
        {
            if (controller != null)
            {
                controller.FadeToZero();
            }
        }
    }

}