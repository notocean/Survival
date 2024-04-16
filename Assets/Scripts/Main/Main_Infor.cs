using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Infor : MonoBehaviour
{
    public Robot infor { get; private set; }
    int maxEnergy = 100;
    public int energy { get; private set; }
    float maxTimeImmunity = 3f;
    float timer = 0;

    Rigidbody2D rb2d;
    Animator lightEffectAnim;
    [SerializeField] UIBar healthBar;
    [SerializeField] UIBar energyBar;
    enum stateEffect {
        noEffect, isAttacked, immunity
    }
    stateEffect effect;
    private void Awake() {
        infor = GetComponent<Robot>();
        infor.Init(LevelManager.Instance.maxHealthM, LevelManager.Instance.speedM, LevelManager.Instance.damageM, healthBar);

        rb2d = GetComponent<Rigidbody2D>();
        lightEffectAnim = transform.GetChild(1).gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        energy = maxEnergy;
        effect = stateEffect.noEffect;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            return;

        if (infor.isDestroy && GameManager.Instance.isPlaying) {
            GameManager.Instance.gameState = GameManager.GameState.Pause;
            NotifyManager.Instance.ShowNotify(NotifyManager.NotifyType.LevelEnd, true);
            GameManager.Instance.isPlaying = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (energy >= 50) {
                ChangeEnergy(-50);
                lightEffectAnim.SetTrigger("immunity");
                effect = stateEffect.immunity;
            }
        }

        if (effect == stateEffect.isAttacked) {
            if (lightEffectAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9) {
                lightEffectAnim.SetTrigger("noEffect");
                effect = stateEffect.noEffect;
            }
        }
        else if (effect == stateEffect.immunity) {
            timer += Time.deltaTime;
            if (timer > maxTimeImmunity) {
                timer = 0;
                lightEffectAnim.SetTrigger("noEffect");
                effect = stateEffect.noEffect;
            }
        }
    }

    public void ChangeEnergy(int amount) {
        energy = Mathf.Clamp(energy + amount, 0, maxEnergy);
        energyBar.SetCurrHealth((float)energy / maxEnergy);
    }

    public void DecreaseHealth(int amount) {
        if (effect != stateEffect.immunity) {
            infor.ChangeHealth(-amount);

            lightEffectAnim.SetTrigger("isAttacked");
            effect = stateEffect.isAttacked;
        }
    }

    public bool GetItem(Collectible.ItemType itemType) {
        switch (itemType) {
            case Collectible.ItemType.Damage:
                infor.damage += (int)(infor.damage * 0.1);
                UIEffectManager.Instance.BuffDamageEffect(transform.position);
                break;
            case Collectible.ItemType.Health:
                if (infor.ChangeHealth(20))
                    return true;
                else return false;
        }
        return true;
    }
}
