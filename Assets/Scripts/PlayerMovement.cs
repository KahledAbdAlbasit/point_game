using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
    Rigidbody2D rb;

    public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
    bool isJump = false;
    bool crouch = false;
	public int score;
	int maxHelth=10;
    int plyarHelth = 6;
	public Transform holder;
	TextMeshProUGUI healthText;
	TextMeshProUGUI scoreText;
    TextMeshProUGUI points;

    public Transform collectedSound;
    void Start() {
		score = 0;
		plyarHelth = maxHelth;
		//HelthText.text = "kahled";

		healthText = holder.Find("helthText").GetComponent<TextMeshProUGUI>();
        scoreText = holder.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        //points = holder.Find("points").GetComponent<TextMeshProUGUI>();

        scoreText.text = "SCORE : "+score;
		healthText.text = plyarHelth + "/" + maxHelth;

    }
    // Update is called once per frame
    void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		} else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}

	}

	public void OnLanding ()
	{
		animator.SetBool("IsJumping", false);
	}

	public void OnCrouching (bool isCrouching)
	{
		animator.SetBool("IsCrouching", isCrouching);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
    void SoundOn(Vector3 itemPos)
	{
		Transform obj = Instantiate(collectedSound,itemPos,new Quaternion());	
		obj.gameObject.SetActive(true);
		Destroy(obj.gameObject, obj.GetComponent<AudioSource>().clip.length);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb = GetComponent<Rigidbody2D>();

        if (collision.tag == "Enemy")
        {
            if(isJump&&rb.velocity.y <0)
			{
				Destroy(collision.gameObject);
			}
			else
                plyarHelth = plyarHelth - 1;
            //Debug.Log("Current Helth:" + plyarHelth + "/" + maxHelth);
            healthText.text = plyarHelth + "/" + maxHelth;

        }
        else if(collision.CompareTag("Gem"))
		{
			score +=100;
			scoreText.text = "Score : " + score;
			SoundOn(collision.transform.position);
            Destroy(collision.gameObject);
        }
		else if (collision.CompareTag("cherry"))
        {
            score += 50;
			scoreText.text = "Score : " + score;
            SoundOn(collision.transform.position);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("feed"))
        {
			if (plyarHelth < maxHelth)
				plyarHelth += 2;
			//Debug.Log("Current Helth:" + plyarHelth + "/" + maxHelth);
			healthText.text = plyarHelth + "/" + maxHelth;
            SoundOn(collision.transform.position);
            Destroy(collision.gameObject);

        }
        //Debug.Log(plyarHelth);

        if (plyarHelth <= 0)
        {
            Debug.Log("You Are Died");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("lose Scene");
        }

        if (score >= 3000)
        {
            //Debug.Log("You Are Died");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("Win");
        }
    }

	
}
