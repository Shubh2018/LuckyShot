using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private void Start()
    {
        Invoke("DestroyThis", 2);
    }
    void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
