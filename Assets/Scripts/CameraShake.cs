using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake _instance;

    public static CameraShake instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<CameraShake>();
            }
            return _instance;
        }
    }

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private float shakeElapsedTime;
    private float amplitude;
    private float frequency;
    private void Awake()
    {
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        shakeElapsedTime = 0f;
    }

    private void Update()
    {
        ShakeCamera();
    }

    private void ShakeCamera()
    {
        if(shakeElapsedTime > 0)
        {
            virtualCameraNoise.m_AmplitudeGain = amplitude;
            virtualCameraNoise.m_FrequencyGain = frequency;

            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            virtualCameraNoise.m_AmplitudeGain = 0f;
            shakeElapsedTime = 0f;
        }        
    }

    public void SetCameraSake(float _amplitude, float _frequency, float _shakeTime)
    {
        amplitude = _amplitude;
        frequency = _frequency;
        shakeElapsedTime = _shakeTime;
    }
}
