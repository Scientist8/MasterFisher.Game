using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hook : MonoBehaviour
{
    public Transform _hookedTransform;

    private Camera _mainCamera;
    private Collider2D _coll;

    private int _length;
    private int _strength;
    private int _fishCount;

    private bool _canMove;

    private List<Fish> _hookedFish;

    private Tweener _cameraTween;

    void Awake()
    {
        _mainCamera = Camera.main;
        _coll = GetComponent<Collider2D>();
        _hookedFish = new List<Fish>();
    }
    

    void Update()
    {
        if (_canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;
        }
    }

    public void StartFishing()
    {
        _length = IdleManager.instance._length - 20;
        _strength = IdleManager.instance._strength;
        _fishCount = 0;
        float time = (-_length) * 0.1f;

        _cameraTween = _mainCamera.transform.DOMoveY(_length, 1 + time * 0.25f, false).OnUpdate(delegate
        {
            if (_mainCamera.transform.position.y <= -11)
                transform.SetParent(_mainCamera.transform);
        }).OnComplete(delegate
        {
            _coll.enabled = true;
            _cameraTween = _mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
            {
                if (_mainCamera.transform.position.y >= -25f)
                    StopFishing();
            });
        });

        ScreenManager._instance.ChangeScreen(Screens.GAME);
        _coll.enabled = false;
        _canMove = true;
        _hookedFish.Clear();
    }

    void StopFishing()
    {
        _canMove = false;
        _cameraTween.Kill(false);
        _cameraTween = _mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (_mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            _coll.enabled = true;
            int num = 0;
            for (int i = 0; i < _hookedFish.Count; i++)
            {
                _hookedFish[i].transform.SetParent(null);
                _hookedFish[i].ResetFish();
                num += _hookedFish[i].Type._price;
            }
            IdleManager.instance._totalGain = num;
            ScreenManager._instance.ChangeScreen(Screens.END);
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Fish") && _fishCount != _strength)
        {
            _fishCount++;
            Fish component = other.GetComponent<Fish>();
            component.Hooked();
            _hookedFish.Add(component);
            other.transform.SetParent(transform);
            other.transform.position = _hookedTransform.position;
            other.transform.rotation = _hookedTransform.rotation;
            other.transform.localScale = Vector3.one;

            other.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                other.transform.rotation = Quaternion.identity;
            });
            if (_fishCount == _strength)
                StopFishing();
        }
    }
}
