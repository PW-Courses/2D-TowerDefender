using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum projType { rock, arrow, fireball };

public class Projectile : MonoBehaviour {

	[SerializeField]
	private int attackStrength;
	[SerializeField]
	private projType projectileType;

	public int GetAttackStr(){
		return attackStrength;
	}

	public projType GetProjectileType()
	{
		return projectileType;
	}
}
