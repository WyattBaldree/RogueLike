using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [System.NonSerialized]
    public Sprite mySprite;

    private bool initalized = false;

    private void Awake()
    {
        if (!initalized) Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!initalized) Initialize();
    }

    private void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initalized = true;
    }

    public void SetSprite(Sprite s)
    {
        if (!initalized) Initialize();
        mySprite = s;
        spriteRenderer.sprite = mySprite;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }
}


