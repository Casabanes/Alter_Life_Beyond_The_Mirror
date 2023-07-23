using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 80f;
    [SerializeField]
    private float _xPositiveClamp;
    [SerializeField]
    private float _xNegativeClamp;
    private float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yRotation -= mouseX;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, _xNegativeClamp, _xPositiveClamp);
        transform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f);
    }
}
