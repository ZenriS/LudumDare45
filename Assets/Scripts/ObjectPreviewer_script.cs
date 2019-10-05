using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPreviewer_script : MonoBehaviour
{
    private Transform _previewTransform;
    private SpriteRenderer _spriteRendere;
    private bool _follow;

    void Start()
    {
        _previewTransform = transform.GetChild(0);
        _spriteRendere = _previewTransform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_follow)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            this.transform.position = worldPos;
        }
    }

    public void SetPreview(Sprite sprite, Vector3 size)
    {
        _previewTransform.localScale = size;
        _spriteRendere.sprite = sprite;
        _follow = true;
    }

    public void ClearPreview()
    {
        _spriteRendere.sprite = null;
        _follow = false;
    }

}
