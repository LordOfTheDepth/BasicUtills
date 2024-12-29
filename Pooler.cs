using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public partial class Pooler : Singleton<Pooler>
{
    
    [SerializeField] private List<PoolParams> _poolElements;
    [SerializeField] private bool _enableCollectionChecks = true;
    private Dictionary<string, ObjectPool<GameObject>> _poolsDict = new Dictionary<string, ObjectPool<GameObject>>();

    private void OnValidate()
    {
        foreach (var pool in _poolElements)
        {
            if(pool.id == "" && pool.prefab != null)
            {
                pool.id = pool.prefab.name;
            }
        }
    }
    private void Start()
    {
        UpdatePools();
    }

    public static bool PoolContainsObject(string name)
    {
        if (instance._poolsDict == null) UpdatePools();
        if (instance._poolsDict.ContainsKey(name))
        {
            return true;
        }
        return false;
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach (var item in FindObjectsOfType<PoolerTag>())
        {
            DestroyImmediate(item.gameObject);
        }
        _poolsDict.Clear();
        Debug.Log("Pool cleared");
    }

    [ContextMenu("Update Pools")]
    public static void UpdatePools()
    {
        instance._poolsDict.Clear();    
        foreach (var poolParams in instance._poolElements)
        {
            var poolId = poolParams.id;

            var createItemFunc = new Func<GameObject>(() =>
            {
                var item = Instantiate(poolParams.prefab, instance.transform);
                item.AddComponent<PoolerTag>().id = poolId;
                item.gameObject.SetActive(false);
                return item;
            });

            var objectPool = new ObjectPool<GameObject>
            (
                createItemFunc,
                instance.OnGet,
                instance.OnRelease,
                collectionCheck: instance._enableCollectionChecks,
                defaultCapacity: poolParams.size
            );
            instance._poolsDict.Add(poolId, objectPool);

            for (int i = 0; i < poolParams.size; i++)
            {
                var newItem = createItemFunc.Invoke();
            }
        }
    }
    void OnRelease(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(null);
    }

    void OnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
        gameObject.transform.SetParent(null);
    }

    public static void Release(GameObject go)
    {
        var tag = go.GetComponent<PoolerTag>();
        if(tag == null) throw new System.Exception("GameObject " + go.name + " doesn't have a pooler tag");
        instance._poolsDict[tag.id].Release(go);
    }

    public static GameObject Take(string id)
    {
        return instance.TakeProtected(id);
    }

    protected virtual GameObject TakeProtected(string id)
    {
        if (_poolsDict == null || _poolsDict.Count == 0) UpdatePools();
        if (!_poolsDict.ContainsKey(id)) throw new System.Exception("No such pool query " + id);

        if (_poolsDict[id].CountInactive == 0)
        {
            Debug.Log("Pool query of " + id + " is empty. Instanciating a new item");
        }
        var item = _poolsDict[id].Get();
        item.transform.SetParent(null);
        item.gameObject.SetActive(true);
        return item;
    }

    [System.Serializable]
    public class PoolParams
    {
        public GameObject prefab;
        public int size;
        public string id;
    }

}
