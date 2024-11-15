using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D bcollider;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bcollider = GetComponent<BoxCollider2D>();
        Invoke("Think", 5);
    }

    // Update is called once per frame
    void FixedUpdate()              
    {
        //move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        //platform check
        Vector2 frontVec = new Vector2(rigid.position.x+nextMove*0.2f,rigid.position.y);
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            turn();

        }
    }
    void Think()
    {
        //set next active
        nextMove = Random.Range(-1, 2);
        //sprite animation
        anim.SetInteger("WalkSpeed", nextMove);

        //flip sprite
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        //recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
        
        
    }
    void turn()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 2);
    }
    public void OnDamaged()
    {
        //sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //sprite flip y
        spriteRenderer.flipY = true;
        //collider disable
        bcollider.enabled = false;
        //die effect jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //destroy
        Invoke("DeActive", 5);
    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }
    
}
