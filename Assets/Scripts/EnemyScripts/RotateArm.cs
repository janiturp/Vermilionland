using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArm : MonoBehaviour
{
    public float currentAngle;
    public float startAngle;
    public bool rotating;
    public float rotateDuration;
    public float counter;
    private float _zAngle;

    public float zAngle
    {
        get
        {
            return _zAngle;
        }
        set
        {
            startAngle = transform.localRotation.eulerAngles.z;
            _zAngle = value;
            rotating = true;
            counter = 0;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;

        if (counter > rotateDuration && rotating)
        {
            rotating = false;
        }

        currentAngle = Mathf.LerpAngle(startAngle, _zAngle, counter / rotateDuration);
        transform.localEulerAngles = new Vector3(0, 0, currentAngle);
    }
}
