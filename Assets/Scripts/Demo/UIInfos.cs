using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.UICore;

public class UIInfos
{
    // 核心数据差异化构造不同类型的UI
    public static UICoreData UI_TypeBase = new UICoreData(UIRootType.Base, UIShowModel.DoNoting, UIColliderType.Penetrate);


    // 游戏中的所有UI定义
    // eg:
    public static UIInfoData defaultUI = new UIInfoData(UI_TypeBase, "", "");
    public static UIInfoData baseUI = new UIInfoData(UI_TypeBase, "cccc", "ccc");

}