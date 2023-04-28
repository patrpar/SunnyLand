using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float XMin = 3f;
    public float XMax = 3f;
    public float moveSpeed = 2f;
    public Animator animator;

    public float killOffset = 0.2f;
    private float startPositionX;
    private bool isMovingRight = false;
    private Rigidbody2D rigidBody;
    private bool isFacingRight = false;
    // Start is called before the first frame update
    void Awake()
    {
        startPositionX = this.transform.position.x;
        this.transform.position = new Vector2(Random.Range(startPositionX - XMin, startPositionX + XMax), this.transform.position.y);
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    
    // the enemy is constantly moving, switching to left/right after reaching maximum position
    void Update()
    {
        if (isMovingRight)
        {
            if (this.transform.position.x < startPositionX + XMax)
            { 
                MoveRight();
            } 
            else 
            { 
                isMovingRight = false; 
                MoveLeft();
                Flip();
            } 
        } 
        else 
        { 
            if (this.transform.position.x > startPositionX - XMin) 
            { 
                MoveLeft();
            } 
            else 
            { 
                isMovingRight = true; 
                MoveRight();
                Flip();
            } 
        }
    }

    // go right
    void MoveRight()
    {
        if (rigidBody.velocity.x < moveSpeed)
        {
            rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.right * 0.1f, ForceMode2D.Impulse);
        }
    }

    // go left
    void MoveLeft()
    {
        if (rigidBody.velocity.x > -moveSpeed)
        {
            rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.right * 0.1f, ForceMode2D.Impulse);
        }
    }

    // flip the character to face its moving direction
    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator KillOnAnimationEnd() 
    { 
        yield return new WaitForSeconds(0.7f); 
        this.gameObject.SetActive(false); 
    }

    // collision with the player, delete the enemy if player jumped on it
    private void OnTriggerEnter2D(Collider2D other) 
    { 
        if (other.CompareTag("Player")) 
        { 
            if (other.gameObject.transform.position.y > this.transform.position.y + killOffset)
            { 
                //Debug.Log("Enemy is dead"); 
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            } 
        } 
    }
}
