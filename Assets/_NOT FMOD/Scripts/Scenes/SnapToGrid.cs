using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour {
    public float GridSize = 0.25f;
	
	// Update is called once per frame
	void Update () {
		if (GridSize > 0.0f)
        {
            float invGrid = 1.0f / GridSize;

            float x = Mathf.Round(transform.position.x * invGrid) / invGrid;
            float y = Mathf.Round(transform.position.y * invGrid) / invGrid;


            transform.position = new Vector3(x, y, transform.position.z);
        }
	}
}
