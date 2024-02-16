using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay;
    private void OnEnable()
    {
        Destroy(this.gameObject, delay);
    }
}
