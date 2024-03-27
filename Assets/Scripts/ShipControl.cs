using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
	private float thrustInput;
	private float turnInput;
	public int speed;
	public int turnSpeed;
	public int bulletSpeed;
	public GameObject bullet;
	public Transform gun;
	public int lives;
	public int damageMultiplier;
	public float ufoBulletDamage;
	private float damage = 0.0f;
	private bool canShoot;
	public float top;
	public float right;
	public Rigidbody2D rb;
	public ScreenWrap sw;
	public GameManager gm;
	public GameObject explosion;
	public GameObject trail;
	public GameObject explosionUfoBullet;
	[SerializeField] private AudioSource sfxCollision;
	[SerializeField] private AudioSource sfxExplosion;
	[SerializeField] private AudioSource sfxWarning;
	[SerializeField] private AudioSource sfxThrusting;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		sw = GetComponent<ScreenWrap>();
		canShoot = true;
		gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
		thrustInput = Input.GetAxis("Vertical");
		turnInput = Input.GetAxis("Horizontal");
		transform.Rotate(Vector3.forward, turnSpeed * -turnInput * Time.deltaTime);

		if (thrustInput != 0)
			trail.SetActive(true);
		else
			trail.SetActive(false);

		if (Input.GetKey(KeyCode.UpArrow) && !sfxThrusting.isPlaying)
			sfxThrusting.Play();
		else if (!Input.GetKey(KeyCode.UpArrow)/* && sfxThrusting.isPlaying*/)
			sfxThrusting.Pause();

		if (sw.CheckForWrapping(transform.position, top, right))
			transform.position = sw.wrappedPosition;
		
		if (Input.GetButtonDown("Jump") && canShoot)
		{
			GameObject firedBullet = Instantiate(bullet, gun.position, gun.rotation);
			firedBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed);
			Destroy(firedBullet, 5.0f);
		}	
    }
	
	void FixedUpdate()
	{
		rb.AddRelativeForce(Vector2.up * thrustInput * speed);
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		float impactForce = other.relativeVelocity.magnitude;
		float impactDamage = impactForce * damageMultiplier;
		damage += impactDamage;
		if (impactDamage <= 25)
			sfxCollision.volume = 0.5f;
		else
			sfxCollision.volume = 1.0f;
		sfxCollision.Play();
		CheckDamage();
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("UFO Bullet"))
		{
			var bulletExplode = Instantiate(explosionUfoBullet, other.transform.position, other.transform.rotation);
			Destroy(bulletExplode, 2.0f);
			damage += ufoBulletDamage;
			Destroy(other.gameObject);
			CheckDamage();
		}		
	}
	
	void CheckDamage()
	{
		if (damage >= 80 && damage <= 99)
			sfxWarning.Play();
		if (damage >= 100.0f)
		{
			var explode = Instantiate(explosion, transform.position, transform.rotation);
			Destroy(explode, 2.0f);
			sfxExplosion.Play();
			lives--;
			rb.velocity = Vector2.zero;
			rb.angularVelocity = 0.0f;
			transform.position = new Vector2(10.0f, 6.0f); // to tell ufo to go away from center
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<PolygonCollider2D>().enabled = false;
			canShoot = false;
			if (lives == 0)
			{
				damage = 100.0f;
				gm.SendMessage("GameOver");
			}
			else
				Invoke("Respawn", 2.0f);
		}
		gm.SendMessage("UpdateLives", lives);
		gm.SendMessage("UpdateDamage", (int)damage);
	}
	
	void Respawn()
	{
		damage = 0.0f;
		transform.position = Vector2.zero;
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0.2f);
		trail.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0.2f);
		Invoke("EndSpawnProtection", 3.0f);
		gm.SendMessage("UpdateDamage", (int)damage);
	}
	
	void EndSpawnProtection()
	{
		GetComponent<PolygonCollider2D>().enabled = true;
		GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1.0f);
		trail.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1.0f);
		canShoot = true;
	}
}
