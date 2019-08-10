using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    public Entry entryClass = null;

    //List of strings that represents the log history.
    List<Entry> entryList;

    public int logHeightMax = 10;
    private int logHeight = 0;
    public float textWidth = 1;

    public float logWidth = 50;
    public float messageMargin = 1;

    public float entryScale = 1;

    public Font font;

    Entry newEntry;
    public void Initialize()
    {
        entryList = new List<Entry>();


        NewEntry("<d>0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0");
        NewEntry("<d> 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0");
        NewEntry("<d>The basalisk bites you for 15 damage! Its venom poisons you.");
        NewEntry("<d>You slash the basalisk for 15 damage!");
        NewEntry("<d><anim.wave>You drink a crimson potion...");
        NewEntry("<d>You gain 30 health!");
        NewEntry("<d>hey <size.0.58><color.blue>BLUE <size.2><color.white>white");
        NewEntry("<d>NORMAL <bold>BOLD<bold> <italic>ITALIC<italic> <bold><italic>BOLDITALIC<italic><bold>");
        NewEntry("<d><bold><color.yellow><anim.bounce>LEVEL UP!");
        NewEntry("0");
        

    }

    /// <summary>
    /// Create a new entry in the log.
    /// </summary>
    /// <param name="str">What the entry should say.</param>
    public void NewEntry(string str)
    {
        newEntry = Instantiate(entryClass, this.transform);
        newEntry.transform.localScale = new Vector3(entryScale, entryScale, newEntry.transform.localScale.z);
        newEntry.maxSize.y = 10;
        newEntry.SetText(str, logWidth, font);
        entryList.Insert(0, newEntry);
        logHeight += newEntry.GetHeight();

        updateEntryPositions();
    }

    /// <summary>
    /// Update the entry positions in the log.
    /// </summary>
    private void updateEntryPositions()
    {
        float height = 0;
        foreach (Entry e in entryList)
        {
            height += e.GetPixelHeight() + messageMargin;
            
            e.transform.localPosition = new Vector3(0, height, 0);
        }
        Prune();
    }

    /// <summary>
    /// Remove entries until we have less than logHeightMax lines.
    /// </summary>
    private void Prune()
    {
        if(logHeight > logHeightMax)
        {
            Entry prunedEntry = entryList.ToArray()[entryList.Count-1];
            entryList.RemoveAt(entryList.Count-1);
            logHeight -= prunedEntry.GetHeight();

            Destroy(prunedEntry.gameObject);
            Prune();
        }
    }
}