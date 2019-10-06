using UnityEngine;
using System.Collections;

public class StringHelper
{
    /// <summary>
    /// Capitalize the first 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string CapitalizeFirst(string s)
    {
        // Check for empty string.  
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '<')
            {
                while (i < s.Length && s[i] != '>')
                {
                    i++;
                }
            }
            else if (s[i] >= 65 && s[i] <= 90 || s[i] >= 97 && s[i] <= 122)
            {
                return s.Substring(0, i) + char.ToUpper(s[i]) + s.Substring((i + 1 > s.Length - 1) ? s.Length - 1 : i + 1, s.Length - 1 - i);
            }
        }

        // Return char and concat substring.  
        return s;
    }
}
