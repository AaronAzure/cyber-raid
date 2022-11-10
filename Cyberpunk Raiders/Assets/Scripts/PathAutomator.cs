using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PathAutomator : MonoBehaviour
{
    public Node[] nodes;


	// Start is called before the first frame update
	void Start()
	{
		nodes = GetComponentsInChildren<Node>();

		// EACH PATH WILL LEAD TO THE NEXT NODE, 
		// EXCEPT THE END WILL EITHER LEAD TO A DIFFERENT NODE(S) IN A DIFFERENT PATH (JOIN / SPLIT)
		for (int i=0 ; i<nodes.Length - 1 ; i++) 
		{
			if (nodes[i].nodes.Count == 0) 
				nodes[i].nodes = new List<Node>();
			nodes[i].nodes.Add(nodes[i + 1]);
			//  = nodes[i + 1].gameObject;
		}

		// AUTOMATICALLY CREATE ARROW POINTING TO NEXT NODE
		// if (arrows.Length > 0) {
		// 	for (int i=0 ; i<nodes.Length ; i++) 
		// 	{
		// 		for (int j=0 ; j<nodes[i].nexts.Length ; j++) 
		// 		{
		// 			if (nodes[i].nexts.Length <= 0 || nodes[i].nexts[j].dontCreateArrow || nodes[i].nexts[0].node == null) 
		// 				continue;
		// 		}
		// 	}
		// }
	}

	public void HOW_MANY_NODES()
	{
		Debug.Log(transform.childCount);
	}
}


[CustomEditor(typeof(PathAutomator))]
[CanEditMultipleObjects]
public class PathAutomatorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PathAutomator myScript = (PathAutomator)target;
		if(GUILayout.Button("Log Path Length"))
		{
			myScript.HOW_MANY_NODES();
		}
	}
}