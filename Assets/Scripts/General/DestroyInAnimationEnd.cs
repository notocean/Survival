using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInAnimationEnd : MonoBehaviour
{
    public void Destroy() {
        Destroy(gameObject);
    }

    private void OnDestroy() {

    }
}
