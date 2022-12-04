using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	public enum NodeType 
	{
		normal=0,
		poison=1
	}

	public bool isSpecialSpace;
	public bool hasData;
	public Boss boss;
	public List<Node> nodes;
	[SerializeField] NodeType nodeType;


	// Start is called before the first frame update
	void Start()
	{
		
	}

	/// <summary>
	/// Returns true if player movement should not be decremented;
	/// </summary>
	public bool OnTraverse()
	{
		return (boss != null || isSpecialSpace);
	}


	public int GetNodeEffect()
	{
		switch (nodeType)
		{
			case NodeType.normal:
				return 5;
			case NodeType.poison:
				return -5;
			default:
				return 5;
		}
	}


	// Update is called once per frame
	// void Update()
	// {
	// 
	// }
}
