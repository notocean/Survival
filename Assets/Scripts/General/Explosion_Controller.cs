using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Controller : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision) {
        Main_Infor main = collision.gameObject.GetComponent<Main_Infor>();
        if (main != null) {
            main.DecreaseHealth(damage);
        }
    }
}
