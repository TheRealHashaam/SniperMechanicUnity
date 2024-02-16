using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
public class SniperController : MonoBehaviour
{
    public float AimSpeed;
    public bool _isAiming = false;
    public bool canAim = true;
    public GameObject DefaultBlend, ScopeBlend;
    public CinemachineVirtualCamera VirtualCamera;
    public Gun gun;
    public StarterAssetsInputs assetsInputs;
    public float MaxBreath = 5f;
    private float currentBreath;
    public Image BreathSlider;
    public float duration;
    bool slider, CanHoldBreath;
    CinemachineBasicMultiChannelPerlin Perlin;
    private void Start()
    {
        currentBreath = MaxBreath;
        Perlin = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    void Update()
    {
        if(assetsInputs.BreathHold && _isAiming&& CanHoldBreath && canAim)
        {
            currentBreath -= Time.deltaTime;

            if (!slider)
            {
                slider = true;
                Perlin.m_FrequencyGain = 0;
                ShowHiderSlider(1);
            }
            if (currentBreath <= 0f)
            {
                CanHoldBreath = false;
            }
        }
        else
        {
            currentBreath += Time.deltaTime;
            if(currentBreath>= MaxBreath)
            {
                CanHoldBreath = true;
            }
            if (slider)
            {
                slider = false;
                Perlin.m_FrequencyGain = 0.3f;
                ShowHiderSlider(0);
            }
        }
        currentBreath = Mathf.Clamp(currentBreath, 0, MaxBreath);
        UpdateBreathSlider();

    }

    public void HideHealthBar()
    {
        ShowHiderSlider(0);
    }

    void UpdateBreathSlider()
    {
        BreathSlider.fillAmount = currentBreath / MaxBreath;
    }
    void ShowHiderSlider(int val)
    {
        BreathSlider.DOFade(val, duration);
    }
    public void OnAim()
    {
        if(!_isAiming)
        {
            _isAiming = true;
            Aim();
        }
        else
        {
            _isAiming = false;
            UnEquip();
        }
    }

    public void Aim()
    {
        DefaultBlend.SetActive(false);
        ScopeBlend.SetActive(true);
        gun.Aim();
    }
    public void UnEquip()
    {
        DefaultBlend.SetActive(true);
        ScopeBlend.SetActive(false);
        gun.UnEquip();
    }

    public void OnFire()
    {
        gun.Fire();
    }
}
