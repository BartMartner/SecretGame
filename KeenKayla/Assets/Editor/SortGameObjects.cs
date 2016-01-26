using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class SortGameObjects : MonoBehaviour 
{
    [MenuItem("Extensions/Sort Game Objects")]
    public static void SortSelected()
    {
        var transforms = new List<Transform>(UnityEditor.Selection.GetTransforms(SelectionMode.TopLevel));
        transforms = transforms.OrderByDescending(t => t.position.y).ThenBy(t => t.position.x).ToList();
        var lowestIndex = transforms[0].GetSiblingIndex();
        foreach (var t in transforms)
        {
            var i = t.GetSiblingIndex();
            if (i < lowestIndex)
            {
                lowestIndex = i;
            }
        }

        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].SetSiblingIndex(i + lowestIndex);
        }
    }
}
