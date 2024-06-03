using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

public class CameraFollower : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]GameObject camera;

    Transform cameraTransform;

    void Start()
    {
        cameraTransform = camera.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        //UpdateRotation();
    }
    void UpdatePosition()
    {
        Vector3 pos= cameraTransform.position;
        pos.y = transform.position.y;
        pos.z = cameraTransform.position.z;
        transform.position = pos;
    }
    void UpdateRotation()
    {
        Quaternion cameraRotation = cameraTransform.rotation;
        Vector3 eulerCameraAngles = cameraRotation.eulerAngles;

        Vector3 childEulerAngles = transform.rotation.eulerAngles;
        childEulerAngles.y = eulerCameraAngles.y;
        Quaternion childRotation = Quaternion.Euler(childEulerAngles);
        transform.rotation = childRotation;
    }
}
