using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LoadImageByWWWExample : MonoBehaviour
{
    void Start()
    {
        if (transform.parent == null || transform.parent.GetComponent<Canvas>() == null)
        {
            Debug.LogError("请把该示例(LoadImageExample)放置在Canvas内。");
            return;
        }
        string _fileURL = CheckImageFile();
        StartCoroutine(WaitLoad(_fileURL));
    }

    //检查图片是否存在
    string CheckImageFile()
    {
        int index = 0;
        if (index < 0 || index > ConfigXML.XMLData["ImageList"].ChildNodes.Count)
        {
            Debug.LogError("图片序号超出边界！");
            return null;
        }
        string _filename = ConfigXML.XMLData["ImageList"].ChildNodes[index].InnerText;
        return ConfigXML.AppMediaPath + _filename;
    }

    //加载图片
    IEnumerator WaitLoad(string fileName)
    {
        WWW wwwTexture = new WWW("file://" + fileName);
        //Debug.Log(wwwTexture.url);
        yield return wwwTexture;
        
        Texture2D tex = wwwTexture.texture;
        //不手动掉用销毁，有可能引发内存泄漏！！！！！
        if(GetComponent<Image>().sprite !=null)
            DestroyImmediate(GetComponent<Image>().sprite.texture,false);
        GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }
}
