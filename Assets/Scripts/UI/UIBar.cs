using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    Image mask;
    float originalSize;
    [SerializeField] float speed = 1f;
    float currValue;
    float newValue;

    private void Awake() {
        mask = GetComponent<Image>();
    }
    private void Start() {
        originalSize = mask.rectTransform.rect.width;
        currValue = 1;
        newValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (currValue > newValue) {
            currValue -= speed * Time.unscaledDeltaTime;
            if (currValue < newValue)
                currValue = newValue;
        }
        else if (currValue < newValue) {
            currValue += speed * Time.unscaledDeltaTime;
            if (currValue > newValue)
                currValue = newValue;
        }
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currValue * originalSize);
    }

    public void SetCurrHealth(float value) {
        newValue = value;
    }
}
