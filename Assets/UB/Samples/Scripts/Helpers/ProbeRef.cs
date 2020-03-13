using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class ProbeRef : MonoBehaviour {
    
    ReflectionProbe _probe;

    void Awake() {
        _probe = GetComponent<ReflectionProbe>();
    }

    void Update () {
        _probe.transform.position = new Vector3(
            Camera.main.transform.position.x, 
            Camera.main.transform.position.y * -1, 
            Camera.main.transform.position.z
        );

        _probe.RenderProbe();
    }
}
