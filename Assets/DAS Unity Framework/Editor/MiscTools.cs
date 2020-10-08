using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
public class MiscTools
{
    [MenuItem("Misc/How Many Objects Selected?")]
    static void PrintNumberOfObjectsSelected()
    {
        Debug.Log(Selection.objects.Length);
    }

    [MenuItem("Misc/Print Color Code For Selected Images")]
    static void PrintColorCodeForSelectedImages()
    {
        string output = "";
        
        GameObject[] objects = Selection.gameObjects;
        foreach (GameObject gameObject in objects)
        {
            Image image = gameObject.GetComponent<Image>();
            if (image)
            {
                output += "new Color(" + image.color.r + "f, " + image.color.g + "f, " + image.color.b + "f)\n";
            }
        }
        Debug.Log(output);
    }

    [MenuItem("Misc/Append \" PLACEHOLDER\" to selected GameObjects' names")]
    static void AppendToGameObjectNames()
    {
        string stringToAppend = " PLACEHOLDER";

        foreach (GameObject gameObject in Selection.gameObjects)
        {
            gameObject.name = gameObject.name + stringToAppend;
        }
    }
}