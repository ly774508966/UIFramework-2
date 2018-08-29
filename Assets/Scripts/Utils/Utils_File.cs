/*****************************************************************************
 * filename :  Utils.cs
 * author   :  Zhang Yunxing
 * date     :  2018/08/29 17:41
 * desc     :  和file相关额Util函数
 * changelog:  
*****************************************************************************/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;
public partial class Utils
{
    /// <summary>
    /// 将一行字符串写入文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool WriteStringToFile(string path, string text)
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(text);
            sw.Close();
            fs.Close();
            return true;
        }
        catch (Exception ex)
        {
            LogModule.LogError(ex);
        }
        return false;
    }

    /// <summary>
    /// 将一行字符串写入文件,如果文件已经存在，则追加字符串
    /// </summary>
    /// <param name="path"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool AppendStringToFile(string path, string text)
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(text);

            sw.Close();
            fs.Close();

            return true;
        }
        catch (Exception ex)
        {
            LogModule.LogError(ex);
        }
        return false;
    }

    /// <summary>
    /// 从文件读取一个字符串
    /// </summary>
    /// <param name="path"></param>
    /// <param name="retString"></param>
    /// <returns></returns>
    public static bool ReadFileString(string path, ref string retString)
    {
        try
        {
            if (!File.Exists(path))
            {
                return false;
            }
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);
            retString = sr.ReadToEnd();
            sr.Close();
            fs.Close();

            return true;
        }
        catch (Exception ex)
        {
            LogModule.LogError(ex.ToString());
            return false;
        }
    }

    /// <summary>
    /// 从文件读取一个INT值
    /// </summary>
    /// <param name="path"></param>
    /// <param name="retInt"></param>
    /// <returns></returns>
    public static bool ReadFileInt(string path, out int retInt)
    {
        string text = "";
        retInt = 0;
        if (!ReadFileString(path, ref text))
        {
            return false;
        }

        if (!int.TryParse(text, out retInt))
        {
            LogModule.LogError("parse int error path:" + path);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 检查目录是否存在，不存在则创建对应路径
    /// </summary>
    /// <param name="targetPath"></param>
    public static void CheckTargetPath(string targetPath)
    {
        targetPath = targetPath.Replace('\\', '/');

        int dotPos = targetPath.LastIndexOf('.');
        int lastPathPos = targetPath.LastIndexOf('/');

        if (dotPos > 0 && lastPathPos < dotPos)
        {
            targetPath = targetPath.Substring(0, lastPathPos);
        }
        if (Directory.Exists(targetPath))
        {
            return;
        }

        string[] subPath = targetPath.Split('/');
        string curCheckPath = "";
        int subContentSize = subPath.Length;
        for (int i = 0; i < subContentSize; i++)
        {
            curCheckPath += subPath[i] + '/';
            if (!Directory.Exists(curCheckPath))
            {
                Directory.CreateDirectory(curCheckPath);
            }
        }
    }

    /// <summary>
    /// 删除对应文件
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteFile(string path)
    {
        if (!File.Exists(path))
        {
            return;
        }
        System.IO.File.Delete(path);
    }

    /// <summary>
    /// 删除一个路径下所有文件
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            return;
        }

        string[] strTemp;
        //先删除该目录下的文件
        strTemp = System.IO.Directory.GetFiles(path);
        foreach (string str in strTemp)
        {
            System.IO.File.Delete(str);
        }
        //删除子目录，递归
        strTemp = System.IO.Directory.GetDirectories(path);
        foreach (string str in strTemp)
        {
            DeleteFolder(str);
        }

        System.IO.Directory.Delete(path);
    }

    /// <summary>
    /// 拷贝一个路径下所有的文件，不包含子目录
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="distPath"></param>
    public static void CopyPathFile(string srcPath, string distPath)
    {
        if (!Directory.Exists(srcPath))
        {
            return;
        }

        Utils.CheckTargetPath(distPath);

        string[] strLocalFile = System.IO.Directory.GetFiles(srcPath);
        foreach (string curFile in strLocalFile)
        {
            System.IO.File.Copy(curFile, distPath + "/" + Path.GetFileName(curFile), true);
        }
    }

    /// <summary>
    /// 根据路径和后缀获取所有的文件
    /// </summary>
    /// <param name="retList"></param>
    /// <param name="curPath"></param>
    /// <param name="fileEnd"></param>
    public static void GetFileListByPathAndType(List<string> retList, string curPath, string fileEnd)
    {
        if (null == retList)
        {
            return;
        }

        string[] fileList = Directory.GetFiles(curPath);
        string[] dictionaryList = Directory.GetDirectories(curPath);

        foreach (string curFile in fileList)
        {
            if (curFile.EndsWith(fileEnd))
            {
                string curFilePath = curFile.Replace('\\', '/');
                retList.Add(curFilePath);
            }
        }

        // 逐层目录开始遍历，获取所有的file end的文件
        foreach (string curDic in dictionaryList)
        {
            string curDicName = curDic.Replace('\\', '/');
            GetFileListByPathAndType(retList, curDicName, fileEnd);
        }
    }

    /// <summary>
    /// 根据路径和排除后缀获取所有的文件
    /// </summary>
    /// <param name="retList"></param>
    /// <param name="curPath"></param>
    /// <param name="exceptFileEnd"></param>
    public static void GetFileListByPathWithoutType(List<string> retList, string curPath, string exceptFileEnd)
    {
        if (null == retList)
        {
            return;
        }

        string[] fileList = Directory.GetFiles(curPath);
        string[] dictionaryList = Directory.GetDirectories(curPath);

        foreach (string curFile in fileList)
        {
            if (!curFile.EndsWith(exceptFileEnd))
            {
                string curFilePath = curFile.Replace('\\', '/');
                retList.Add(curFilePath);
            }
        }

        // 逐层目录开始遍历，获取所有的file end的文件
        foreach (string curDic in dictionaryList)
        {
            string curDicName = curDic.Replace('\\', '/');
            GetFileListByPathWithoutType(retList, curDicName, exceptFileEnd);
        }
    }

    /// <summary>
    /// 获取字符的Md5值
    /// </summary>
    /// <param name="srcString"></param>
    /// <returns></returns>
    public static string GetStringMD5(string srcString)
    {
        MD5CryptoServiceProvider oMD5Hasher = new MD5CryptoServiceProvider();
        MemoryStream msm = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(srcString));
        byte[] arrbytHashValue = oMD5Hasher.ComputeHash(msm);
        string strHashData = System.BitConverter.ToString(arrbytHashValue);
        return strHashData.Replace("-", "");
    }
    /// <summary>
    /// 获取MD5
    /// </summary>
    /// <param name="pathName"></param>
    /// <returns></returns>
    public static string GetMD5Hash(string pathName)
    {

        string strResult = "";
        string strHashData = "";
#if !UNITY_WP8
        byte[] arrbytHashValue;
#endif
        System.IO.FileStream oFileStream = null;
        MD5CryptoServiceProvider oMD5Hasher = new MD5CryptoServiceProvider();
        try
        {
            oFileStream = new System.IO.FileStream(pathName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
#if UNITY_WP8
                strHashData = oMD5Hasher.ComputeHash(oFileStream);
                oFileStream.Close();
#else
            arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream);
            oFileStream.Close();
            strHashData = System.BitConverter.ToString(arrbytHashValue);
            strHashData = strHashData.Replace("-", "");
#endif

            strResult = strHashData;
        }
        catch (System.Exception ex)
        {
            LogModule.LogError("read md5 file error :" + pathName + " e: " + ex.ToString());
        }
        return strResult;
    }
}
