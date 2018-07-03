using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotateSpeed;

	void Start () {}
	void Update () {
	    transform.Rotate(rotateSpeed);
	}
}
