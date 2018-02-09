using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour {

	[SerializeField] GameObject avatar;
	NavMeshAgent avatarAgent;

	// Use this for initialization
	void Start () {
		avatarAgent = avatar.GetComponent<NavMeshAgent>();
		avatarAgent.updateRotation = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		 RaycastHit hit;
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                avatarAgent.SetDestination(hit.point);
        }

	}
}