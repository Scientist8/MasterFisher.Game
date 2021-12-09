using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Fish : MonoBehaviour
{
    private Fish.FishType _fishType;

    private CircleCollider2D _collider;

    private SpriteRenderer _renderer;

    private float _screenLeft;

    private Tweener _tweener;

    public Fish.FishType Type
    {
        get
        {
            return _fishType;
        }
        set
        {
            _fishType = value;
            _collider.radius = _fishType._colliderRadius;
            _renderer.sprite = _fishType._sprite;
        }
    }
  

    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }


    public void ResetFish()
    {
        if (_tweener != null)
            _tweener.Kill(false);

        float num = UnityEngine.Random.Range(_fishType._minLength, _fishType._maxLength);
        _collider.enabled = true;

        Vector3 position = transform.position;
        position.y = num;
        position.x = _screenLeft;
        transform.position = position;

        float num2 = 1;
        float y = UnityEngine.Random.Range(num - num2, num + num2);
        Vector2 v = new Vector2(-position.x, y);

        float num3 = 3;
        float delay = UnityEngine.Random.Range(0, 2 * num3);
        _tweener = transform.DOMove(v, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        });
    }

    public void Hooked()
    {
        _collider.enabled = false;
        _tweener.Kill(false);
    }


    [Serializable]
    public class FishType
    {
        public int _price;

        public float _fishCount;

        public float _minLength;

        public float _maxLength;

        public float _colliderRadius;

        public Sprite _sprite;
    }
}
