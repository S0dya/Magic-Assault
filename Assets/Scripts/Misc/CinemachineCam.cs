using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCam : SingletonMonobehaviour<CinemachineCam>
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera.Follow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
}
