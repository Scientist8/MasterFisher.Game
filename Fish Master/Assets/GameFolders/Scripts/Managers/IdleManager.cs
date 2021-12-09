using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IdleManager : MonoBehaviour
{
    [HideInInspector]
    public int _length;

    [HideInInspector]
    public int _strength;

    [HideInInspector]
    public int _offlineEarnings;

    [HideInInspector]
    public int _lengthCost;

    [HideInInspector]
    public int _strengthCost;

    [HideInInspector]
    public int _offlineEarningsCost;

    [HideInInspector]
    public int _wallet;

    [HideInInspector]
    public int _totalGain;

    private int[] _costs = new int[]
    {
        120,
        151,
        197,
        250,
        324,
        414,
        537,
        687,
        892,
        1145,
        1484,
        1911,
        2479,
        3196,
        4148,
        5359,
        6954,
        9000,
        11687,
    };

    public static IdleManager instance;

    void Awake()
    {
        if (IdleManager.instance)
            UnityEngine.Object.Destroy(gameObject);
        else
            IdleManager.instance = this;

        _length = -PlayerPrefs.GetInt("Length", 30);
        _strength = PlayerPrefs.GetInt("Strength", 3);
        _offlineEarnings = PlayerPrefs.GetInt("Offline", 3);
        _lengthCost = _costs[-_length / 10 - 3];
        _strengthCost = _costs[_strength - 3];
        _offlineEarningsCost = _costs[_offlineEarnings - 3];
        _wallet = PlayerPrefs.GetInt("Wallet", 0);
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            DateTime now = DateTime.Now;
            PlayerPrefs.SetString("Date", now.ToString());
            //MonoBehaviour.print(now.ToString());
        }
        else
        {
            string @string = PlayerPrefs.GetString("Date", string.Empty);
            if(@string != string.Empty)
            {
                DateTime d = DateTime.Parse(@string);
                _totalGain = (int)((DateTime.Now - d).TotalMinutes * _offlineEarnings + 1.0);
                ScreenManager._instance.ChangeScreen(Screens.RETURN);
            }
        }
    }

    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }

    public void BuyLength()
    {
        _length -= 10;
        _wallet -= _lengthCost;
        _lengthCost = _costs[-_length / 10 - 3];
        PlayerPrefs.SetInt("Length", -_length);
        PlayerPrefs.SetInt("Wallet", _wallet);
        ScreenManager._instance.ChangeScreen(Screens.MAIN);
    }

    public void BuyStrength()
    {
        _strength++;
        _wallet -= _strengthCost;
        _strengthCost = _costs[_strength - 3];
        PlayerPrefs.SetInt("Strength", _strength);
        PlayerPrefs.SetInt("Wallet", _wallet);
        ScreenManager._instance.ChangeScreen(Screens.MAIN);
    }

    public void BuyOfflineEarnings()
    {
        _offlineEarnings++;
        _wallet -= _offlineEarningsCost;
        _offlineEarningsCost = _costs[_offlineEarnings - 3];
        PlayerPrefs.SetInt("Offline", _offlineEarnings);
        PlayerPrefs.SetInt("Wallet", _wallet);
        ScreenManager._instance.ChangeScreen(Screens.MAIN);
    }

    public void CollectMoney()
    {
        _wallet += _totalGain;
        PlayerPrefs.SetInt("Wallet", _wallet);
        ScreenManager._instance.ChangeScreen(Screens.MAIN);
    }

    public void CollectDoubleMoney()
    {
        _wallet += _totalGain * 2;
        PlayerPrefs.SetInt("Wallet", _wallet);
        ScreenManager._instance.ChangeScreen(Screens.MAIN);
    }
}
