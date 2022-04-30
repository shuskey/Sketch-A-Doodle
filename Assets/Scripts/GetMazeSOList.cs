using Assets.Scripts.DataProviders;
using Assets.Scripts.DataObjects;
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
        var listOfMazesFromDataBase = new ListOfMazesFromDataBase();

        //string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Mazes" });

        //foreach (string spritename in assetNames)
        //{
        //    var spritepath = AssetDatabase.GUIDToAssetPath(spritename);
        //    var mysprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritepath);
        //    if (mysprite != null)
        //    {
        //        var newMazeLevel = new MazeLevel(
        //           mazeId: 0,
        //           title: mysprite.name,
        //           creator: "unknown",
        //           mazeTextureFileName: spritepath,
        //           invertToUseBlackLines: true,
        //           startPositionRatio: new Vector2(0f,0f),
        //           endPositionRatio: new Vector2(1f,1f),
        //           createdDate: System.DateTime.Now,
        //           numberOfPlayThroughs: 0
        //            );

        //        listOfMazesFromDataBase.AddMaze(newMazeLevel);
        //    }
        //}

        StartCoroutine(GetAndInstantiateMazePanelsCoroutine());        
    }

    IEnumerator GetAndInstantiateMazePanelsCoroutine(bool use_SO = false)
    {
        var listOfMazesFromDataBase = new ListOfMazesFromDataBase();
        listOfMazesFromDataBase.GetListOfMazesFromDataBase();

        foreach(var mazeLevel in listOfMazesFromDataBase.mazeList)
        {
            var mazePanel = Instantiate(mazePanelPrefab);
            mazePanel.GetComponent<MazeDisplay>().Initialize(mazeLevel);
            mazePanel.transform.SetParent(transform);
            yield return null;
        }
    }
}
