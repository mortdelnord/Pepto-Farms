using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FirstPersonCam : MonoBehaviour
{
    public Transform player;

    public CinemachineVirtualCamera firstPersonCam;
    private CinemachinePOV povCam;

    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        povCam = firstPersonCam.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {

        float yRotation = povCam.m_HorizontalAxis.Value;
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
