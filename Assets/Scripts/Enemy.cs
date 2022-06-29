using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _points;

    private Player _player;
    private Animator _anim;
    private BoxCollider2D _boxCollider2D;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.Log("The Player is NULL!");

        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.Log("The Animator is NULL!");


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

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;

            if (_boxCollider2D != null)
                _boxCollider2D.enabled = false;
            
            Destroy(gameObject, 2.8f);
        }

        if (collision.gameObject.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);

            if (_player != null)
                _player.AddScore(_points);

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;

            if (_boxCollider2D != null)
                _boxCollider2D.enabled = false;

            Destroy(gameObject, 2.8f);
        }
    }
}