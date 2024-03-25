using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlicker : MonoBehaviour
{
    [SerializeField] private bool _startActive;

    public bool Flickering
    {
        get
        {
            return _flickeing;
        }

        set
        {
            _flickeing = value;
            if (value == false)
            {
                _target.color = _defaultColor;
            }
        }
    }

    private SpriteRenderer _target;
    public Color darkColor;
    public Color lightColor;
    private Color _defaultColor;
    private bool _flickeing;
    public float speed;

    private void Awake()
    {
        _target = GetComponent<SpriteRenderer>();
        _defaultColor = _target.color;
    }

    private void Start()
    {
        if (_startActive)
        {
            Flickering = true;
        }
    }

    private void FixedUpdate()
    {
        if (_flickeing)
        {
            var sin = Mathf.Abs(Mathf.Sin(Time.time * speed));
            _target.color = Color.Lerp(darkColor, lightColor, sin);
        }
    }
}