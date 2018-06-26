using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

	[SerializeField]
	private float timeBetweenAttacks;
	[SerializeField]
	private float attackRadius;
	[SerializeField]
	private Projectile projectile;

	private EnemyBehaviour targetEnemy = null;
	private float attackCounter;

	private bool isAttacking = false;


	// Use this for initialization
	void Start()
	{
//		var dir = targetEnemy.transform.position - transform.position;
//		Debug.Log(transform.position);
//		Debug.Log(dir);
			
	}

	void FixedUpdate()
	{
		if( isAttacking )
			Attack ();
	}
	
	// Update is called once per frame
	void Update()
	{
		attackCounter -= Time.deltaTime;
		if( targetEnemy == null || targetEnemy.GetIsDead())
		{
			EnemyBehaviour nearestEnemy = GetNearestEnemyInRange();
			if( nearestEnemy != null && Vector2.Distance(transform.position, nearestEnemy.transform.position) <= attackRadius)
			{
				targetEnemy = GetNearestEnemyInRange ();
			}
		} else {
			if( attackCounter <= 0 ){
				isAttacking = true;
				attackCounter = timeBetweenAttacks;
			} else {
				isAttacking = false;
			}

			if( Vector2.Distance (transform.position, targetEnemy.transform.position) > attackRadius )
				targetEnemy = null;
		}


	}

	public void Attack()
	{
		isAttacking = false;
		Projectile newProjectile = Instantiate (projectile) as Projectile;
		newProjectile.transform.position = transform.position;

		if( targetEnemy == null )
		{
			Destroy (newProjectile);
		} else
		{
			StartCoroutine (MoveProjectile (newProjectile));
		}
	}

	IEnumerator MoveProjectile(Projectile projectile)
	{
		while (GetDistanceToTarget(targetEnemy) > 0.20f && projectile != null && targetEnemy != null)
		{
			var dir = targetEnemy.transform.position - transform.position;
			var angleDirection = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			projectile.transform.rotation = Quaternion.AngleAxis (angleDirection, Vector3.forward);
			projectile.transform.position = Vector2.MoveTowards (projectile.transform.position, targetEnemy.transform.position, 5f * Time.deltaTime);
			yield return null;
		}
		if( projectile != null || targetEnemy == null )
		{
			Destroy (projectile);
		}
	}

	private float GetDistanceToTarget(EnemyBehaviour targetedEnemy)
	{
		if(targetedEnemy == null)
		{
			targetedEnemy = GetNearestEnemyInRange();
			if(targetedEnemy == null)
			{
				return 0f;
			}
		}

		return Mathf.Abs(Vector2.Distance(transform.position, targetedEnemy.transform.position));
	}

	private List<EnemyBehaviour> GetEnemiesInRange()
	{
		List<EnemyBehaviour> enemiesInRange = new List<EnemyBehaviour>();

		foreach (EnemyBehaviour enemy in GameManager.instance.enemyList)
		{
			if( Vector2.Distance (transform.position, enemy.transform.position) <= attackRadius )
				enemiesInRange.Add (enemy);
		}

		return enemiesInRange;
	}

	private EnemyBehaviour GetNearestEnemyInRange()
	{
		EnemyBehaviour nearestEnemy = null;
		float smallestDistance = float.PositiveInfinity;

		foreach (EnemyBehaviour enemy in GetEnemiesInRange())
		{
			if (Vector2.Distance(transform.position, enemy.transform.position) < smallestDistance)
			{
				smallestDistance = Vector2.Distance(transform.position, enemy.transform.position);
				nearestEnemy = enemy;
			}
		}

		return nearestEnemy;
	}
}
