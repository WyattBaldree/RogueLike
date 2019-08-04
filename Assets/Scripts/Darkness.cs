using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour
{
    MapController mapController;
    public SpriteRenderer mySpriteRenderer;
    public Sprite darknessSprite;

    private bool visibility = true;

    private bool initialized = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!initialized) Initalize();
    }

    private void Initalize()
    {
        mapController = GameController.mapC;

        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshSprite()
    {
        if (!initialized) Initalize();

        int touchingCount = 0;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int checkPosX = (int)transform.position.x + i;
                int checkPosY = (int)transform.position.y + j;

                if (checkPosX < 0 ||
                    checkPosY < 0 ||
                    checkPosX >= mapController.wallMapArray.GetLength(0) ||
                    checkPosY >= mapController.wallMapArray.GetLength(1))

                {
                    touchingCount++;
                }
                else
                {
                    bool isPresent = mapController.wallMapArray[checkPosX, checkPosY] != null;
                    if (isPresent)
                    {
                        touchingCount++;
                    }
                }

            }
        }

        if (touchingCount == 9)
        {
            SetVisibility(true);
        }
        else
        {
            SetVisibility(false);
        }
    }

    public void SetVisibility(bool visible)
    {
        visibility = visible;
        mySpriteRenderer.sprite = (visible) ? darknessSprite : null;
    }

    public bool GetVisibility()
    {
        return visibility;
    }
}
