//HideWhenTrueAttribute.cs 
//Should be placed outside an Editor folder.

using UnityEngine;
using System.Collections;

public class HideWhenTrueAttribute : PropertyAttribute
{
    public readonly string hideBoolean;

    public HideWhenTrueAttribute(string booleanName)
    {
        this.hideBoolean = booleanName;
    }
}
