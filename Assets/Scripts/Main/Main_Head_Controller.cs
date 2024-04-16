using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Main_Head_Controller : MonoBehaviour
{
    Animator animHead;
    float horHead, verHead;
    float force = 1000;
    Vector2 mainPos;
    Main_Infor mainInfor;
    AudioSource audioSource;
    float timer;

    [SerializeField] GameObject shootVisualObject;
    [SerializeField] GameObject shootPositionObject;
    Light2D shootvisual;
    [SerializeField] GameObject projectilePrefab;
    public enum stateDir {
        idle0, idle1, rotate0, rotate1, up, upleft, left, downleft, down, downright, right, upright
    }
    stateDir stateHead;
    // Start is called before the first frame update
    void Awake()
    {
        animHead = GetComponent<Animator>();
        mainInfor = gameObject.GetComponentInParent<Main_Infor>();
        shootvisual = shootVisualObject.GetComponent<Light2D>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start() {
        horHead = 0;
        verHead = -1;
        timer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            return;

        if (mainInfor.infor.isDestroy)
            return;

        mainPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 moveDir = (mousePos - mainPos).normalized;
        float inputHorHead = moveDir.x;
        float inputVerHead = moveDir.y;

        if (Mathf.Abs(inputHorHead) > 0 || Mathf.Abs(inputVerHead) > 0) {
            horHead = inputHorHead;
            verHead = inputVerHead;
        }
        animHead.SetFloat("horHead", horHead);
        animHead.SetFloat("verHead", verHead);

        if (Input.GetMouseButtonDown(0)) {
            Vector2 shootDir = new Vector2(horHead, verHead).normalized;
            Shoot(shootDir);
        }

        if (audioSource.isPlaying)
            timer += Time.unscaledDeltaTime;
        if (timer > 0.25f) {
            timer = 0;
            audioSource.Stop();
        }
    }
    void Shoot(Vector2 shootDir) {
        if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            return;

        if (mainInfor.energy >= 1) {
            audioSource.Play();
            GameObject projectileGO = Instantiate(projectilePrefab, shootPositionObject.transform.position, Quaternion.identity);
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.Shoot(shootDir, force);
            projectile.damage = mainInfor.infor.damage;
            mainInfor.ChangeEnergy(-1);
        }
    }
}
