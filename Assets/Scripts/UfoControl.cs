using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoControl : MonoBehaviour
{
	private Vector2 direction;
	public float speed;
	public Rigidbody2D rb;
	private Transform transformShip;
	public GameObject ufoBullet;
	public GameManager gm;
	public int bulletSpeed;
	public int hitPoints;
	public bool canShoot = true;
	private int timesHit;
	public GameObject explosion;
	[SerializeField] private AudioSource sfxCollision;
	public AudioSource sfxUfoEngine;
	public GameObject explosionShipBullet;
	
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		transformShip = GameObject.FindWithTag("Player").transform;
		gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
		InvokeRepeating("Shoot", 5.0f, 5.0f);
		timesHit = 0;
		sfxUfoEngine.Play();
    }

    void Update()
    {
		if (timesHit >= hitPoints)
		{
			var explode = Instantiate(explosion, transform.position, transform.rotation);
			Destroy(explode, 2.0f);
			gameObject.SetActive(false);
			canShoot = false;
			sfxUfoEngine.Pause();
			timesHit = 0;
            transform.position = new Vector2(Random.Range(-10.0f, 10.0f), 8.0f);
			gm.SendMessage("UpdateScore", 500);
		}		
    }
	
	void FixedUpdate()
	{
		direction = (transformShip.position - transform.position).normalized;
		rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
	}
	
	void Shoot()
	{
		if (canShoot)
		{
			GameObject firedUfoBullet = Instantiate(ufoBullet, transform.position, transform.rotation);
			firedUfoBullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);
			Destroy(firedUfoBullet, 10.0f);
		}	
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		timesHit += 2;
		if (other.gameObject.tag == "Asteroid")
			sfxCollision.Play();
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Bullet"))
		{
			timesHit++;
			Destroy(other.gameObject);
			var bulletExplode = Instantiate(explosionShipBullet, other.transform.position, other.transform.rotation);
			Destroy(bulletExplode, 2.0f);
		}
	}
}
