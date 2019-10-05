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

    [Serializable]
    public class ObjectInfo
    {
        public string ObjectName;
        public int ObjectCount;
        public Button ObjectButton;
        public GameObject ObjectPrefab;
        public int ObjectCost;
    }

    public ObjectInfo[] Objects;
    private ObjectInfo _activeObject;

    private bool _removalTool;

    void Start()
    {
        _uiManager = GetComponent<UIManager_script>();
        _scoreManager = GetComponent<ScoreManager_script>();
        _uiManager.SetUpButtons(Objects);
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
            _activeObject.ObjectCount--;
            go.transform.name = _activeObject.ObjectName;
            _uiManager.UpdateObjectCount(_activeObject);
            _scoreManager.DecreaseScore(_activeObject.ObjectCost);
        }
        else
        {
          //error sound or something   
        }
    }

    public void SelectObject(int index)
    {
        Debug.Log("Objects index: " + index);
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
                    oi.ObjectCount++;
                    _uiManager.UpdateObjectCount(oi);
                    _scoreManager.IncreaseScore(oi.ObjectCost);
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
}
