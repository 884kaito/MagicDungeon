using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//control camera
public class CameraController : MonoBehaviour
{
    private float shakeDuration;

    CinemachineBasicMultiChannelPerlin noise;


    private void Start()
    {
        noise = GetComponentInChildren<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void StartShake(float width, float duration)
    {
        //set parameters
        shakeDuration = duration;
        noise.m_AmplitudeGain = width;

        StartCoroutine(WaitShake());


    }


    private IEnumerator WaitShake()
    {
        yield return new WaitForSecondsRealtime(shakeDuration);

        //stop shake
        noise.m_AmplitudeGain = 0;
    }
}
