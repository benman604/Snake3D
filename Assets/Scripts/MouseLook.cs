using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float cameraBuffer = 1;
    public Transform toPosition;
    float xRotation = 45f;
    float yRotation = 10f;

    public GameObject pointer;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.position = Vector3.Lerp(transform.position, toPosition.position, 0.2f);

        Vector3 rayDir = transform.TransformDirection(Vector3.back);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, rayDir * 40, out hit, 50))
        {
            Camera.main.transform.position = hit.point + transform.forward * cameraBuffer; //r.GetPoint(hit.distance - 4);
        }
        Camera.main.transform.rotation = transform.rotation;

        dir = transform.eulerAngles;
        dir.x = 0;
        dir.y = Mathf.Round(dir.y / 90) * 90;
        dir.z = Mathf.Round(dir.z / 90) * 90;
        pointer.transform.eulerAngles = dir;
    }
}