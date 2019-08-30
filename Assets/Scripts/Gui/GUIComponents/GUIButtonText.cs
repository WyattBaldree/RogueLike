using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIButtonText : GUIButton
{
    [SerializeField]
    Entry buttonEntry;

    public void SetText(string text)
    {
        buttonEntry.SetText(text);
    }

    public override void UpdateGUI()
    {
        base.UpdateGUI();
        buttonEntry.transform.localPosition = new Vector3(0,0,1);
        buttonEntry.maxSize = (Vector2)buttonSpriteRenderer.bounds.size;
        buttonEntry.horizontalAlignment = HorizontalAlignmentEnum.middle;
        buttonEntry.verticalAlignment = VerticalAlignmentEnum.middle;
        buttonEntry.UpdateGUI();
    }
}
