using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMover : MonoBehaviour
{
    [Header("Framing")]
    [SerializeField] private Camera slamera = null;
    [SerializeField] private GameObject proto = null;
    [SerializeField] private Transform followTransform = null;
    [SerializeField] private Vector3 framing = new Vector3(0, 0, 0);

    [Header("Distance")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minDistance = 0f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float defaultDistance = 5f;
    
    [Header("Rotation")]
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = true;
    [SerializeField] private float rotationSharpness = 25f;
    [SerializeField] private float rotationXSpeed = 25f;
    [SerializeField] private float rotationYSpeed = 25f;
    [SerializeField] private float defaultVerticalAngle = 20;
    [SerializeField] [Range(-90, 90)]private float minVerticalAngle = -90;
    [SerializeField] [Range(-90, 90)]private float maxVerticalAngle = 90;
    private List<Collider> ignoreColliders = new List<Collider>();

    [Header("Obstructions")]
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask obstructionLayers = -1;


    //Privates
    private Vector3 planarDirection; //cam forward on x,z plane
    private Quaternion targetRotation;
    private float targetVerticalAngle;
    private Vector3 targetPosition;
    private float targetDistance; 
    private Vector3 newPosition;
    public Quaternion newRotation;
    

    private void OnValidate()
    {
        defaultDistance = Mathf.Clamp(defaultDistance, minDistance, maxDistance);
        defaultVerticalAngle = Mathf.Clamp(defaultVerticalAngle, minVerticalAngle, maxVerticalAngle);
    }

    private void Start()
    {
        ignoreColliders.AddRange(GetComponentsInChildren<Collider>());
        planarDirection = followTransform.forward;
        Cursor.lockState = CursorLockMode.Locked;
        targetDistance = defaultDistance;
        targetVerticalAngle = defaultVerticalAngle;
        targetRotation = Quaternion.LookRotation(planarDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        targetPosition = followTransform.position - (targetRotation * Vector3.forward) * targetDistance;
    }

    private void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        return;

        //Handles inputs
        float zoom = -PlayerInputs.MouseScrollInput * zoomSpeed;
        float mouseX = PlayerInputs.MouseXInput * rotationXSpeed;
        float mouseY = PlayerInputs.MouseYInput * rotationYSpeed;

        if (invertX) {mouseX *= -1f;}
        if (invertY) {mouseY *= -1f;}

        Vector3 focusPosition = followTransform.position + slamera.transform.TransformDirection(framing);

        planarDirection = Quaternion.Euler(0, mouseX, 0) * planarDirection;
        targetDistance =Mathf.Clamp(targetDistance + zoom, minDistance, maxDistance);
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + mouseY, minVerticalAngle, maxVerticalAngle);

        Debug.DrawLine(slamera.transform.position, slamera.transform.position + planarDirection, Color.red);


        //Handles Obstructions
        float smallestDistance = targetDistance;
        RaycastHit[] hits = Physics.SphereCastAll(focusPosition, checkRadius, targetRotation * -Vector3.forward, targetDistance, obstructionLayers);
        if (hits.Length != 0)
            foreach (RaycastHit hit in hits)
                if(!ignoreColliders.Contains(hit.collider))
                    if (hit.distance < smallestDistance)
                        smallestDistance = hit.distance;

        //Final Targets
        targetRotation = Quaternion.LookRotation(planarDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
        targetPosition = focusPosition - (targetRotation * Vector3.forward) * smallestDistance;

        //Handle Smoothing
        newRotation = Quaternion.Slerp(slamera.transform.rotation, targetRotation, Time.deltaTime * rotationSharpness);
        newPosition = Vector3.Lerp(slamera.transform.position, targetPosition, Time.deltaTime * rotationSharpness);

        //Apply
        slamera.transform.rotation = newRotation;
        slamera.transform.position = newPosition;
    }

}
