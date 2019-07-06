using UnityEngine;
using System.Collections;

public class ConstantRotationInZ : MonoBehaviour
{
    [Header("It makes an object rotate always")]
    [SerializeField] Transform objectToRotate;
    [SerializeField] float velocityToRotate;

	void Update ()
    {
        objectToRotate.Rotate(objectToRotate.eulerAngles.x, objectToRotate.eulerAngles.y, Time.deltaTime * velocityToRotate * 100);
	}
}
