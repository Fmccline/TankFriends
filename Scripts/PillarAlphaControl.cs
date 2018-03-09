using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarAlphaControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Material material = GetComponent<MeshRenderer>().material;
        Color rendererColor = material.color;
        material.color = new Color(rendererColor.r, rendererColor.g, rendererColor.b, 0.85f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
