using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class WorldGeneration
{
    [MenuItem("Sacrizism/WorldCreation/CreateGround")]
    public static void CreateGround()
    {
        GameObject.FindObjectOfType<WorldManager>().CreateGround();
    }
}
#endif