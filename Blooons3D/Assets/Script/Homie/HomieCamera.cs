using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomieCamera : MonoBehaviour
{
    public static HomieCamera Instance;
    public bool canRotate;
    public float mouseSensitivity = 5;
    public float zoomSensitivity = 2;
    public HomieMovement target;
    public Vector2 minMaxPitch = new Vector2(10, 80);
    public Vector2 minMaxZoom = new Vector2(5, 15);
    public GameObject Nose, Torso;
    public Transform CameraPitch;
    public int cameraMode;
    Vector3 defaultpos;


    private float yaw, pitch, zoom;
    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        if (Application.isPlaying)
        {
           Cursor.lockState = CursorLockMode.Locked;
        }
        mainCamera = Camera.main;
        Instance = this;
    }
    void Start()
    {
        defaultpos = CameraPitch.localPosition;
        //target = gameObject.GetComponent<HomieMovement>();
    }

    void Update()
    {
        if (canRotate)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minMaxPitch.x, minMaxPitch.y);
           // zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
            zoom = Mathf.Clamp(zoom, minMaxZoom.x, minMaxZoom.y);
            target.Head.localEulerAngles = new Vector3(pitch - 15, 0, 0);


            transform.position = target.Head.transform.position;
            transform.localEulerAngles = new Vector3(0, yaw, 0);
            transform.GetChild(0).localEulerAngles = new Vector3(pitch, 0, 0);
            mainCamera.transform.localPosition = Vector3.forward * -zoom;
        }
    }
    private void LateUpdate()
    {
    }
    public void SwitchCamera(int cameraint)
    {
        switch (cameraint)
        {
            case 1:
                CameraPitch.localPosition = defaultpos;
                break;
            case 2:
                CameraPitch.localPosition = new Vector3(CameraPitch.localPosition.x, 3, CameraPitch.localPosition.z);
                break;
        }


    }
    public void EnterFirstPerson() {
        cameraMode = 2;
        minMaxZoom = new Vector2(0,0);
        zoom = 0;
        Nose.GetComponent<MeshRenderer>().enabled = false;
        Torso.GetComponent<MeshRenderer>().enabled = false;
    }
    public void EnterThirdPerson() {
        minMaxZoom = new Vector2(5, 15);
        zoom = 10;
        cameraMode = 1;
        Nose.GetComponent<MeshRenderer>().enabled = true;
        Torso.GetComponent<MeshRenderer>().enabled = true;

    }

}
