using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AutoRotateInterval : MonoBehaviour {
    [SerializeField] private Vector3 rotateSpeed;
    [SerializeField] private float interval=1f;

    void Start()
    {
        StartCoroutine(RotateWithInterval(interval));
    }
    IEnumerator RotateWithInterval(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Vector3 temp = transform.localRotation.eulerAngles + rotateSpeed;
            transform.DOLocalRotate(temp, interval - 0.1f);
        }
    }
}
