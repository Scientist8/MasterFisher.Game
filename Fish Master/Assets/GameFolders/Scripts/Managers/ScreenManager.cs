using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager _instance;

    private GameObject _currentScreen;

    public GameObject _endScreen;
    public GameObject _gameScreen;
    public GameObject _mainScreen;
    public GameObject _returnScreen;

    public Button _lengthButton;
    public Button _strengthButton;
    public Button _offlineButton;

    public Text _gameScreenMoney;
    public Text _lengthCostText;
    public Text _lengthValueText;
    public Text _strengthCostText;
    public Text _strengthValueText;
    public Text _offlineCostText;
    public Text _offlineValueText;
    public Text _endScreenMoney;
    public Text _returnScreenMoney;

    private int _gameCount;

    private void Awake()
    {
        if (ScreenManager._instance)
            Destroy(base.gameObject);
        else
            ScreenManager._instance = this;

        _currentScreen = _mainScreen;
    }

    private void Start()
    {
        CheckIdles();
        UpdateTexts();
    }

    public void ChangeScreen(Screens screen)
    {
        _currentScreen.SetActive(false);
        switch (screen)
        {
            case Screens.MAIN:
                _currentScreen = _mainScreen;
                UpdateTexts();
                CheckIdles();
                break;
            case Screens.GAME:
                _currentScreen = _gameScreen;
                _gameCount++;
                break;
            case Screens.END:
                _currentScreen = _endScreen;
                SetEndScreenMoney();
                break;
            case Screens.RETURN:
                _currentScreen = _returnScreen;
                SetReturnScreenMoney();
                break;
        }
        _currentScreen.SetActive(true);
    }

    public void SetEndScreenMoney()
    {
        _endScreenMoney.text = "$" + IdleManager.instance._totalGain;
    }

    public void SetReturnScreenMoney()
    {
        _returnScreenMoney.text = "$" + IdleManager.instance._totalGain + " gained while waiting!";
    }

    public void UpdateTexts()
    {
        _gameScreenMoney.text = "$" + IdleManager.instance._wallet;
        _lengthCostText.text = "$" + IdleManager.instance._lengthCost;
        _lengthValueText.text = -IdleManager.instance._length + "m";
        _strengthCostText.text = "$" + IdleManager.instance._strengthCost;
        _strengthValueText.text = IdleManager.instance._strength + "fish";
        _offlineCostText.text = "$" + IdleManager.instance._offlineEarningsCost;
        _offlineValueText.text = "$" + IdleManager.instance._offlineEarnings + "/min";
    }

    public void CheckIdles()
    {
        int lengthCost = IdleManager.instance._lengthCost;
        int strengthCost = IdleManager.instance._strengthCost;
        int offlineEarningsCost = IdleManager.instance._offlineEarningsCost;
        int wallet = IdleManager.instance._wallet;

        if (wallet < lengthCost)
            _lengthButton.interactable = false;
        else
            _lengthButton.interactable = true;

        if (wallet < strengthCost)
            _strengthButton.interactable = false;
        else
            _strengthButton.interactable = true;

        if (wallet < offlineEarningsCost)
            _offlineButton.interactable = false;
        else
            _offlineButton.interactable = true;
    }
}
