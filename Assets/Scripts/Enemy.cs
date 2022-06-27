using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _points;

    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.Log("The Player is NULL!");
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
            if (_player != null)
                _player.Damage();

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);

            if (_player != null)
                _player.AddScore(_points);

            Destroy(gameObject);
        }
    }
}