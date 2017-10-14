using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] //Comment this line out to disable line-render updating in the editor and save a bit of performance.
public class SimpleRopeLineRenderPositioning : MonoBehaviour {

    public LineRenderer LineRenderer;
    public GameObject LineRenderStartTarget;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        LineRenderer.SetPosition(0, transform.position);
        LineRenderer.SetPosition(1, LineRenderStartTarget.transform.position);
    }
}
