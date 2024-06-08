using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Rigidbody rigidBody;
    public float speed;

    public GameObject radius;
    public bool isBouns;
    public float bounsPower;
    void Start()
    {
        
    }
    private void Update()
    {
        Bouns();
    }

    private void Bouns()
    {
        if (transform.position.x * transform.position.x + transform.position.z * transform.position.z < (radius.transform.localScale.x / 2) * (radius.transform.localScale.x / 2))
        {
            PlayerMove(-speed);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove(speed);

    }
    private void PlayerMove(float speed)
    {
        float x = Input.GetAxisRaw("Horizontal"); // x方向のキー入力
        float z = Input.GetAxisRaw("Vertical"); // z方向のキー入力
        Vector3 Player_movedir = new Vector3(x, rigidBody.velocity.y, z); // 正規化
        Player_movedir = this.transform.forward * z + this.transform.right * x;
        Player_movedir = Player_movedir.normalized;
        rigidBody.velocity = new Vector3(Player_movedir.x * speed, rigidBody.velocity.y, Player_movedir.z * speed);
    }
}
