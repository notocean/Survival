using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb2d;
    [SerializeField] float maxDistanceLife = 100f;
    public int damage;
    // Start is called before the first frame update
    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rb2d.position.magnitude > maxDistanceLife) {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector2 shootDir, float force) {
        float angle = Vector2.Angle(shootDir, Vector2.down);
        if (shootDir.x < 0) {
            angle = 360 - angle;
        }
        transform.Rotate(Vector3.forward, angle);
        rb2d.AddForce(shootDir * force);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Robot robot = collision.gameObject.GetComponent<Robot>();
        if (robot != null) {
            robot.ChangeHealth(-damage);
        }
        Destroy(gameObject);
    }
}
