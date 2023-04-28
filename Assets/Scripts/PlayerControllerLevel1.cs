using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevel1 : MonoBehaviour
{
	public float moveSpeed=3.5f;
	public float jumpForce = 8f;
	private Rigidbody2D rigidBody;
	public LayerMask groundLayer;
	public Animator animator;
	private bool isWalking = false;
	private bool isFacingRight = true;

	public SpriteRenderer exitSpriteRenderer;

	private Vector2 startPosition;
	private float killOffset = 0.2f;
	public int score = 0;
	
	public AudioClip gemSound;
	private AudioSource source;
	
	void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		startPosition = this.transform.position;
		source = GetComponent<AudioSource>();
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	
	// in this version of PlayerController the player can choose to walk right, left or jump depending on a key clicked
	void Update()
	{
		if (GameManager.instance.currentGameState == GameManager.GameState.GS_GAME)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W))
                Jump();
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				MoveRight();
				isWalking = true;
				if (!isFacingRight)
					Flip();
			}
			else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				MoveLeft();
				isWalking = true;
				if (isFacingRight)
					Flip();
			}
			else
			{
				if (rigidBody.velocity.x > 0.01f)
					rigidBody.velocity = new Vector2(0.95f * rigidBody.velocity.x, rigidBody.velocity.y);
				else if (rigidBody.velocity.x < 0.01f)
					rigidBody.velocity = new Vector2(0.95f * rigidBody.velocity.x, rigidBody.velocity.y);
				else
					rigidBody.velocity = new Vector2(0f, 0f);
				isWalking = false;
			}
			animator.SetBool("isGrounded", isGrounded());
			animator.SetBool("isWalking", isWalking);
		}
    }
	
	// is the player standing on a ground layer or a platform layer
	bool isGrounded()
	{
		return Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, groundLayer.value);
	}
	
	// jump after clicking W or mouse button
	void Jump()
	{
		if (isGrounded())
			rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}

	// flip the character to face its moving direction
	void Flip()
    {
		isFacingRight = !isFacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

	// collisions with gems, enemies or a house
	void OnTriggerEnter2D(Collider2D other)
    {
		if (other.CompareTag("Gem"))
        {
			GameManager.instance.addGems(1);
			source.PlayOneShot(gemSound, AudioListener.volume);
			other.gameObject.SetActive(false);
        }
		else if (other.CompareTag("Enemy"))
        {
			if (other.gameObject.transform.position.y + killOffset < this.transform.position.y)
			{
				GameManager.instance.addEnemies(1);
				rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			} 
			else 
			{
				score = 0;
				Debug.Log("Player’s hurt"); 
				this.transform.position = startPosition; 
			}
		}
		else if (other.CompareTag("House"))
		{
			GameManager.instance.LevelCompleted();
		}
	}
	
	// go right after clicking right arrow or D
	void MoveRight() 
	{
		if (rigidBody.velocity.x < moveSpeed) 
		{ 
			rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y); 
			rigidBody.AddForce(Vector2.right * 0.1f, ForceMode2D.Impulse); 
		}
	}

	// go left after clicking left arrow or A
	void MoveLeft()
	{
		if (rigidBody.velocity.x > -moveSpeed)
		{
			rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);
			rigidBody.AddForce(Vector2.right * 0.1f, ForceMode2D.Impulse);
		}
	}
}
