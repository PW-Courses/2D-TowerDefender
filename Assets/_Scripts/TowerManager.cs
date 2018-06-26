using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
	public static TowerManager instance = null;

	public Sprite[] grassGroudSprites;

	private static GameObject selectedTower; //bez static selectedTower od razu po kliknięciu zmienia się znowu na NULL;

	private static GameObject[] grounds; // tu to samo bez static nie zadziała funkcja na dole ;_;

	private static SpriteRenderer selectedTowerSprite;

	[SerializeField]
	private int TowerPrice;

	void Awake()
	{
		
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy(gameObject);
	
		DontDestroyOnLoad(gameObject);

	}

	void Start()
	{
		selectedTowerSprite = GetComponent<SpriteRenderer> ();

		grounds = GameObject.FindGameObjectsWithTag ("placeableGround");
	}


	void Update()
	{
		if( Input.GetKeyDown (KeyCode.Mouse0) )
		{
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (worldPoint, Vector2.zero);

			if( hit.collider.tag == "placeableGround" && selectedTower )
			{
				PlaceTower (hit);
				hit.collider.tag = "usedGround";
			}
		}

		if( selectedTowerSprite.sprite != null )
		{
			followMouseWithSprite ();
		}

	}

	public void followMouseWithSprite()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		selectedTowerSprite.transform.position = mousePosition;
	}

	public void PlaceTower(RaycastHit2D hit)
	{
		if( selectedTower && !EventSystem.current.IsPointerOverGameObject () )
		{
			GameObject newTower = Instantiate (selectedTower, hit.transform.position, Quaternion.identity) as GameObject;
			selectedTower = null;
			selectedTowerSprite.sprite = null;

			foreach (GameObject ground in grounds)
			{
				ground.GetComponent<SpriteRenderer> ().sprite = grassGroudSprites[0];
			}
		} 
	}
		
	public void GetTowerPressed(GameObject tower)
	{
		
		selectedTower = tower;
		selectedTowerSprite.sprite = tower.gameObject.GetComponent<SpriteRenderer> ().sprite;

		foreach (GameObject ground in grounds)
		{
			if (ground.tag != "usedGround")
			ground.GetComponent<SpriteRenderer> ().sprite = grassGroudSprites[1];
			//ground.GetComponent<SpriteRenderer>().color = new Color32(225,225,225,255);
		}
	}

	public void GetTowerPrice(int price)
	{
		TowerPrice = price;
	}

	public void DisableSelectedTower()
	{
		selectedTower = null;
		selectedTowerSprite.sprite = null;
	}
		
}
