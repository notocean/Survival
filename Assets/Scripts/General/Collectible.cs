using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    bool isCollecting = false;
    float timeHide = 1.5f;
    float timer = 0;
    SpriteRenderer myRenderer;
    GameObject smoke;
    bool isSmoke = true;
    public enum ItemType {
        Health, Damage
    }
    [SerializeField] ItemType itemType;

    private void Awake() {
        myRenderer = GetComponent<SpriteRenderer>();
        smoke = transform.GetChild(0).gameObject;
    }

    private void Update() {
        if (isCollecting) {
            if (isSmoke) {
                Destroy(smoke);
                isSmoke = false;
            }
            timer += Time.deltaTime;
            Color color = new(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, (timeHide - timer) / timeHide);
            myRenderer.color = color;
            if (timer > timeHide)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isCollecting)
            return;
        Main_Infor main = collision.gameObject.GetComponent<Main_Infor>();
        if (main != null) {
            if (main.GetItem(itemType)) {
                isCollecting = true;
            }
        }
    }
}
