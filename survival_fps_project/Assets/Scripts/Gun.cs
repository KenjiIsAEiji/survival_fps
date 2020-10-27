using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public virtual void Shoot()
    {
        Debug.Log("GunShoot");
    }
}
