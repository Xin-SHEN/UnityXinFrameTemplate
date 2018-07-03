using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadLocalImageByIO{

    /// <summary>
    /// 以IO方式进行加载,Media文件夹下的相对路径
    /// </summary>
    public static byte[] LoadImageBytes(string ImageURL)
    {        
        //创建文件读取流
        FileStream fileStream = new FileStream(ImageURL, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        return bytes;
    }
}
