using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class AsteroidControl : MonoBehaviour
{
	public string size;
	public int speed;
	public float rotationSpeed;
	public float top;
	public float right;
	public Rigidbody2D rb;
	public ScreenWrap sw;
	public GameManager gm;
	public PolygonCollider2D pc;
	public GameObject mediumAsteroid;
	public GameObject smallAsteroid;
	public GameObject explosion;
	
    void Start()
    {	
        rb = GetComponent<Rigidbody2D>();
		sw = GetComponent<ScreenWrap>();
		gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
		pc = GetComponent<PolygonCollider2D>();

		rb.AddForce(Random.insideUnitCircle * speed);
		rb.AddTorque(rotationSpeed);
    }

    void Update()
    {
		if(sw.CheckForWrapping(transform.position, top, right))
			transform.position = sw.wrappedPosition;
    }
	
	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(other.gameObject);
		Split();
	}

	void ExplosionEffect()
	{
		var explode = Instantiate(explosion, transform.position, transform.rotation);
		Destroy(explode, 2.0f);
	}
	
	void Split()
	{
		Vector2 location = new Vector2(transform.position.x, transform.position.y);
		switch (size)
		{
			case "Large":	
				ExplosionEffect();
				Instantiate(mediumAsteroid, new(location.x + 0.8f, location.y + 0.8f), transform.rotation);
				Instantiate(mediumAsteroid, location, Quaternion.identity);
				Destroy(gameObject);
				gm.SendMessage("UpdateScore", 50);
				break;
			case "Medium":
				ExplosionEffect();
				Instantiate(smallAsteroid, new(location.x + 0.35f, location.y + 0.35f), transform.rotation);
				Instantiate(smallAsteroid, location, Quaternion.identity);
				Destroy(gameObject);
				gm.SendMessage("UpdateScore", 100);
				break;
			case "Small":
				ExplosionEffect();
				Destroy(gameObject);
				gm.SendMessage("UpdateScore", 200);
				break;
		}
	}

	void OnCollisionEnter2D(Collision2D collisionInfo)
	{
		if(collisionInfo.gameObject.tag == "Asteroid")
		{
			rb.AddForce(rb.totalForce * 1.1f);
		}	
	}
}
