using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

    [System.Serializable]
public class Entry : GUIComponent
{
    public CharacterContainer CharacterContainerClass = null;

    public string text;
    List<CharacterContainer> containers;
    Font font;
    
    [SerializeField]
    public List<GameObject> lines;

    public GameObject emptyGameObjectSource;

    public Color color = Color.white;
    public float fontSize = 0.5f;
    public bool bold = false;
    public bool italic = false;
    public CharacterContainer.AnimationStateEnum anim = CharacterContainer.AnimationStateEnum.none;

    //public float widthAdjust = .001f;
    public float heightAdjust = .01f;

    public float textBoxWidth;

    float defaultFontSize;

    public void SetText(string str, float tbw = -1, Font entryFont = null)
    {
        //Set it's text
        text = str;

        //set the width of the text box
        if (tbw != -1) maxSize.x = tbw;// textBoxWidth = tbw;

        if(entryFont != null) font = entryFont;

        defaultFontSize = fontSize;

        DestroyLines();
        DestroyContainers();

        //Create the containers that will hold our characters.
        MakeContainers();
    }

    ///////////////////////////////////////////////////////////////Animations
    /// <summary>
    /// How long it takes a character to complete 1 wave loop.
    /// </summary>
    public float waveDuration = 0.1f;
    /// <summary>
    /// The time delta between adjacent characters.
    /// </summary>
    public float waveCharacterSpacing = 0.6f;

    /// <summary>
    /// How long it takes a character to complete 1 rainbow loop.
    /// </summary>
    public float rainbowDuration = 0.1f;

    /// <summary>
    /// The time delta between adjacent characters.
    /// </summary>
    public float rainbowCharacterSpacing = 0.6f;

    public float bounceWaitTime = 0.1f;



    public enum animationEnum { };

    /// <summary>
    /// Make all of our child character containers
    /// </summary>
    /// <param name="CharacterContainerClass"></param>
    /// <returns></returns>
    void MakeContainers()
    {
        DestroyContainers();


        containers = new List<CharacterContainer>();

        char[] cArray = text.ToCharArray();

        List<CharacterContainer> newList = new List<CharacterContainer>();

        for (int i = 0; i < cArray.Length; i++)
        {
            if (cArray[i] == '<')
            {
                //<d> example <d> resets the formatting to default
                if (text.Substring(i + 1, "d".Length) == "d")
                {
                    if (GameController.gameC && GameController.gameC.debug) Debug.Log("default");

                    i += "d".Length + 1;

                    color = Color.white;
                    fontSize = defaultFontSize;
                    italic = false;
                    bold = false;
                    anim = CharacterContainer.AnimationStateEnum.none;
                }
                //<color.[colorname]> example: <color.blue>
                //Change the font color
                else if (text.Substring(i + 1, "color".Length) == "color")
                {
                    if (GameController.gameC && GameController.gameC.debug) Debug.Log("color");

                    i += "color".Length + 1;

                    int j;
                    for (j = 0; j < text.Length; j++)
                    {
                        if (cArray[i + j] == '>')
                        {
                            break;
                        }
                    }

                    string str = text.Substring(i + 1, j - 1);
                    i += j;
                    Debug.Log(str);

                    switch (str)
                    {
                        case "white":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("white");

                            color = Color.white;
                            break;
                        case "blue":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("blue");

                            color = Color.blue;
                            break;
                        case "red":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("red");

                            color = Color.red;
                            break;
                        case "yellow":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("yellow");

                            color = Color.yellow;
                            break;
                        case "green":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("green");

                            color = Color.green;
                            break;
                        case "purple":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("purple");

                            color = Color.magenta;
                            break;
                        case "tan":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("tan");

                            color = new Color(212, 214, 185);
                            break;
                    }
                }
                //<size.[fontscale]> example <size.0.58> (about 0.4-0.6 is advised)
                //changes font size.
                else if (text.Substring(i + 1, "size".Length) == "size")
                {
                    if (GameController.gameC && GameController.gameC.debug) Debug.Log("size");

                    i += "size".Length + 1;

                    int j;
                    for (j = 0; j < text.Length; j++)
                    {
                        if (cArray[i + j] == '>')
                        {
                            break;
                        }
                    }

                    string str = text.Substring(i + 1, j - 1);
                    i += j;
                    Debug.Log(str);

                    fontSize = Convert.ToSingle(str);
                }
                //<bold>
                //toggles bolding
                else if (text.Substring(i + 1, "bold".Length) == "bold")
                {
                    if (GameController.gameC && GameController.gameC.debug) Debug.Log("bold");

                    i += "bold".Length + 1;
                    bold = !bold;
                }
                //<italic>
                //toggles italics
                else if (text.Substring(i + 1, "italic".Length) == "italic")
                {
                    if (GameController.gameC && GameController.gameC.debug) Debug.Log("italic");

                    i += "italic".Length + 1;
                    italic = !italic;
                }
                //<anim.[aimationName]> example <anim.wave>
                //set the current animation.
                else if (text.Substring(i + 1, "anim".Length) == "anim")
                {
                    if (GameController.gameC && GameController.gameC.debug) Debug.Log("anim");

                    i += "anim".Length + 1;

                    int j;
                    for (j = 0; j < text.Length; j++)
                    {
                        if (cArray[i + j] == '>')
                        {
                            break;
                        }
                    }

                    string str = text.Substring(i + 1, j - 1);
                    i += j;
                    Debug.Log(str);

                    switch (str)
                    {
                        case "none":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("none");
                            anim = CharacterContainer.AnimationStateEnum.none;
                            break;
                        case "wave":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("wave");
                            anim = CharacterContainer.AnimationStateEnum.wave;
                            break;
                        case "bounce":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("bounce");
                            anim = CharacterContainer.AnimationStateEnum.bounce;
                            break;
                        case "rainbow":
                            if (GameController.gameC && GameController.gameC.debug) Debug.Log("rainbow");
                            anim = CharacterContainer.AnimationStateEnum.rainbow;
                            break;
                    }
                }
                continue;
            }



            CharacterContainer newChar = Instantiate<CharacterContainer>(CharacterContainerClass);
            TextMeshPro tm = newChar.GetComponentInChildren<TextMeshPro>();
            tm.text = "" + cArray[i];
            tm.color = color;
            tm.fontSize = fontSize;
            

            if (anim == CharacterContainer.AnimationStateEnum.none)
            {
                newChar.EndAnimation();
                newChar.StartAnimation(CharacterContainer.AnimationStateEnum.none, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
            }
            else if (anim == CharacterContainer.AnimationStateEnum.wave)
            {
                newChar.StartAnimation(CharacterContainer.AnimationStateEnum.wave, (waveCharacterSpacing - 1 - (i % waveCharacterSpacing)) / (waveCharacterSpacing), 0.0f, waveCharacterSpacing / waveDuration, 0.0f, 3f, 1.0f);
            }
            else if (anim == CharacterContainer.AnimationStateEnum.bounce)
            {
                newChar.StartAnimation(CharacterContainer.AnimationStateEnum.bounce, 0.0f, bounceWaitTime, 1.6f, 0.0f, .09f, 1.0f);
            }
            else if (anim == CharacterContainer.AnimationStateEnum.rainbow)
            {
                newChar.StartAnimation(CharacterContainer.AnimationStateEnum.rainbow, (rainbowCharacterSpacing - 1 - (i % rainbowCharacterSpacing)) / (rainbowCharacterSpacing), 0.0f, rainbowCharacterSpacing / rainbowDuration, 1.0f, 0.8f, 0.8f);
            }

            if (bold)
            {
                if (italic)
                {
                    tm.fontStyle = FontStyles.Bold | FontStyles.Italic;
                }
                else
                {
                    tm.fontStyle = FontStyles.Bold;
                }

            }
            else if (italic)
            {
                tm.fontStyle = FontStyles.Italic;
            }
            else
            {
                tm.fontStyle = FontStyles.Normal;
            }

            newChar.anim = anim;

            newList.Add(newChar);
            tm.ForceMeshUpdate();
        }

        containers = newList;
        AddCharacterContainersToLines();
    }

    /// <summary>
    /// This function loops through all of the text containers created by the MakeContainers function positions them. Each text container is parented to an empty game object.
    /// When the text containers overflow the maxSize.x, a new empty game object (a line) is made below and the text wraps to the new game object. 
    /// </summary>
    void AddCharacterContainersToLines()
    {
        //Destroy all of the current lines which contain CharacterContainers
        DestroyLines();
        //Create an empty list of lines.
        lines = new List<GameObject>();

        //Create the first line, and immediately set its local scale to 1 to avoid resizing issues.
        GameObject currentLine = Instantiate<GameObject>(emptyGameObjectSource, this.transform);
        currentLine.name = "Line" + lines.Count;
        currentLine.transform.localScale = new Vector3(1, 1, 1);

        //Add our newly created line to the empty lines list
        lines.Add(currentLine);

        // width is the width of out current line in unity units. Each time a character is added, it's char width is added to this value
        // We use this value to place each character to the right of the last.
        float width = 0;
        
        // this value holds the index of the last "wrapable" character (usually a space) in the line. We try to break at this index to avoid cutting words in half.
        int wrapIndex = 0;

        // this value holds the total width of the characters we have added since the last "wrapable" character. If a word is longer than textbox width, we have to cut it when we wrap.
        float wordWidth = 0;

        for (int i = 0; i < containers.Count; i++)
        {
            // The container at our current i index
            CharacterContainer currentContainer = containers[i];
            // The child text mesh of our current character.
            TextMeshPro tm = (TextMeshPro)currentContainer.GetComponentInChildren<TextMeshPro>();

            // Parent our currentContainer to our currentLine
            currentContainer.transform.SetParent(currentLine.transform, true);
            // The following line is required to prevent the current container from inheriting the scale of the parent of the currentLine. All of our scaling is coming from resizing the Entry.
            currentContainer.transform.localScale = new Vector3(1, 1, 1);

            // Add half the width of the first character to the width and word width so the characters start
            // just to the right of the Entry transform (as opposed to in the center of it.
            if(width == 0)
            {
                //width += currentContainer.GetWidth() * transform.lossyScale.x / 2;
                //wordWidth += currentContainer.GetWidth() * transform.lossyScale.x / 2;
            }

            // Set the position of our currentCharacter "width distance" to the right of the currentline's position
            currentContainer.transform.position = currentLine.transform.position + new Vector3(width, 0, currentContainer.transform.localPosition.z);

            // Get the width of our current character.
            float charWidth = currentContainer.GetWidth();

            // Get the char of our current character.
            char c = tm.text.ToCharArray()[0];

            if (c == ' ')
            {
                // If this is a wrap character, make it our new wrap index and reset word width
                wrapIndex = i;
                wordWidth = 0;
            }
            else
            {
                // Otherwise continue to add to our current wordWidth
                wordWidth += charWidth * transform.lossyScale.x;
            }

            // If a single character is ever larger than our text box width, something is horribly wrong.
            if (charWidth * transform.lossyScale.x > maxSize.x)
            {
                Debug.Log("Text box is smaller than a single character!!!!!!!");
                break;
            }

            // Increase our width by the size of our character
            width += charWidth * transform.lossyScale.x;

            // When our width becomes larger than our maxSize.x, we need to line break
            if (width > maxSize.x)
            {
                //If there is at least one ' ' on the line, wrap there, else, wrap at the last character
                // This should prevent infinite loops from happening when a single word is longer than a line.
                if(wordWidth != width)
                {
                    i = wrapIndex;
                }
                else
                {
                    i--;
                }
                
                // Since we are wrapping, we need to create a new line object.
                currentLine = Instantiate<GameObject>(emptyGameObjectSource, this.transform);
                currentLine.name = "Line" + lines.Count;
                // The following line is required to prevent the current container from inheriting the scale of the parent of the currentLine. All of our scaling is coming from resizing the Entry.
                currentLine.transform.localScale = new Vector3(1, 1, 1);
                lines.Add(currentLine);

                // since we are wrapping, reset width and word width.
                width = 0;
                wordWidth = 0;
                continue;
            }
        }

        Align();
    }

    /// <summary>
    /// Return the width of the line supplied.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    float GetLineWidth(GameObject line)
    {
        CharacterContainer[] childContainers = line.GetComponentsInChildren<CharacterContainer>();
        float w = 0;
        foreach(CharacterContainer tc in childContainers)
        {
            w += tc.GetWidth() * transform.lossyScale.x;
        }
        return w;
    }

    /// <summary>
    /// Align each line in lines based on the alignment enum.
    /// </summary>
    public override void Align()
    {
        // Vertical Alignment
        float yAlign;
        if (verticalAlignment == VerticalAlignmentEnum.top)
        {
            yAlign = 0;
        }
        else if (verticalAlignment == VerticalAlignmentEnum.middle)
        {
            yAlign = (Mathf.Max(minSize.y, maxSize.y) / 2) - (GetPixelHeight() / 2);
        }
        else
        {
            yAlign = Mathf.Max(minSize.y, maxSize.y) - (GetPixelHeight());
        }

        for ( int i = 0; i < lines.Count; i++)
        {
            // Horizontal Alignment
            float xAlign;
            if (horizontalAlignment == HorizontalAlignmentEnum.left)
            {
                xAlign = 0;
            }
            else if (horizontalAlignment == HorizontalAlignmentEnum.middle)
            {
                xAlign = (Mathf.Max(minSize.x, maxSize.x) / 2) - (GetLineWidth(lines[i]) / 2);
            }
            else
            {
                xAlign = Mathf.Max(minSize.x, maxSize.x) - (GetLineWidth(lines[i]));
            }

            lines[i].transform.position = transform.position + new Vector3(xAlign, ((i+1) * heightAdjust * transform.lossyScale.y * -1) - yAlign, lines[i].transform.localPosition.z);
        }
    }

    void DestroyLines()
    {
        if (lines == null) return;
        foreach (GameObject o in lines)
        {
            if(o) DestroyImmediate(o.gameObject);
        }
        lines.Clear();
    }

    void DestroyContainers()
    {
        if (containers == null) return;
        foreach (CharacterContainer tc in containers)
        {
            if(tc) DestroyImmediate(tc.gameObject);
        }
        containers = null;
    }

    // Return the height of the message in lines.
    public int GetHeight()
    {
        if (lines == null) return 0;
        return lines.Count;
    }

    // Return the height of the message in pixels.
    public float GetPixelHeight()
    {
        if (lines == null) return 0;
        return lines.Count * heightAdjust * transform.lossyScale.y;
    }

    public override Vector2 GetDimensions()
    {
        return new Vector2(Mathf.Max(minSize.x, maxSize.x), Mathf.Max(minSize.y, heightAdjust * transform.lossyScale.y, GetPixelHeight()));
    }

    public override void UpdateGUI()
    {
        SetText(text);
        if (!Application.isPlaying) EditorUtility.SetDirty(this);
    }
}
