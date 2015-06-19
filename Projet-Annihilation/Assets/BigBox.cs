using UnityEngine;
using System.Collections;

public class BigBox : MonoBehaviour {

	public UserInput userInput;
	
	void OnTriggerEnter(Collider collider)
	{
		userInput.objects.Add(collider.transform.GetComponent<Harvester>());
		userInput.objects.Add (collider.transform.GetComponentInParent<Harvester> ());
		Debug.Log ("coucou");
	}
}
