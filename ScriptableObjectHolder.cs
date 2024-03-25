using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectHolder : MonoBehaviour
{
    public List<ScriptableObject> scriptableObjects;

    private void Awake()
    {
        Resources.LoadAll<ScriptableObject>("");
    }
}