using UnityEngine;
using System.Collections;
using RTS;

public class GameObjectList : MonoBehaviour {

	public GameObject[] buildings;
	public GameObject[] units;
	public GameObject[] worldObjects;
	public GameObject player;
	public Player actualPlayer;

	private static bool created = false;

	void Awake ()
	{
		if (!created) 
		{
			DontDestroyOnLoad (transform.gameObject);
			RessourceManager.SetGameObjectList (this);
			created = true;
		} 
		else 
		{
			Destroy(this.gameObject);
		}
	}
	

	public GameObject GetBuilding(string name)
	{
		for (int i = 0; i < buildings.Length; i++) 
		{
			Building building = buildings[i].GetComponent< Building >();
			if (building && building.name == name)
			{
				return buildings[i]; //retourne debranchant a eviter, chercher une meilleure maniere de faire
			}
		}
		return null;
	}

	public GameObject GetUnit(string name)
	{
		for (int i = 0; i < units.Length; i++) 
		{
			Unit unit = units[i].GetComponent< Unit >();
			if (unit && unit.name == name)
			{
				return units[i];
			}
		}
		return null;
	}
	
	public GameObject GetWorldObject(string name)
	{
		foreach (GameObject worldObject in worldObjects) 
		{
			if (worldObject.name == name)
			{
				return worldObject;
			}
		}
		return null;
	}
	
	public GameObject GetPlayerObject()
	{
		return player;
	}

	public Texture2D GetBuildImage(string name)
	{
		name = name.Replace (" ", "");
		name = name.Replace ("(Clone)", "");
		for (int i = 0; i < buildings.Length; i++) 
		{
			Building building = buildings[i].GetComponent< Building >();
			if (building && building.name == name)
			{
				return building.buildImage;
			}
		}
		for (int i = 0; i < units.Length; i++) 
		{
			Unit unit = units[i].GetComponent< Unit >();
			if (unit && unit.name == name)
			{
				return unit.buildImage;
			}
		}
		return null;
	}

}
