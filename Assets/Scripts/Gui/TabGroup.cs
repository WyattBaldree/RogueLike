using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public int defaultIndex = 0;

    public Tab[] tabs;
    public PageGroup[] pages;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].index = i;
        }

        if(defaultIndex < tabs.Length)
        {
            TabSelect(defaultIndex);
        }
    }

    public void TabSelect(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (i == index)
            {
                tabs[i].SetSelected(true);
                pages[i].SetActive(true);
            }
            else
            {
                TabUnselect(i);
            }
        }
    }

    public void TabUnselect(int index)
    {
        tabs[index].SetSelected(false);
        pages[index].SetActive(false);
    }

    private void OnValidate()
    {
        Initialize();
    }
}
