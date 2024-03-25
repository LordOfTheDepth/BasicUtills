using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    protected static T _i;

    public static T instance
    {
        get
        {
            if (_i == null)
            {
                var objects = Resources.FindObjectsOfTypeAll<T>();
                if (objects.Length == 0)
                {
                    throw new System.Exception("Can not find instance");
                }
                if (objects.Length > 1)
                {
                    throw new System.Exception("Multiple instances located");
                }
                _i = objects[0];
            }
            return _i;
        }
    }
}