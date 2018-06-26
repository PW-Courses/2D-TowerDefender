using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	[SerializeField]
	private GameObject exitPoint;
	[SerializeField]
	private GameObject[] waypoints;
	[SerializeField]
	private float enemySpeed = 10f;
	[SerializeField]
	private int healthPoints;
	[SerializeField]
	private int rewardAmount;

	private float navigationTime = 0;
	private bool isDead = false;
	private int nextWaypoint = 0;
	private Collider2D enemyCollider;
	private Animator anim;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator> ();
		enemyCollider = GetComponent<Collider2D> ();
		GameManager.instance.RegisterEnemy (this);
	}
	
	// Update is called once per frame
	void Update()
	{
		if( waypoints != null && isDead == false )
		{
			navigationTime = enemySpeed * Time.deltaTime;
	
			if( nextWaypoint < waypoints.Length )
			{
				transform.position = Vector2.MoveTowards (transform.position, waypoints[nextWaypoint].transform.position, navigationTime);
			} else
			{
				transform.position = Vector2.MoveTowards (transform.position, exitPoint.transform.position, navigationTime);
			}
		}

		//Debug.Log(navigationTime);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if( collider.tag == "Checkpoint" )
			nextWaypoint++;
		else if( collider.tag == "Exit" )
		{
			GameManager.instance.RoundEscaped += 1;
			GameManager.instance.TotalEscaped += 1;
			GameManager.instance.UnregisterEnemy (this);
			GameManager.instance.isWaveOver();
		} else if( collider.tag == "projectile" )
		{
			Projectile newP = collider.gameObject.GetComponent<Projectile> ();
			EnemyHit (newP.GetAttackStr ());
			Destroy (collider.gameObject);
		}
	}

	public void EnemyHit(int damage)
	{
		if( healthPoints - damage > 0 )
		{
			anim.Play ("Hurt");
			healthPoints -= damage;
		} else
		{
			Die ();
		}
	}

	public void Die()
	{
		anim.SetTrigger ("Die");
		Destroy (enemyCollider);
		isDead = true;
		GameManager.instance.TotalKilled += 1;
		GameManager.instance.AddMoney(rewardAmount);
		GameManager.instance.isWaveOver();


	}

	public bool GetIsDead()
	{
		return isDead;
	}
}
