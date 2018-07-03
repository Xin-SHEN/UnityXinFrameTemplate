using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CDKeyCover : MonoBehaviour {
    //CDKEY
    private CDKey _cdKey;
    [HideInInspector] public bool IsValid = false;
    
    void Start()
    {
        CheckCDKEY();
    }

    void CheckCDKEY()
    {
        //#if UNITY_EDITOR
        if (CDKey.Instance.VerifyCDKEY())
        {
            transform.GetChild(0).gameObject.SetActive(false);
            IsValid = true;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponentInChildren<InputField>().text = CDKey.Instance.CreateRequestFile();
            StartCoroutine(DelayedShowMouse());
        }
        //#endif
    }

    IEnumerator DelayedShowMouse() {
        yield return new WaitForSeconds(1);
        Cursor.visible = true;
    }

    public void CopyRegisterCode()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = GetComponentInChildren<InputField>().text;
        textEditor.OnFocus();
        textEditor.Copy();
    }
}
