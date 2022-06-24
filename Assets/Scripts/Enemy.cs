using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4.5f) 
        {
            var xRandomPos = Random.Range(9.5f, -9.5f);
            var yPos = 7.0f;
            transform.position = new Vector3(xRandomPos, yPos, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.transform.GetComponent<Player>();

            if (player != null)
                player.Damage();

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}