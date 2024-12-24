using UnityEngine;


public class CharacterAiming : MonoBehaviour
{
    [SerializeField] private float _turnSpeed = 15f;

    //[SerializeField] private CinemachineVirtualCamera _aimCamera;
    private Camera _mainCamera;


    //=====================Awake Method=====================//
    private void Awake()
    {
        _mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }

    //=====================FixedUpdate Method=====================//
    void FixedUpdate()
    {
        //Handle Player Rotation base on camera
        float cameraYAxis = _mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, cameraYAxis, 0), _turnSpeed * Time.fixedDeltaTime);
    }



}
