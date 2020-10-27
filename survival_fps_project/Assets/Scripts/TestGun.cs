using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : Gun
{
    public override void Shoot()
    {
        Debug.Log("Test Gun fire!");
    }
}
