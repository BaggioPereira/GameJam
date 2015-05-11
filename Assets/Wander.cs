using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour {

	GameObject[] people;
	public float moveSpeed = 3.0f;

	Vector3[] peopleDirections;
	int peopleCounter = 0;
	int peopleIndex = 0;

	// Use this for initialization
	void Start () {
	
		people = GameObject.FindGameObjectsWithTag ("Person");
		peopleDirections = new Vector3[people.Length];
	}

	void wander(GameObject obj, Vector3 direction, int directionIndex)
	{
		direction.y = 0.0f;

		Vector3 ahead = obj.transform.forward;
		float rayLength = 3.0f;
		RaycastHit hit;
		ahead = ahead.normalized;
		
		if(Physics.Raycast(obj.transform.position, ahead, out hit, rayLength))
		{
			if(hit.transform != obj.transform && hit.transform.gameObject.tag != "Person")
			{	
				//Debug.DrawRay(obj.transform.position, ahead, Color.red, 10);	
				direction += hit.normal * 45;
			}
		}
		
		
		//rotate towards the direction
		obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, Quaternion.LookRotation(direction), 2.0f * Time.deltaTime);
		
		
		//move away from it
		Vector3 moveVector = obj.transform.forward * moveSpeed * Time.deltaTime;
		obj.transform.position += moveVector;

		peopleDirections [directionIndex] = direction;
	}

	// Update is called once per frame
	void Update () {
	
		peopleCounter = 0;
		peopleIndex = 0;

		foreach (GameObject gob in people)
		{
			wander (gob, peopleDirections[peopleCounter], peopleIndex);
			peopleCounter ++;
			peopleIndex ++;

		}
	}
}
