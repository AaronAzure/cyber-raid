using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable] public class NodeIntDictionary : SerializableDictionary<int, Node> {}
[Serializable] public class IntNodeDictionary : SerializableDictionary<Node, int> {}

[System.Serializable]
public class NodeMaster : MonoBehaviour
{
	public List<Node> nodes = new List<Node>();
	public NodeIntDictionary nodeMap;
	public IntNodeDictionary indMap;
	private int counter;

	public void CreateNodeMap()
	{
		if (nodes != null) nodes.Clear();
		if (nodeMap != null) nodeMap.Clear();
		if (indMap != null) indMap.Clear();
		counter = 0;

		Node[] childNodes = GetComponentsInChildren<Node>();
		
		foreach (Node node in childNodes)
		{
			nodes.Add(node);
			if (!indMap.ContainsKey(node))
				indMap.Add(node, counter);
			if (!nodeMap.ContainsValue(node))
				nodeMap.Add(counter++, node);
		}
	}

	public void ClearNodeMap()
	{
		if (nodes != null) nodes.Clear();
		if (nodeMap != null) nodeMap.Clear();
		if (indMap != null) indMap.Clear();
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
