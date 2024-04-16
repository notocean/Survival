using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Controller : MonoBehaviour
{
    Robot infor;
    Rigidbody2D rb2d;
    Animator anim;
    Vector2 enemyPos;
    Vector2 pos;
    Vector2 moveDir;
    float speed, speedUp = 2f;
    float hor, ver;
    float disSpeedUp = 5;
    float distanceCanAttack = 2;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject brokenObject;
    UIBar healthBar;
    enum State {
        move, explosion, broken
    }
    State currState;
    // Start is called before the first frame update
    private void Awake() {
        healthBar = GetComponentInChildren<UIBar>();
        infor = GetComponent<Robot>();
        infor.Init(LevelManager.Instance.maxHealthC, LevelManager.Instance.speedC, LevelManager.Instance.damageC, healthBar);


        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        speed = infor.speed;
        hor = 0; ver = 0;
        moveDir = new Vector2(0, 0);
        currState = State.move;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            return;

        GameObject enemyObject = GameObject.FindGameObjectWithTag("Player");
        if (enemyObject != null && (int)currState <= 0) {
            enemyPos = enemyObject.transform.position;
            pos = gameObject.transform.position;
            moveDir = enemyPos - pos;
            if (moveDir.magnitude <= disSpeedUp && speedUp - speed > 0.1) {
                speed *= speedUp;
            }
            if (moveDir.magnitude <= distanceCanAttack) {
                currState = State.explosion;
            }
            moveDir.Normalize();
        }
        else moveDir = new Vector2(0, 0);

        if (infor.isDestroy)
            currState = State.explosion;

        switch (currState) {
            case State.move:
                hor = moveDir.x;
                ver = moveDir.y;
                if (Mathf.Abs(moveDir.x) > 0 || Mathf.Abs(moveDir.y) > 0) {
                    hor = moveDir.x;
                    ver = moveDir.y;
                }
                anim.SetFloat("horBody", hor);
                anim.SetFloat("verBody", ver);
                break;
            case State.explosion:
                anim.SetTrigger("explosion");
                currState = State.broken;
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Main_Infor main = collision.gameObject.GetComponent<Main_Infor>();
        if (main != null) {
            currState = State.explosion;
        }
    }

    private void FixedUpdate() {
        if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            return;

        if (currState != State.move)
            return;
        Vector2 pos = rb2d.position;
        Vector2 dirBody = new Vector2(hor, ver);
        dirBody.Normalize();

        pos.x += speed * dirBody.x * Time.deltaTime;
        pos.y += speed * dirBody.y * Time.deltaTime;
        rb2d.MovePosition(pos);
    }

    public void Explosion() {
        if (LevelManager.Instance.HasGenerateScrapC()) {
            Instantiate(brokenObject, gameObject.transform.position, Quaternion.identity);
        }
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Explosion_Controller explosion_Controller = explosion.GetComponent<Explosion_Controller>();
        explosion_Controller.damage = infor.damage;
        LevelManager.Instance.DestroyC();
        Destroy(gameObject);
    }
}
