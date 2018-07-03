using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LoadImageByIOExample : MonoBehaviour {
    
	void Start () {
        if (transform.parent == null || transform.parent.GetComponent<Canvas>() == null)
        {
            Debug.LogError("请把该示例(LoadImageExample)放置在Canvas内。");
            return;
        }

        Texture2D tex = new Texture2D(1920,1080);
        tex.LoadImage( LoadLocalImageByIO.LoadImageBytes(ConfigXML.AppMediaPath + @"TestImage.png"));
        //不手动掉用销毁，有可能引发内存泄漏！！！！！
        if (GetComponent<Image>().sprite != null)
            DestroyImmediate(GetComponent<Image>().sprite.texture, false);
        GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }
}
