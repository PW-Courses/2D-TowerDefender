using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
	next,
	play,
	gameover,
	win}
;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	[SerializeField]
	private int totalWaves = 10;
	[SerializeField]
	private Text totalMoneyLbl;
	[SerializeField]
	private Text currentWaveLbl;
	[SerializeField]
	private Text totalEscapedLbl;
	[SerializeField]
	private Text playBtnLbl;
	[SerializeField]
	public Button playBtn;

	private bool coroutineStart = false;

	private int waveNumber = 0;
	private int totalMoney = 10;
	private int totalEscaped = 0;
	private int roundEscaped = 0;
	private int totalKilled = 0;
	private int whichEnemiesToSpawn = 0;
	private gameStatus currentState = gameStatus.play;

	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	[SerializeField]
	private GameObject spawnPoint;
	[SerializeField]
	private GameObject[] enemies;
	[SerializeField]
	private int maxEnemiesOnScreen;
	[SerializeField]
	private int totalEnemies = 3;
	[SerializeField]
	private float spawnDelay = 0.5f;
	[SerializeField]
	private int enemiesPerSpawn;

	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


	public List<EnemyBehaviour> enemyList = new List<EnemyBehaviour>();

	public int TotalEscaped {
		get {
			return totalEscaped;
		}
		set {
			totalEscaped = value;
		}
	}

	public int RoundEscaped {
		get {
			return roundEscaped; 
		}
		set {
			roundEscaped = value;
		}
	}

	public int TotalKilled {
		get {
			return totalKilled;
		}
		set {
			totalKilled = value;
		}
	}

	public int TotalMoney {
		get {
			return totalMoney;
		}
		set {
			totalMoney = value;
			totalMoneyLbl.text = totalMoney.ToString ();
		}
	}

	void Awake()
	{
		if( instance == null )
			instance = this;
		else if( instance != null )
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start()
	{
		StartCoroutine (spawnEnemies ());
		playBtn.gameObject.SetActive (false);
		ShowMenu ();
	}
	
	// Update is called once per frame
	void Update()
	{
		handleEscape ();
	}

	IEnumerator spawnEnemies()
	{
		if( enemiesPerSpawn > 0 && enemyList.Count < totalEnemies )
		{
			for (int i = 0; i < enemiesPerSpawn; i++)
			{
				if( enemyList.Count < maxEnemiesOnScreen )
				{
					GameObject newEnemy = Instantiate (enemies[Random.Range (0, 3)], spawnPoint.transform.position, Quaternion.identity) as GameObject;
					//RegisterEnemy(newEnemy.GetComponent<EnemyBehaviour>());
				}
			}
			yield return new WaitForSeconds(spawnDelay);
			StartCoroutine (spawnEnemies ());
		}

	}

	public void RegisterEnemy(EnemyBehaviour enemy)
	{
		enemyList.Add (enemy);
	}

	public void UnregisterEnemy(EnemyBehaviour enemy)
	{
		enemyList.Remove (enemy);
		Destroy (enemy.gameObject);
	}

	public void DestroyAllEnemies()
	{
		foreach (EnemyBehaviour enemy in enemyList)
		{
			Destroy (enemy.gameObject);
		}

		enemyList.Clear ();
	}

	public void AddMoney(int amount)
	{
		TotalMoney += amount;
	}

	public void SubtractMoney(int amount)
	{
		TotalMoney -= amount;
	}

	public void isWaveOver()
	{
		totalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";
		if( (RoundEscaped + TotalKilled) == totalEnemies )
		{
			SetCurrentGameState (); 
			ShowMenu ();
		}
	}

	public void SetCurrentGameState()
	{
		if( TotalEscaped >= 10 )
			currentState = gameStatus.gameover;
		else if( waveNumber == 0 && (TotalKilled + RoundEscaped) == 0 )
			currentState = gameStatus.play;
		else if( waveNumber >= totalWaves )
			currentState = gameStatus.win;
		else
			currentState = gameStatus.next;
	}


	public void ShowMenu()
	{
		switch (currentState)
		{
		case gameStatus.gameover:
			playBtnLbl.text = "Play Again!";
				//add game over sound
			break;
		case gameStatus.next:
			playBtnLbl.text = "Next Wave";
			break;
		case gameStatus.play:
			playBtnLbl.text = "Start Game";
			break;
		case gameStatus.win:
			playBtnLbl.text = "Play";
			break;
		}
		playBtn.gameObject.SetActive (true);
	}

	private void handleEscape()
	{
		if( Input.GetKeyDown (KeyCode.Escape) )
		{
			TowerManager.instance.DisableSelectedTower ();
		}
	}

}

//	public void PlayButtonPressed()
//	{
//		switch (currentState)
//		{
//		case gameStatus.next:
//			waveNumber += 1;
//			totalEnemies += waveNumber;
//			break;
//		default:
//			totalEnemies = 3;
//			totalEscaped = 0;
//			totalMoney = 10;
//			break;
//		}
//
//		DestroyAllObjects();
//		TotalKilled = 0;
//		RoundEscaped = 0;
//		coroutineStart = true;
//	}

	//	public void DestroyAllObjects()
	//	{
	//		GameObject[] towersToDestroy = GameObject.FindGameObjectsWithTag("Tower");
	//
	//		foreach (GameObject tower in towersToDestroy)
	//		{
	//			Destroy(tower);
	//		}
	//	}
	
