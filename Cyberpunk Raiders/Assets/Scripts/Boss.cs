using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
	[SerializeField] int maxHp;
	[SerializeField] int hp;
	[SerializeField] int totalCooldown;
	[SerializeField] int cooldown;

	// Start is called before the first frame update
	void Start()
	{
		
	}
}
