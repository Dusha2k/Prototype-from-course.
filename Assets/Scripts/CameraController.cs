using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform target;

    private float _startFOV, _targetFOV;

    public float zoomSpeed = 1f;
    public Camera theCam;
    private void Awake()
    {
        instance = this;   
    }
    void Start()
    {
        _startFOV = theCam.fieldOfView;
        _targetFOV = _startFOV;
    }

    
    void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;

        theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, _targetFOV, zoomSpeed * Time.deltaTime);
    }

    public void ZoomIn(float newZoom)
    {
        _targetFOV = newZoom;
    }

    public void ZoomOut()
    {
        _targetFOV = _startFOV;
    }
}
