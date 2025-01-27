using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lection3Controller : MonoBehaviour
{
    [SerializeField]
    private int newInt = 0;
    [SerializeField]
    private string newString = "Hello World!";

    private List<int> intList = new List<int>();
    private List<string> stringList = new List<string>();

    //INT LIST
    [ContextMenu("AddIntToList")]
    private void AddIntToList()
    {
        intList.Add(newInt);
        Debug.Log($"Int {newInt} added to list.");
    }
    [ContextMenu("RemoveIntFromList")]
    private void RemoveIntFromList()
    {
        if (intList.Count > 0)
        {
            if (!intList.Remove(newInt))
            {
                Debug.Log($"Int {newInt} not found in list.");
            }
            else
            {
                Debug.Log($"Int {newInt} removed from list.");
            }
        } else
        {
            Debug.Log("Int list is empty.");
        }
    }
    [ContextMenu("ClearIntList")]
    private void ClearIntList()
    {
        if (intList.Count > 0)
        {
            intList.Clear();
            Debug.Log("Int list cleared.");
        }
        else
        {
            Debug.Log("Int list is empty.");
        }
    }
    [ContextMenu("PrintIntList")]
    private void PrintIntList()
    {
        if (intList.Count > 0)
        {
            string intListString = "Int List: ";
            for (int i=0;i<intList.Count; i++)
            {
                intListString += "["+i + "] = " + intList[i]+"; ";
            }
            Debug.Log(intListString);
        }
        else
        {
            Debug.Log("Int list is empty.");
        }
    }
    [ContextMenu("SortASCIntList")]
    private void SortASCIntList()
    {
        if (intList.Count > 0)
        {
            intList.Sort();
            Debug.Log("Int list sorted in ascending order.");
        }
        else
        {
            Debug.Log("Int list is empty.");
        }
    }
    [ContextMenu("SortDESCIntList")]
    private void SortDESCIntList()
    {
        if (intList.Count > 0)
        {
            intList.Sort((a, b) => b.CompareTo(a));
            Debug.Log("Int list sorted in descending order.");
        }
        else
        {
            Debug.Log("Int list is empty.");
        }
    }
    [ContextMenu("SortIntByEvenOrOddList")]
    private void SortIntByEvenOrOddList()
    {
        if (intList.Count > 0)
        {
            intList.Sort((a, b) => a % 2 == b % 2 ? 0 : a % 2 == 0 ? -1 : 1);
            //intList.Sort((a, b) =>
            //{
            //    if (a % 2 == b % 2) return 0;
            //    if (a % 2 == 0) return -1;
            //    else return 1;
            //});
            Debug.Log("Int list sorted by even or odd.");
        }
        else
        {
            Debug.Log("Int list is empty.");
        }
    }

    //STRING LIST
    [ContextMenu("AddStringToList")]
    private void AddStringToList()
    {
        stringList.Add(newString);
        Debug.Log($"String {newString} added to list.");
    }

    [ContextMenu("RemoveStringFromList")]
    private void RemoveStringFromList()
    {
        if (stringList.Count > 0)
        {
            if (!stringList.Remove(newString))
            {
                Debug.Log($"String {newString} not found in list.");
            }
            else
            {
                Debug.Log($"String {newString} removed from list.");
            }
        }
        else
        {
            Debug.Log("String list is empty.");
        }
    }

    [ContextMenu("ClearStringList")]
    private void ClearStringList()
    {
        if (stringList.Count > 0)
        {
            stringList.Clear();
            Debug.Log("String list cleared.");
        }
        else
        {
            Debug.Log("String list is empty.");
        }
    }

    [ContextMenu("PrintStringList")]
    private void PrintStringList()
    {
        if (stringList.Count > 0)
        {
            string stringListString = "String List: ";
            for (int i = 0; i < stringList.Count; i++)
            {
                stringListString += "[" + i + "] = " + stringList[i] + "; ";
            }
            Debug.Log(stringListString);
        }
        else
        {
            Debug.Log("String list is empty.");
        }
    }

    [ContextMenu("SortAlphabeticalOrderStringList")]
    private void SortASCStringList()
    {
        if (stringList.Count > 0)
        {
            stringList.Sort();
            Debug.Log("String list sorted in alphabetical order.");
        }
        else
        {
            Debug.Log("String list is empty.");
        }
    }

    [ContextMenu("SortReverseAlphabeticalOrderStringList")]
    private void SortDESCStringList()
    {
        if (stringList.Count > 0)
        {
            stringList.Sort(StringDescendingComparison);
            Debug.Log("String list sorted in reverse alphabetical order.");
        }
        else
        {
            Debug.Log("String list is empty.");
        }
    }

    [ContextMenu("SortStringByLengthList")]
    private void SortStringByLengthList()
    {
        if (stringList.Count > 0)
        {
            stringList.Sort((a, b) => a.Length.CompareTo(b.Length));
            Debug.Log("String list sorted by length.");
        }
        else
        {
            Debug.Log("String list is empty.");
        }
    }

    private int StringDescendingComparison(string a, string b)
    {
        return b.CompareTo(a);
    }

}
