using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public Texture2D sourceTex, sourceTex2;

    public bool rectangleSelect = false;
    public Vector3[] positionList = { };
    public Rect rectangle;
    public int spriteSize = 16;
    public int spriteScale = 1;
    public List<Sprites> mySpriteRenderers;
    private List<Sprite> mySprites;

    // Start is called before the first frame update
    void Awake()
    {
        mySprites = new List<Sprite>();
        GetSprites();
        SetSprite(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Sprites> GetMySpriteRenderers()
    {
        return mySpriteRenderers;
    }

    public void GetSprites()
    {
        if (rectangleSelect)
        {
            for(int i = 0; i < (int)rectangle.width ; i++)
            {
                for (int j = 0; j < (int)rectangle.height; j++)
                {
                    int x = (i + (int)rectangle.x) * spriteSize;
                    int y = (j + (int)rectangle.y) * spriteSize;
                    int width = spriteSize;
                    int height = spriteSize;


                    Sprite newSprite = Sprite.Create(sourceTex, new Rect((float)x, sourceTex.height - spriteSize - (float)y, width, height), new Vector2(0.0f, 0.0f), spriteSize / spriteScale);

                    mySprites.Add(newSprite);
                }
            }
        }
        else
        {
            foreach (Vector3 v in positionList)
            {
                int x = Mathf.FloorToInt(v.x) * spriteSize;
                int y = Mathf.FloorToInt(v.y) * spriteSize;
                int width = Mathf.FloorToInt(spriteSize);
                int height = Mathf.FloorToInt(spriteSize);


                Sprite newSprite = Sprite.Create((v.z == 0) ? sourceTex : sourceTex2, new Rect((float)x, sourceTex.height - spriteSize - (float)y, width, height), new Vector2(0.0f, 0.0f), spriteSize / spriteScale);

                mySprites.Add(newSprite);
            }
        }
    }

    public void SetSprite(int sprIndex)
    {
        mySpriteRenderers[0].SetSprite(mySprites[sprIndex]);
    }

    public void SetSprite(int sprIndex, int sprRendererIndex)
    {
       mySpriteRenderers[sprRendererIndex].SetSprite(mySprites[sprIndex]);
    }

    public void SetSpritePosition(float x, float y, int sprIndex)
    {
        mySpriteRenderers[0].transform.position = new Vector2(x, y);
    }

    public void SetSpritePosition(float x, float y, int sprIndex, int sprRendererIndex)
    {
        mySpriteRenderers[sprRendererIndex].transform.position = new Vector2(x, y);
    }

    public void SetSpriteLocalPosition(float x, float y, int sprIndex)
    {
        mySpriteRenderers[0].transform.localPosition = new Vector2(x, y);
    }

    public void SetSpriteLocalPosition(float x, float y, int sprIndex, int sprRendererIndex)
    {
        mySpriteRenderers[sprRendererIndex].transform.localPosition = new Vector2(x, y);
    }


}

[CustomPreview(typeof(SpriteController))]
public class MyPreview : ObjectPreview
{
    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        SpriteController sc = (SpriteController)target;
        GUI.Label(r, sc.sourceTex);
    }
}

/*[CustomEditor(typeof(Sprites))]
public class MyPlayerEditorAlternative : Editor
{
    private GUIStyle style = new GUIStyle();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SpriteController mp = (SpriteController)target;

        //GUILayout.Label(mp.sourceTex);
        //GUILayout.Label(mp.sourceTex2);

        var texture = AssetPreview.GetAssetPreview(mp.sourceTex);
        GUILayout.Label(texture);
    }
}*/