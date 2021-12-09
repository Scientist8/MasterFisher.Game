using System;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{

    void Awake()
    {
        for(int i = 0; i < _fishTypes.Length; i++)
        {
            int num = 0;
            while (num < _fishTypes[i]._fishCount)
            {
                Fish fish = UnityEngine.Object.Instantiate<Fish>(_fishPrefab);
                fish.Type = _fishTypes[i];
                fish.ResetFish();
                num++;
            }
        }
    }

    [SerializeField] private Fish _fishPrefab;
    [SerializeField] private Fish.FishType[] _fishTypes;
}
