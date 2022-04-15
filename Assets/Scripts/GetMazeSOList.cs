using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetMazeSOList : MonoBehaviour
{
    [SerializeField] private GameObject mazePanelPrefab;
    // Start is called before the first frame update
    void Start()
    { 

        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Mazes/ScriptableObjects" });
       
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var maze_SO = AssetDatabase.LoadAssetAtPath<MazeLevel_ScriptableObject>(SOpath);
            var mazePanel = Instantiate(mazePanelPrefab);
            mazePanel.GetComponent<MazeDisplay>().Initialize(maze_SO);
            mazePanel.transform.SetParent(transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
