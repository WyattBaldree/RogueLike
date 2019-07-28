using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveGUI : MonoBehaviour
{
    List<Button> myButtons;
    Interactive targetInteractive;

    public Entry interactiveNameEntry;

    public SpriteRenderer background;

    public Vector2 separation;
    public Vector2 offset;
    public Vector2 dimensions;
    public Vector2 guiMargin;
    public float entryHeight = 0.8f;


    public Button buttonSource;

    private int numButtons = 0;

    public void Initialize()
    {
        
    }

    public void Show(Interactive sourceInteractive)
    {
        if (myButtons != null)
        {
            //destroy the old buttons
            foreach (Button b in myButtons)
            {
                Destroy(b.gameObject);
            }
        }

        //Set our new targetInteractive
        targetInteractive = sourceInteractive;

        //How many buttons do we need?
        numButtons = targetInteractive.myInteractions.Count;
        
        //make a new button list.
        myButtons = new List<Button>();

        
        for (int i = 0; i < numButtons; i++)
        {
            Button newButton = Instantiate<Button>(buttonSource, transform);
            newButton.Initialize();
            newButton.downEvent.AddListener(targetInteractive.myInteractions[i].interactionEvent.Invoke);

            myButtons.Add(newButton);
        }
        Vector2 mousePos = GameController.GetMousePosition();
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        interactiveNameEntry.EntryInitialize(targetInteractive.gameObject.GetComponent<Object>().instanceName);
        UpdateGUIDimensions();

        //load new the buttons from sourceInteractive
        //Keep a tally on our total height
        //update the height of the background texture
        //
    }

    private void UpdateGUIDimensions()
    {
        float height = offset.y;
        for (int i = 0; i < numButtons; i++)
        {
            myButtons[i].transform.localPosition = new Vector3(offset.x, height, transform.position.z);
            myButtons[i].GetSpriteRenderer().size = dimensions;
            height += dimensions.y + separation.y;
        }

        background.transform.localPosition = new Vector3(-guiMargin.x, -guiMargin.y);

        background.size = new Vector2((dimensions.x / background.transform.localScale.x) + (guiMargin.x / background.transform.localScale.x * 2), (height / background.transform.localScale.y) + (guiMargin.y / background.transform.localScale.y * 2) + (entryHeight / background.transform.localScale.y * interactiveNameEntry.GetHeight()));
        interactiveNameEntry.transform.localPosition = new Vector3(0, height + (entryHeight * (interactiveNameEntry.GetHeight() - 1)), interactiveNameEntry.transform.localPosition.z);
    }

    private void OnValidate()
    {
        UpdateGUIDimensions();
    }

    public void Hide()
    {

    }
}
