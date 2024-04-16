using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    public int maxHealth { get; private set; }
    public float speed { get; private set; }
    public int damage { get; set; }
    public int currHealth { get; private set; }
    public Vector2 pos { get; private set; }
    public bool isDestroy {
        get {
            return currHealth <= 0;
        }
    }
    public UIBar healthBar { get; private set; }
    public void Init(int maxHealth, float speed, int damage, UIBar healthBar) {
        this.maxHealth = maxHealth;
        this.speed = speed;
        this.damage = damage;
        this.healthBar = healthBar;
        currHealth = maxHealth;
    }

    public bool ChangeHealth(int amount) {
        if (amount > 0) {
            if (currHealth != maxHealth) {
                currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth);
                healthBar.SetCurrHealth((float)currHealth / maxHealth);
                return true;
            }
            else return false;
        }
        currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth);
        healthBar.SetCurrHealth((float)currHealth / maxHealth);
        return true;
    }
}
