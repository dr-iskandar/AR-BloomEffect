using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;


[Serializable]
public class ProbeLimiter
{
    public GameObject WillBeEffected;
    public ReflectionProbe By;
}

public class ProbeRefLimit : MonoBehaviour {

    /// <summary>
    /// Exclude from probes 
    /// </summary>
    public GameObject[] Limitations;

	// Use this for initialization
	void Start () {
        if (Limitations != null)
        {
            foreach (var item in Limitations)
            {
                var renderers = item.GetComponentsInChildren<Renderer>();
                if (renderers != null)
                {
                    foreach (var renderer in renderers)
                    {
                        //Debug.Log("Renderer: " + renderer.name);
                        renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
                    }
                }
            }
        }
	}
}
