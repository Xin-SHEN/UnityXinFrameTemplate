using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HiveEffect : MonoBehaviour
{
    //-----序列化变量
    [SerializeField] private Material _material;

    //-----内部变量
    private Image _image;
    private float _time=0;

    void OnEnable()
    {
        _time = 0;
    }

    void Start ()
	{
	    _image = GetComponent<Image>();
	    _image.material = _material;
	    _material.mainTexture = _image.mainTexture;
    }

    void Update()
    {
        if (_time > 1) return;
        _time += 0.01f;
        _material.SetFloat("_Cutoff", _time);
    }
}
