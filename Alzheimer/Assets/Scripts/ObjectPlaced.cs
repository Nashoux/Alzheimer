﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlaced : MonoBehaviour {

public 	Vector3 positionAvatar;
public 	Vector3 positionObjet;
[SerializeField] string objectName;
public int numberInDictionnary;
public string zoneName;

public Vector3 position1;
public Vector3 position2;

	// Use this for initialization
	void Start () {
		positionObjet = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
