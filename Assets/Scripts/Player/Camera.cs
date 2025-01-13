using UnityEngine;
using System;
using NUnit.Framework.Constraints;
using UnityEditor.Callbacks;
public class Camera : MonoBehaviour
{
    public GameObject target;
    int zoom_value = 5;
    int max_zoom = 10;
    public float moveLerp = 0.4f;
    public float rotateLerp = 0.4f;
    Global global;
    enum CameraMode
    {
        FirstPerson,
        ThirdPerson
    }
    CameraMode mode = CameraMode.ThirdPerson;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        global = Global.Instance;
        transform.position = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        target.transform.rotation = Quaternion.Euler(0, yRotation, 0); ;
        CameraFirstPerson();
        CameraThirdPerson();
        CameraZoom();

    }
    float xRotation;
    float yRotation;
    float sens = 10.0f;
    public void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sens;
        float mouseY = Input.GetAxis("Mouse Y") * sens;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(xRotation, yRotation, 0), rotateLerp);

    }
    void CameraFirstPerson()
    {
        transform.position = target.transform.position;
        if (mode != CameraMode.FirstPerson)
        {
            return;
        }
        CameraRotation();

    }
    void CameraZoom()
    {
        if (zoom_value > 0)
        {
            mode = CameraMode.ThirdPerson;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            mode = CameraMode.FirstPerson;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            zoom_value = Math.Clamp(zoom_value - 1, 0, max_zoom);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            zoom_value = Math.Clamp(zoom_value + 1, 0, max_zoom);
        }

    }
    void CameraThirdPerson()
    {
        if (mode != CameraMode.ThirdPerson)
        {
            return;
        }
        float offset = 2 * (float)Math.Pow(1.5, zoom_value);
        Vector3 direction = new Vector3(0, 0, -offset - 1.0f);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

        Vector3 desiredPosition = target.transform.position + rotation * direction;
        desiredPosition = AccountForWalls(desiredPosition, target.transform.position);
        transform.position = desiredPosition;
        if (Input.GetMouseButton((int)global.controller.moveCamera))
        {
            CameraRotation();
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateLerp);
    }
    Vector3 AccountForWalls(Vector3 cameraPosition, Vector3 targetPosition)
    {
        Vector3 direction = cameraPosition - targetPosition;
        float distance = direction.magnitude;
        direction.Normalize();
        RaycastHit hit;
        if (Physics.Raycast(targetPosition, direction, out hit, distance))

        {

            // If the ray hits something, log the hit information

            // Debug.Log("Hit: " + hit.collider.name);

            // Debug.Log("Hit Point: " + hit.point);

            // Debug.Log("Distance: " + hit.distance);

            Vector3 newCameraPosition = hit.point - direction;
            return newCameraPosition;
        }

        else

        {

            // Debug.Log("No hit detected.");

        }
        Debug.DrawLine(targetPosition, cameraPosition, Color.red);
        return cameraPosition;
    }
}
