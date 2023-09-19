using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private bool isShake = false;
    private float shakeDuration;

    CinemachineBasicMultiChannelPerlin noise;
    CinemachineBrain brain;


    private void Start()
    {
        noise = GetComponentInChildren<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        brain = GetComponentInChildren<CinemachineBrain>();

    }


    public void StartShake(float width, float duration)
    {
        isShake = true;
        shakeDuration = duration;
        noise.m_AmplitudeGain = width;

        StartCoroutine(WaitShake());


    }


    private IEnumerator WaitShake()
    {
        yield return new WaitForSecondsRealtime(shakeDuration);

        isShake = false;
        noise.m_AmplitudeGain = 0;
    }

    private void Update()
    {
        if (GameManager.inst.timeScale == 0)
        {
            brain.ManualUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.inst.timeScale != 0)
        {
            brain.ManualUpdate();
        }
    }
}
