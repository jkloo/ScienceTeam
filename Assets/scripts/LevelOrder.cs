using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelOrder : MonoBehaviour
{
    public static List<string> Order = new List<string>(){"UpAndUp", "MindTheJump", "IntoTheMountain"};

    public static string GetNextLevel(string currLevel)
    {
        int index = Order.IndexOf(currLevel) + 1;
        string nextLevel;
        if(index >= Order.Count)
        {
            nextLevel = "WorldSelect";
        }
        else
        {
            nextLevel = Order[index];
        }
        return nextLevel;
    }
}
