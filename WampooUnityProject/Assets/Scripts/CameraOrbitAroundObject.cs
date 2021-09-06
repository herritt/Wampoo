using UnityEngine;

public class CameraOrbitAroundObject : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private float rotateSpeed;

    private Vector3 previousPosition;

    private void Update()
    {
        cam.transform.RotateAround(target.position, target.forward, Time.deltaTime * rotateSpeed);
    }
}