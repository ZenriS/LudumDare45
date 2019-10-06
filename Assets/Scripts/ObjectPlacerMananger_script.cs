using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectPlacerMananger_script : MonoBehaviour
{
    public Transform ObjectHolder;
    private UIManager_script _uiManager;
    public ObjectPreviewer_script ObjectPreviewer;
    private ScoreManager_script _scoreManager;
    private GameController_script _gameController;
    private EffectMananger_script _effectMananger;

    [Serializable]
    public class ObjectInfo
    {
        public string ObjectName;
        public int ObjectCount;
        public int ObjectStartCount;
        public Button ObjectButton;
        public GameObject ObjectPrefab;
        public int ObjectCost;
    }

    public ObjectInfo[] Objects;
    private ObjectInfo _activeObject;

    private bool _removalTool;
    private Transform _goal;
    private List<GameObject> _placedObjects;
    public AudioClip PlacementClip;
    public AudioClip ButtonClip;

    void Start()
    {
        _uiManager = GetComponent<UIManager_script>();
        _scoreManager = GetComponent<ScoreManager_script>();
        _gameController = GetComponent<GameController_script>();
        _effectMananger = GetComponent<EffectMananger_script>();
        _uiManager.SetUpButtons(Objects);
        _placedObjects = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _activeObject != null && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceObject();
        }

        if (Input.GetButtonDown("Fire1") && _removalTool)
        {
            RemoveObject();
        }
    }

    void PlaceObject()
    {
        if (_activeObject.ObjectCount > 0)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            GameObject go = Instantiate(_activeObject.ObjectPrefab, worldPos, Quaternion.identity, ObjectHolder);
            go.transform.name = _activeObject.ObjectName;
            if (_activeObject.ObjectName == "Spawn Point")
            {
                float dist = Vector2.Distance(go.transform.position, _goal.position);
                _scoreManager.CalcBasScore(dist);
                _activeObject.ObjectCount--;
            }
            else
            {
                _scoreManager.DecreaseScore(_activeObject.ObjectCost);
            }
            _uiManager.UpdateObjectCount(_activeObject);
            _placedObjects.Add(go);
            _effectMananger.PlayEffect(PlacementClip);
        }
        else
        {
          //error sound or something   
        }
    }

    public void SelectObject(int index)
    {
        Debug.Log("Object selected index: " + index);
        _effectMananger.PlayEffect(ButtonClip);
        if (index == 99 || (_activeObject != null && _activeObject.ObjectName == Objects[index].ObjectName))
        {
            _uiManager.UpdateButtons();
            ObjectPreviewer.ClearPreview();
            _activeObject = null;
            return;
        }
        _removalTool = false;
        _activeObject = new ObjectInfo();
        _activeObject = Objects[index];
        _uiManager.UpdateButtons(_activeObject.ObjectButton);
        SpriteRenderer sr = _activeObject.ObjectPrefab.GetComponentInChildren<SpriteRenderer>();
        Vector3 size = sr.transform.localScale;
        ObjectPreviewer.SetPreview(sr.sprite,size);
    }

    public void RemoveObject()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        GameObject go;
        RaycastHit2D hit2D = Physics2D.Raycast(worldPos, Vector3.forward);
        if (hit2D.collider != null)
        {
            go = hit2D.collider.gameObject;
            foreach (ObjectInfo oi in Objects)
            {
                if (oi.ObjectName == go.transform.name)
                {
                    Destroy(go);
                    if (oi.ObjectName == "Spawn Point")
                    {
                        float dist = Vector2.Distance(go.transform.position, _goal.position);
                        _scoreManager.CalcBasScore(dist,false);
                        oi.ObjectCount++;
                    }
                    else
                    {
                        _scoreManager.IncreaseScore(oi.ObjectCost);
                    }
                    _uiManager.UpdateObjectCount(oi);
                    _placedObjects.Remove(go);
                    _effectMananger.PlayEffect(PlacementClip);
                    return;
                }
            }
        }
    }

    public bool ToggleRemovalTool()
    {
        _removalTool = !_removalTool;
        return _removalTool;
    }

    public void RemoveAllObject()
    {
        foreach (GameObject go in _placedObjects)
        {
            if (go == null)
            {
                continue;
            }
            Destroy(go);
        }

        foreach (ObjectInfo oi in Objects)
        {
            oi.ObjectCount = oi.ObjectStartCount;
            oi.ObjectButton.interactable = true;
        }
        _uiManager.UpdateButtons();
        _placedObjects.Clear();
        _activeObject = null;
    }

    public void SetGoal(Transform g)
    {
        _goal = g;
    }

    public void ClearSelectedObject()
    {
        _activeObject = null;
        _uiManager.UpdateButtons();
    }
}
