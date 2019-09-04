using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class ContextMenuGUI : GUIPopup
{
    public Window window;
    public Entry titleEntry;

    public float bezelTop;
    public float bezelBottom;
    public float bezelSides;

    [SerializeField]
    private GUIButtonText contextButtonSource;

    private List<GUIButtonText> contextButtons = new List<GUIButtonText>();

    float GUIWidth = 5;

    public float contextButtonSeparation = .5f;
    public float titleSeparation = .5f;

    public float buttonHeight = 1f;
    public float buttonSeparation = .1f;

    public Interactive myInteractive;

    public override Vector2 GetDimensions()
    {
        Assert.IsNotNull(window);
        return window.GetDimensions();
    }

    public override void Align()
    {
        // base the GUI off of the child inventory GUI so update it first.

        titleEntry.maxSize.x = GUIWidth - (bezelSides * 2);
        titleEntry.transform.position = transform.position + new Vector3(bezelSides, -bezelTop, -2);
        titleEntry.horizontalAlignment = HorizontalAlignmentEnum.middle;
        titleEntry.verticalAlignment = VerticalAlignmentEnum.top;
        titleEntry.UpdateGUI();

        float totalButtonHeight = 0;
        foreach (GUIButtonText button in contextButtons)
        {
            button.transform.position = transform.position + new Vector3(bezelSides, -bezelTop - titleEntry.GetDimensions().y - titleSeparation - totalButtonHeight, -2);
            button.buttonSpriteRenderer.transform.localScale = new Vector3(GUIWidth - (bezelSides * 2), buttonHeight, 1);
            totalButtonHeight += buttonHeight + buttonSeparation;
            button.UpdateGUI();
        }
        totalButtonHeight -= buttonSeparation;

        maxSize = new Vector2(GUIWidth, bezelTop + titleEntry.GetDimensions().y + titleSeparation + totalButtonHeight + bezelBottom);

        window.transform.position = transform.position + new Vector3(0, 0, -1);
        window.maxSize = maxSize;
        window.UpdateGUI();

        myCollider.size = GetDimensions();
        myCollider.offset = new Vector2((myCollider.size.x / 2), -(myCollider.size.y / 2));
    }

    public override void UpdateGUI()
    {
        Align();
        if (!Application.isPlaying) EditorUtility.SetDirty(this);
    }

    public void LoadButtons(List<Interaction> interactionsList)
    {
        foreach(GUIButtonText button in contextButtons)
        {
            Destroy(button.gameObject);
        }
        contextButtons.Clear();

        foreach(Interaction interaction in interactionsList)
        {
            GUIButtonText newButton = Instantiate<GUIButtonText>(contextButtonSource,this.transform);
            newButton.gameObject.name = "Button " + interaction.name;
            newButton.SetText(interaction.name);
            newButton.pressEvent.AddListener(Hide);
            newButton.pressEvent.AddListener(interaction.myAction);
            contextButtons.Add(newButton);
        }
    }

    public void Popup(Vector2 targetPosition, Interactive targetInteractive)
    {
        titleEntry.SetText("<d>" + targetInteractive.GetInteractiveName());
        LoadButtons(targetInteractive.GetInteractions());
        Popup(targetPosition, false);
    }

    public override void CustomOnMouseClickedOutside()
    {
        Hide();
    }
}