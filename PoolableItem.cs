using System;
using UnityEngine;
using UnityEngine.Events;

public class PoolableItem : MonoBehaviour
{
    public string PoolID;
    [SerializeField] public UnityEvent OnTakenFromPoolEvent;

    public Action OnTakenFromPoolAction;
}