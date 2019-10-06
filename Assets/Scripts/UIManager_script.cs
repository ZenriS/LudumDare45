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
    private TextMeshProUGUI _placementScoreText;
    private TextMeshProUGUI[] _levelScoreText;
    private TextMeshProUGUI _levelScoreTotalText;
    private EffectMananger_script _effectMananger;
    public GameObject StartScreen;
    private Toggle _musicToggle;
    private Toggle _sfxToggle;
    private TextMeshProUGUI[] _highScoreScreenText;
    private TextMeshProUGUI _highScoreScreenTotalText;
    
    void Awake()
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
        _placementScoreText = ObjectUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _levelScoreText = GameOverScreen.transform.GetChild(3).GetComponentsInChildren<TextMeshProUGUI>();
        _levelScoreTotalText = GameOverScreen.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        _effectMananger = GetComponent<EffectMananger_script>();
        GameOverScreen.SetActive(false);
        _musicToggle = StartScreen.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Toggle>();
        _sfxToggle = StartScreen.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<Toggle>();
        _highScoreScreenText = StartScreen.transform.GetChild(2).GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();
        _highScoreScreenTotalText = StartScreen.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        StartScreen.SetActive(true);
        SetHighScoreScreenText();
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
                //t.text = oi.ObjectName + "\n" + oi.ObjectCount;
                t.text = oi.ObjectName;
                if (oi.ObjectCost > 0)
                {
                    t.text += "\nCost: " + oi.ObjectCost;
                }
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

    public void UpdateObjectCount(ObjectPlacerMananger_script.ObjectInfo oi)
    {
        TextMeshProUGUI t = oi.ObjectButton.GetComponentInChildren<TextMeshProUGUI>();
        //t.text = info.ObjectName + "\n" + info.ObjectCount;
        t.text = oi.ObjectName + "\nCost: " +oi.ObjectCost;
        if (oi.ObjectCount <= 0)
        {
            oi.ObjectButton.interactable = false;
            _objectPreviewer.ClearPreview();
            if (oi.ObjectName == "Spawn Point")
            {
                t.text = oi.ObjectName;
                _startButton.interactable = true;
            }
        }
        else
        {
            oi.ObjectButton.interactable = true;
            if (oi.ObjectName == "Spawn Point")
            {
                t.text = oi.ObjectName;
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

    public void LevelOverUI(bool b, int level)
    {
        if (b)
        {
            _gameoverTitleText.text = "Level Complete";
            _gameoverText.text = "Score: " + _scoreManager.GetScore();
            if (_scoreManager.CheckHighScore(level))
            {
                _gameoverText.text += "\n New High Score";
            }

            _startButton.interactable = false;
        }
        GameOverScreen.SetActive(b);
    }

    public void GameCompleteUI()
    {
        _gameoverTitleText.text = "Level Complete";
        _scoreManager.GetFinalScores(out List<int> levelScores,out int total, out int highScore);
        _gameoverText.text = "Level Scores";
        for (int i = 0; i < levelScores.Count; i++)
        {
            _levelScoreText[i].text = "Level 0" + i + ": " + levelScores[i];
        }
        string s = "Total Score: " + total;
        if (total > highScore)
        {
            s += "\nNew High Score";
        }
        else
        {
            s += "\nHigh Score " + highScore;
        }
        _levelScoreTotalText.text = s;
    }

    public void UpdateScoreUI(int a)
    {
        string s = "Score\n" + a +"\nHigh Score: \n" +_scoreManager.GetLevelHighScore();
        _placementScoreText.text = s;
    }

    public void ToggleSFX()
    {
        bool b = _sfxToggle.isOn;
        _effectMananger.ToggleSfxVolume(b);
    }

    public void ToggleMusic()
    {
        bool b = _musicToggle.isOn;
        _effectMananger.ToggleMusicVolume(b);
    }

    public void SetToggles(bool s, bool m)
    {
        _sfxToggle.isOn = s;
        _musicToggle.isOn = m;
    }

    public void SetHighScoreScreenText()
    {
        for (int i = 0; i < _highScoreScreenText.Length; i++)
        {
            string s = "level:" + i;
            int hs = PlayerPrefs.GetInt(s);
            _highScoreScreenText[i].text = "Level 0" + i + ": " + hs;
        }

        _highScoreScreenTotalText.text = "Run high score\n" + PlayerPrefs.GetInt("FinalHS");
    }
}
