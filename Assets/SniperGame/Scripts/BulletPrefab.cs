using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletPrefab : MonoBehaviour
{
    public Transform BulletTransform;

    private void Awake()
    {
        RotateBullet();
    }

    public void RotateBullet()
    {
        BulletTransform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetUpdate(UpdateType.Normal, true);
    }

}
