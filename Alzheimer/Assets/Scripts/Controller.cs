using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour {

	[SerializeField] GameObject avatar;
	NavMeshAgent avatarAgent;

	bool placeToGoIsNext = false;
	bool objectToPlacedIsNext = false;

	List<string> objectIHave;


	PlaceToGo myPlaceToGo;

	// Use this for initialization
	void Start () {
		avatarAgent = avatar.GetComponent<NavMeshAgent>();
		avatarAgent.updateRotation = false;
		avatarAgent.stoppingDistance = 0.1f;
	}

	
	// Update is called once per frame
	void Update () {
		
		 RaycastHit hit;
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)){
				if( hit.collider.GetComponent<PlaceToGo>()){
					myPlaceToGo = hit.collider.GetComponent<PlaceToGo>();
					avatarAgent.SetDestination(myPlaceToGo.positionAvatar);
					placeToGoIsNext = true;
					objectToPlacedIsNext = false;
				}if(hit.collider.GetComponent<ObjectPlaced>()){
					myPlaceToGo = hit.collider.GetComponent<ObjectPlaced>();
					avatarAgent.SetDestination(myPlaceToGo.positionAvatar);
					objectToPlacedIsNext = true;
					placeToGoIsNext = false;
				} else{
					placeToGoIsNext = false;
					objectToPlacedIsNext = false;
					avatarAgent.SetDestination(hit.point);
				}
			}
        }

		if(placeToGoIsNext){
			if (Mathf.Abs(avatar.transform.position.x-avatarAgent.destination.x)<0.11f && Mathf.Abs(avatar.transform.position.z-avatarAgent.destination.z)<0.11f ){
				Debug.Log("give");
				placeToGoIsNext = false;
			}
		}

		if(objectToPlacedIsNext){
			if (Mathf.Abs(avatar.transform.position.x-avatarAgent.destination.x)<0.11f && Mathf.Abs(avatar.transform.position.z-avatarAgent.destination.z)<0.11f ){
				Debug.Log("take");
				//objectIHave.Add (PlaceToGo)
				objectToPlacedIsNext = false;
			}
		}







	}
}