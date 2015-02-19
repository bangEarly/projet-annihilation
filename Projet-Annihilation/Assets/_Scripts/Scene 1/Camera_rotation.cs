using UnityEngine;
using System.Collections;

public class Camera_rotation : MonoBehaviour {

	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(new Vector3(0.1f, 0.4f, 0.0f) * Time.deltaTime);
	}
}
