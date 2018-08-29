/*****************************************************************************
 * filename :  SetPropertyAttribute.cs
 * author   :  Zhang Yunxing
 * date     :  2018/08/28 21:07
 * desc     :  自定义SetPropertyAttribute
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPropertyAttribute : PropertyAttribute
{
    public string Name { get; private set; }
    public bool IsDirty { get; set; }
    public SetPropertyAttribute(string name)
    {
        this.Name = name;
    }
}