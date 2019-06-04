using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveGUI : MonoBehaviour
{
    List<Button> myButtons;
    Interactive targetInteractive;

    public Entry interactiveNameEntry;

    public SpriteRenderer background;

    public Font font;

    public Vector2 separation;
    public Vector2 offset;
    public Vector2 dimensions;

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
            newButton.downEvent.AddListener(targetInteractive.myInteractions[i].interactionEvent.Invoke);

            myButtons.Add(newButton);
        }

        transform.position = GameController.GetMousePosition();
        UpdateButtons();

        //load new the buttons from sourceInteractive
        //Keep a tally on our total height
        //update the height of the background texture
        //

        interactiveNameEntry.EntryInitialize(targetInteractive.name, 100000, font);
    }

    private void UpdateButtons()
    {
        for (int i = 0; i < numButtons; i++)
        {
            myButtons[i].transform.localPosition = new Vector2(offset.x, offset.y + separation.y*i);
        }
    }

    public void Hide()
    {

    }
}
