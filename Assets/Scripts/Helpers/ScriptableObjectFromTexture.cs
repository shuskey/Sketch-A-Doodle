using System.IO;
using UnityEditor;
using UnityEngine;
public static class ScriptabeObjectFromTexture
{
    [MenuItem("Maze/ScriptableObject From Black Lined Texture")]
    static void CreateScriptableObectBlackLines()
    {
        Texture2D heightmap = Selection.activeObject as Texture2D;
        CreateScriptableObject(heightmap, invertToUseBlackLines: true);
    }

    // Found At https://answers.unity.com/questions/1349349/heightmap-from-texture-script-converter.html
    [MenuItem("Maze/ScriptableObject From White Lined Texture")]
    static void CreateScriptableObjectWhiteLines()
    {
        Texture2D mazeTexture = Selection.activeObject as Texture2D;
        CreateScriptableObject(mazeTexture);
    }

    static void CreateScriptableObject(Texture2D mazeTexture, bool invertToUseBlackLines = false)
    {
        if (mazeTexture == null)
        {
            EditorUtility.DisplayDialog("No texture selected", "Please select a texture.", "Cancel");
            return;
        }

        var nameForNewAsset = mazeTexture.name;
        var saveObj = ScriptableObject.CreateInstance<MazeLevel_ScriptableObject>();

        saveObj.name = nameForNewAsset;
        saveObj.mazeTexture = mazeTexture;
        saveObj.invertToUseBlackLines = invertToUseBlackLines;

        AssetDatabase.CreateAsset(saveObj, $"Assets/Mazes/ScriptableObjects/{nameForNewAsset}.asset");


        //// Now flag the object as "dirty" in the editor so it will be saved
        //EditorUtility.SetDirty(saveObj);

        //// And finally, prompt the editor database to save dirty assets, committing your changes to disk.
        //AssetDatabase.SaveAssets();

    }
}
