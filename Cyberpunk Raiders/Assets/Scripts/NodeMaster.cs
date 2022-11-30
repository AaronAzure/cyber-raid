using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class NodeIntDictionary : SerializableDictionary<int, Node> {}

[System.Serializable]
public class NodeMaster : MonoBehaviour
{
	public List<Node> nodes = new List<Node>();
	public NodeIntDictionary nodeMap;
	private int counter;

	public void CreateNodeMap()
	{
		if (nodes != null) nodes.Clear();
		if (nodeMap != null) nodeMap.Clear();
		counter = 0;

		Node[] childNodes = GetComponentsInChildren<Node>();
		
		foreach (Node node in childNodes)
		{
			nodes.Add(node);
			if (!nodeMap.ContainsValue(node))
				nodeMap.Add(counter++, node);
		}
	}

	public void ClearNodeMap()
	{
		nodes.Clear();
		nodeMap.Clear();
	}
}

[CustomEditor(typeof(NodeMaster))]
public class NodeEditor : Editor
{
	public override void OnInspectorGUI()
	{

		NodeMaster nodeMaster = (NodeMaster) target;

		GUILayout.BeginHorizontal();

			if (GUILayout.Button("Create Node Mapping"))
			{
				nodeMaster.CreateNodeMap();
			}
			if (GUILayout.Button("Clear Node Mapping"))
			{
				nodeMaster.ClearNodeMap();
			}

		GUILayout.EndHorizontal();

		base.OnInspectorGUI();
	}
}
