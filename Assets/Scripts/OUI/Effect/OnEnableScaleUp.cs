using UnityEngine;
using System.Collections;
using DG.Tweening;

public class OnEnableScaleUp : MonoBehaviour
{
    [SerializeField] private Vector3 _OriginalScale,_TargetScale;
    [SerializeField] private float _Duration = 1;

    void OnEnable()
    {
        transform.localScale = _OriginalScale;
        transform.DOScale(_TargetScale, _Duration);
    }
}
