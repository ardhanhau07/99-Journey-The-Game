using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    public Transform Player;
    public Transform ViewPoint;
    public Transform AIMViewPoint;
    public float RotationSpeed;
    public GameObject TPSCamera, AIMCamera;
    public GameObject crosshair;
    public Vector3 TPSCameraOffset;
    public Vector3 AIMCameraOffset;
    public float zoomSpeed = 2f;
    public float minZoom = -10f;
    public float maxZoom = -2f;

    private float currentZoom;
    private bool TPSMode = true, AIMMode = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.SetActive(false);
        currentZoom = TPSCameraOffset.z;
    }

    void Update()
    {
        HandleCameraZoom();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 viewDir = Player.position - new Vector3(transform.position.x, Player.position.y, transform.position.z);
        ViewPoint.forward = viewDir.normalized;

        if (TPSMode)
        {
            Vector3 InputDir = ViewPoint.forward * verticalInput + ViewPoint.right * horizontalInput;
            if (InputDir != Vector3.zero)
            {
                Player.forward = Vector3.Slerp(Player.forward, InputDir.normalized, Time.deltaTime * RotationSpeed);
            }

            TPSCamera.transform.position = Player.position + new Vector3(TPSCameraOffset.x, TPSCameraOffset.y, currentZoom);
        }
        else if (AIMMode)
        {
            Vector3 dirToCombatLookAt = AIMViewPoint.position - new Vector3(transform.position.x, AIMViewPoint.position.y, transform.position.z);
            AIMViewPoint.forward = dirToCombatLookAt.normalized;

            Player.forward = Vector3.Slerp(Player.forward, dirToCombatLookAt.normalized, Time.deltaTime * RotationSpeed);

            AIMCamera.transform.position = Player.position + AIMCameraOffset;
        }

        CameraModeChanger(TPSMode, AIMMode);
    }

    private void HandleCameraZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            currentZoom += scrollInput * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }
    }

    public void CameraModeChanger(bool TPS, bool AIM)
    {
        TPSMode = TPS;
        AIMMode = AIM;

        if (TPS)
        {
            TPSCamera.SetActive(true);
            AIMCamera.SetActive(false);
            crosshair.SetActive(false);
        }
        else if (AIM)
        {
            TPSCamera.SetActive(false);
            AIMCamera.SetActive(true);
            crosshair.SetActive(true);
        }
    }
}
