using Assets.Scripts.DataProviders;
using Assets.Scripts.DataObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class GetMazeDBList : MonoBehaviour
{
    [SerializeField] private GameObject mazePanelPrefab;
    // Start is called before the first frame update
    private string preloadMazeOne = $"{{\"mazeId\":13,\"title\":\"RoundUp\",\"creator\":\"Scott\",\"mazeTextureFileName\":\"{Path.Combine(Application.streamingAssetsPath, "PreLoadedMazes", "RoundUp.JPEG").Replace(@"\", @"\\")}\",\"invertToUseBlackLines\":true,\"startPositionRatio\":{{\"x\":0.0925000011920929,\"y\":0.08500000089406967}},\"endPositionRatio\":{{\"x\":0.8949999809265137,\"y\":0.9200000166893005}},\"numberOfPlayThroughs\":0}}";
    private string preloadMazeTwo = $"{{\"mazeId\":7,\"title\":\"Blue Stripe\",\"creator\":\"Scott\",\"mazeTextureFileName\":\"{Path.Combine(Application.streamingAssetsPath, "PreLoadedMazes", "BlueStripe.JPEG").Replace(@"\", @"\\")}\",\"invertToUseBlackLines\":true,\"startPositionRatio\":{{\"x\":0.029999999329447748,\"y\":0.7799999713897705}},\"endPositionRatio\":{{\"x\":0.7749999761581421,\"y\":0.6424999833106995}},\"numberOfPlayThroughs\":0}}";
    private string preloadMazeThree = $"{{\"mazeId\":8,\"title\":\"Circuits\",\"creator\":\"Scott\",\"mazeTextureFileName\":\"{Path.Combine(Application.streamingAssetsPath, "PreLoadedMazes", "Circuits.PNG").Replace(@"\", @"\\")}\",\"invertToUseBlackLines\":true,\"startPositionRatio\":{{\"x\":0.18250000476837159,\"y\":0.057500001043081287}},\"endPositionRatio\":{{\"x\":0.39750000834465029,\"y\":0.7674999833106995}},\"numberOfPlayThroughs\":0}}";
    private string preloadMazeFour = $"{{\"mazeId\":9,\"title\":\"Trapped\",\"creator\":\"Scott\",\"mazeTextureFileName\":\"{Path.Combine(Application.streamingAssetsPath, "PreLoadedMazes", "HandDrawn.JPG").Replace(@"\", @"\\")}\",\"invertToUseBlackLines\":true,\"startPositionRatio\":{{\"x\":0.4025000035762787,\"y\":0.13500000536441804}},\"endPositionRatio\":{{\"x\":0.6474999785423279,\"y\":0.9024999737739563}},\"numberOfPlayThroughs\":0}}";
    private string preloadMazeFive = $"{{\"mazeId\":10,\"title\":\"In Orbit\",\"creator\":\"Scott\",\"mazeTextureFileName\":\"{Path.Combine(Application.streamingAssetsPath, "PreLoadedMazes", "Orbits.JPEG").Replace(@"\", @"\\")}\",\"invertToUseBlackLines\":true,\"startPositionRatio\":{{\"x\":0.042500000447034839,\"y\":0.03500000014901161}},\"endPositionRatio\":{{\"x\":0.16750000417232514,\"y\":0.8725000023841858}},\"numberOfPlayThroughs\":0}}";

    void Start()
    {        
        StartCoroutine(GetAndInstantiateMazePanelsCoroutine());        
    }

    IEnumerator GetAndInstantiateMazePanelsCoroutine(bool use_SO = false)
    {
        var listOfMazesFromDataBase = new ListOfMazesFromDataBase();
        listOfMazesFromDataBase.CreateDataBaseFileIfNotExists();
        listOfMazesFromDataBase.CreateTableForListOfMazesIfNotExists();
        listOfMazesFromDataBase.GetListOfMazesFromDataBase();

        if (MazeList.Mazes.Count == 0)   // If we are starting fresh - lets pop sone pre-loaded mazes in
        {
            Debug.Log($"Streaming assets path: {Application.streamingAssetsPath}");
            listOfMazesFromDataBase.AddMaze(JsonUtility.FromJson<MazeLevel>(preloadMazeOne));
            listOfMazesFromDataBase.AddMaze(JsonUtility.FromJson<MazeLevel>(preloadMazeTwo));
            listOfMazesFromDataBase.AddMaze(JsonUtility.FromJson<MazeLevel>(preloadMazeThree));
            listOfMazesFromDataBase.AddMaze(JsonUtility.FromJson<MazeLevel>(preloadMazeFour));
            listOfMazesFromDataBase.AddMaze(JsonUtility.FromJson<MazeLevel>(preloadMazeFive));
        }
      
        listOfMazesFromDataBase.GetListOfMazesFromDataBase();
                
        foreach (var mazeLevel in MazeList.Mazes)
        {
            var mazePanel = Instantiate(mazePanelPrefab);
            mazePanel.GetComponent<MazeDisplay>().Initialize(mazeLevel);
            mazePanel.transform.SetParent(transform);
            yield return null;
        }
    }
}
