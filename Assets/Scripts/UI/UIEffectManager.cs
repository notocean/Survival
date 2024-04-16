using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectManager : MonoBehaviour
{
    [SerializeField] GameObject BuffDamageNotify;
    private static UIEffectManager instance;
    public static UIEffectManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<UIEffectManager>();
                if (instance == null) {
                    GameObject obj = new("EffectCanvas");
                    instance = obj.AddComponent<UIEffectManager>();
                }
            }
            return instance;
        }
    }

    public void BuffDamageEffect(Vector2 pos) {
        Instantiate(BuffDamageNotify, pos, Quaternion.identity, transform);
    }
}
