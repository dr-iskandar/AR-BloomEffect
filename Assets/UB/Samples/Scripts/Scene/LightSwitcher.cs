using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LightMatHolder
{
    public bool IsOpen = false;
    public MeshRenderer MeshRenderer;
    public Material NormalMaterial;
    public Material LightMaterial;
    public Light[] Lights;
}

public class LightSwitcher : MonoBehaviour {

    public LightMatHolder[] LightMats;

    public void ChangeLightStates()
    {
        foreach (var item in LightMats) {
            if (item.IsOpen)
            {
                item.IsOpen = false;
                item.MeshRenderer.material = item.NormalMaterial;
                foreach (var light in item.Lights)
                {
                    light.gameObject.SetActive(false);
                }
            } else
            {
                item.IsOpen = true;
                item.MeshRenderer.material = item.LightMaterial;
                foreach (var light in item.Lights)
                {
                    light.gameObject.SetActive(true);
                }
            }
        }
    }
}
