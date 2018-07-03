using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class ExternalTextFileTest : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().text = ConfigXML.XMLData["TextList"].ChildNodes[0].InnerText;
    }
}
