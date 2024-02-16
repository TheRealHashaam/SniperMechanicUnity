using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using StarterAssets;

public class Gun : MonoBehaviour
{
    public Transform Sniper;
    public Transform StartPos, AimPos;
    public float AimSpeed;
    public CinemachineImpulseSource ShakeImpulse;
    public string EnemyTag;
    public ParticleSystem FireParticle;
    bool _canFire = true;
    public Animator GunAnimator;
    public Camera ScopeCamera;
    public GameObject ImpactParticle, BloodParticle;
    public SniperController SniperController;
    public KillCams KillCam;
    GameObject temp;
    public GameObject BuletPrefab;
    public Transform BulletPoint;
    public float CamDuration;
    public Volume globalVolume;
    public AudioSource FireSound, Reload, SlowMoFire;
    public void Aim()
    {
        Sniper.DOLocalMove(AimPos.localPosition, AimSpeed);
    }
    public void UnEquip()
    {
        Sniper.DOLocalMove(StartPos.localPosition, AimSpeed);
    }
    public void CheckReload()
    {
        if(SniperController._isAiming)
        {
            SniperController.UnEquip();
        }
    }
    public void CheckAim()
    {
        if (SniperController._isAiming)
        {
            SniperController.Aim();
        }
    }
    public void Fire()
    {
        SniperController.canAim = false;
        if (!_canFire)
            return;
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0.0f);
        Ray ray = ScopeCamera.ViewportPointToRay(screenCenter);
        RaycastHit hit;
        _canFire = false;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponentInParent<EnemyController>())
            {
                EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();
                SniperController.HideHealthBar();
                PerformCinematic(hit.point, enemy.transform,hit, enemy);
            }
            else
            {
                Instantiate(ImpactParticle, hit.point, Quaternion.LookRotation(hit.normal));
                
                FireSound.Play();
            }
            GunAnimator.SetTrigger("Fire");
            FireParticle.Play();
            ShakeImpulse.GenerateImpulse();
        }
        else
        {
            GunAnimator.SetTrigger("Fire");
            FireParticle.Play();
            ShakeImpulse.GenerateImpulse();
            FireSound.Play();
        }
    }
    
    [ContextMenu("My Custom Context Menu Option")]
    public void PerformCinematic(Vector3 pos,Transform t, RaycastHit hit,EnemyController enemy)
    {
        SniperController.canAim = false;
        SlowMoFire.Play();
        GunAnimator.SetBool("CanReload", false);
        GameObject g = Instantiate(BuletPrefab);
        g.transform.position = BulletPoint.position;
        g.transform.DOMove(pos, 5).SetEase(Ease.InQuad).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
        {
            Destroy(g);
            Vector3 direction = hit.point - transform.position;
            enemy.OnHit(direction, hit.collider.GetComponent<Rigidbody>());
            Instantiate(BloodParticle, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
            KillCams Cams = Instantiate(KillCam, t);
            temp = Cams.gameObject;
            Cams.InIt();
            SetEffectState(true);
            StartCoroutine(Normalize_Delay());
        });
        Time.timeScale = 0.2f;
    }
    void SetEffectState(bool state)
    {
        Bloom bloom;
        if (globalVolume.profile.TryGet(out bloom))
        {
            bloom.active = state;
        }
        ColorAdjustments colorAdjustments;
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.active = state;
        }
    }
    IEnumerator Normalize_Delay()
    {
        yield return new WaitForSecondsRealtime(CamDuration);
        GunAnimator.SetBool("CanReload", true);
        Time.timeScale = 1f;
        Destroy(temp);
        SetEffectState(false);
    }
    void ReloadComplete()
    {
        _canFire = true;
        SniperController.canAim = true;
    }
    public void PlayReload()
    {
        Reload.Play();
    }

}
