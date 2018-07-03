using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class VideoBackground : MonoBehaviour {
    
	void Start ()
	{
	    string firstImageURL = GetImageFileURL(0);
        UpdateImage(firstImageURL);
    }

    /// <summary>
    /// API:根据序号更改背景图片
    /// </summary>
    /// <param name="imageURL"></param>
    public void ChangeImageAt(int index) {
        UpdateImage(GetImageFileURL(index));
    }
    
    //更新图片
    void UpdateImage(string imageURL) {
        if (string.IsNullOrEmpty(imageURL)) return; //如果文件不存在

        Texture2D tex = new Texture2D(1920, 1080);
        tex.LoadImage(LoadLocalImageByIO.LoadImageBytes(imageURL));
        //不手动掉用销毁，有可能引发内存泄漏！！！！！
        if (GetComponent<Image>().sprite != null)
            DestroyImmediate(GetComponent<Image>().sprite.texture, false);
        GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }
    
    //检查第一张图片是否存在，并获取地址
    string GetImageFileURL(int index)
    {        
        if (index < 0 || index > ConfigXML.XMLData["ImageList"].ChildNodes.Count)
        {
            Debug.LogError("图片序号超出边界！");
            return null;
        }
        string _filename = ConfigXML.XMLData["ImageList"].ChildNodes[index].InnerText;

        if (File.Exists(ConfigXML.AppMediaPath + _filename))
            return ConfigXML.AppMediaPath + _filename;
        else
            return null;
    }
}
