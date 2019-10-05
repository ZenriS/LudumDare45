using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_script : MonoBehaviour
{
    public GameObject ObjectUI;
    private List<Image> _buttonImages;
    private Button _activeButton;
    private ObjectPlacerMananger_script _objectPlacerMananger;
    private ObjectPreviewer_script _objectPreviewer;
    private Button _removalToolButton;
    private Image _removalToolButtonImage;
    private Button _startButton;
    private GameController_script _gameControllerScript;
    public GameObject GameOverScreen;
    private TextMeshProUGUI _gameoverTitleText;
    private TextMeshProUGUI _gameoverText;
    private ScoreManager_script _scoreManager;

    void Start()
    {
        _objectPlacerMananger = GetComponent<ObjectPlacerMananger_script>();
        _objectPreviewer = _objectPlacerMananger.ObjectPreviewer;
        _removalToolButton = ObjectUI.transform.GetChild(1).GetChild(4).GetComponent<Button>();
        _removalToolButtonImage = _removalToolButton.GetComponent<Image>();
        _startButton = ObjectUI.transform.GetChild(1).GetChild(5).GetComponent<Button>();
        _gameControllerScript = GetComponent<GameController_script>();
        _gameoverTitleText = GameOverScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _gameoverText = GameOverScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _scoreManager = GetComponent<ScoreManager_script>();
        GameOverScreen.SetActive(false);
    }

    public void SetUpButtons(ObjectPlacerMananger_script.ObjectInfo[] objects)
    {
        _buttonImages = new List<Image>();
        foreach (ObjectPlacerMananger_script.ObjectInfo oi in objects)
        {
            if (oi.ObjectCount > 0)
            {
                oi.ObjectButton.gameObject.SetActive(true);
                int index = oi.ObjectButton.transform.GetSiblingIndex();
                oi.ObjectButton.onClick.AddListener(() => _objectPlacerMananger.SelectObject(index));
                TextMeshProUGUI t = oi.ObjectButton.GetComponentInChildren<TextMeshProUGUI>();
                t.text = oi.ObjectName + "\n" + oi.ObjectCount;
                Image i = oi.ObjectButton.GetComponent<Image>();
                _buttonImages.Add(i);
            }
            else
            {
                oi.ObjectButton.gameObject.SetActive(false);
            }
        }
        _removalToolButton.onClick.AddListener(ToggleRemovalButton);
        _startButton.onClick.AddListener(_gameControllerScript.PlacementDone);
        _startButton.interactable = false;
        UpdateButtons();
    }

    public void UpdateObjectCount(ObjectPlacerMananger_script.ObjectInfo info)
    {
        TextMeshProUGUI t = info.ObjectButton.GetComponentInChildren<TextMeshProUGUI>();
        t.text = info.ObjectName + "\n" + info.ObjectCount;
        if (info.ObjectCount <= 0)
        {
            info.ObjectButton.interactable = false;
            _objectPreviewer.ClearPreview();
            if (info.ObjectName == "Spawn Point")
            {
                _startButton.interactable = true;
            }
        }
        else
        {
            info.ObjectButton.interactable = true;
            if (info.ObjectName == "Spawn Point")
            {
                _startButton.interactable = false;
            }
        }
    }

    public void UpdateButtons(Button b = null)
    {
        foreach (Image i in _buttonImages)
        {
            i.color = Color.white;
        }

        _removalToolButtonImage.color = Color.white;
        if (b != null)
        {
            b.GetComponentInChildren<Image>().color = Color.gray;
        }
    }

    public void TogglePlacementUI(bool b)
    {
        ObjectUI.SetActive(b);
        _objectPreviewer.ClearPreview();
    }

    public void ToggleRemovalButton()
    {
        if (_objectPlacerMananger.ToggleRemovalTool())
        {
            _objectPlacerMananger.SelectObject(99);
            UpdateButtons(_removalToolButton);
        }
        else
        {
            UpdateButtons();
        }
    }

    public void GameOverIU(bool b)
    {
        if (b)
        {
            _gameoverTitleText.text = "Level Complete";
            _gameoverText.text = "Score: " +_scoreManager.GetScore();
        }
        else
        {
            _gameoverTitleText.text = "Game Over";
            _gameoverText.text = "";
        }
        GameOverScreen.SetActive(true);
    }
}
