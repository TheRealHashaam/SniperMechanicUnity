using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<Rigidbody> rigidbodies;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        DisableRagdoll();
    }
    public void EnableRagdoll()
    {
        animator.enabled = false;
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        Invoke("Destroy", 1);
    }
    public void DisableRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void OnHit(Vector3 direction, Rigidbody Hitpoint)
    {
        EnableRagdoll();
        Hitpoint.AddForce(direction.normalized * 20, ForceMode.Impulse);
    }

    void Destroy()
    {
        Destroy(this);    
    }

}
