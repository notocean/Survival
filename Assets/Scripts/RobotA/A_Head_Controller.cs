using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Head_Controller : MonoBehaviour
{
    [SerializeField] GameObject shootPositionObject;
    [SerializeField] GameObject smokeLaunchPrefab;
    [SerializeField] GameObject rocketPrefab;
    Animator anim;
    private void Awake() {
        anim = GetComponent<Animator>();
    }
    public void Launch() {
        anim.SetTrigger("a_Idle");
        Vector2 shootPosition = shootPositionObject.transform.position;
        Instantiate(smokeLaunchPrefab, shootPositionObject.transform);
        Instantiate(rocketPrefab, shootPosition, Quaternion.identity);
    }
}
