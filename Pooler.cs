using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public partial class Pooler : Singleton<Pooler>
{
    private Dictionary<string, PoolableItemPool<PoolableItem>> _poolsDict;
    [SerializeField] private List<PoolParams> _poolElements;
    [SerializeField] private bool _enableCollectionChecks = true;

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
        while (instance.transform.childCount > 0)
        {
            DestroyImmediate(instance.transform.GetChild(0).gameObject);
        }
        _poolsDict = null;
        Debug.Log("Pool cleared");
    }

    [ContextMenu("Update Pools")]
    public static void UpdatePools()
    {
        instance._poolsDict = new Dictionary<string, PoolableItemPool<PoolableItem>>();
        foreach (var poolParams in instance._poolElements)
        {
            var poolId = poolParams.poolableItem.PoolID;
            var createItemFunc = new Func<PoolableItem>(() =>
            {
                var item = Instantiate(poolParams.poolableItem, instance.transform);
                instance._poolsDict[poolParams.poolableItem.PoolID].Release(item);
                item.gameObject.SetActive(false);
                return item;
            });

            var objectPool = new PoolableItemPool<PoolableItem>
            (
                poolParams,
                createItemFunc,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                instance._enableCollectionChecks,
                poolParams.size
            );
            instance._poolsDict.Add(poolId, objectPool);

            for (int i = 0; i < poolParams.size; i++)
            {
                var newItem = createItemFunc.Invoke();
            }
        }
    }

    public static void Put(GameObject go)
    {
        Put(go.GetComponent<PoolableItem>());
    }

    public static void Put(PoolableItem poolableItem)
    {
        var id = poolableItem.PoolID;
        poolableItem.transform.SetParent(instance.transform);
        poolableItem.transform.localPosition = Vector3.zero;
        poolableItem.gameObject.SetActive(false);

        if (instance._poolsDict != null)
        {
            var poolQueue = instance._poolsDict[id];
            poolQueue.Release(poolableItem);
        }
        else
        {
            DestroyImmediate(poolableItem);
        }
    }

    public static PoolableItem Take(PoolableItem poolableItem, Vector3 position)
    {
        return instance.TakeProtected(poolableItem.PoolID, position);
    }

    public static T Take<T>(string name)
    {
        return Take(name).GetComponent<T>();
    }

    public static PoolableItem Take(string name, Vector3 position)
    {
        return instance.TakeProtected(name, position);
    }

    public static PoolableItem Take(string name)
    {
        return instance.TakeProtected(name, Vector3.zero);
    }

    protected virtual PoolableItem TakeProtected(string id, Vector3 position)
    {
        if (_poolsDict == null) UpdatePools();
        if (!_poolsDict.ContainsKey(id)) throw new System.Exception("No such pool query " + id);

        if (_poolsDict[id].CountInactive == 0)
        {
            Debug.Log("Pool query of " + id + " is empty. Instanciating a new item");
        }
        var item = _poolsDict[id].Get();
        item.transform.SetParent(null);
        item.transform.position = position;
        item.gameObject.SetActive(true);
        return item;
    }

    private static void OnTakeFromPool(PoolableItem poolableItem)
    {
    }

    private static void OnReturnedToPool(PoolableItem poolableItem)
    {
    }

    private static void OnDestroyPoolObject(PoolableItem poolableItem)
    {
    }

    [System.Serializable]
    public class PoolParams
    {
        public PoolableItem poolableItem;
        public int size;
    }

    public class PoolableItemPool<T> : ObjectPool<T> where T : PoolableItem
    {
        public PoolParams PoolParams { get; protected set; }

        public PoolableItemPool(PoolParams poolParams, Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, Action<T> actionOnDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
            : base(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize)
        {
            this.PoolParams = poolParams;
        }
    }
}