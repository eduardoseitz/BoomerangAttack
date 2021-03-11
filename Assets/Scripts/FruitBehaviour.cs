using UnityEngine;

public class FruitBehaviour : MonoBehaviour
{
    [SerializeField] float dyingDelay = 1f;
    [SerializeField] Animator animator;
    [SerializeField] string idleAnimationName;
    [SerializeField] float idleStartMaxDelay = 1f;
    [SerializeField] string dyingAnimationName;
    [SerializeField] float fallKillDistance = 1f;
    [SerializeField] AudioSource dyingAudioSource;
    private Vector3 _startingPosition;
    private bool _isAlive = true;
    private bool _hasFallen = false;
    private float _idleStartRandomDelay;

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;

        _idleStartRandomDelay = Random.Range(0, idleStartMaxDelay);
        Invoke(nameof(StartIdleAnimation), _idleStartRandomDelay);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_hasFallen == false)
        {
            // Check if fruit has fallen
            if (transform.position.y < _startingPosition.y - fallKillDistance)
            {
                _hasFallen = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If fruit has fallen on the ground
        if (_isAlive && _hasFallen)
        {
            Invoke(nameof(Die), dyingDelay);
            _isAlive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAlive)
        {
            // Check if a block is hitting the fruit head
            _isAlive = false;
            Invoke(nameof(Die), dyingDelay);
        }
    }

    private void StartIdleAnimation()
    {
        animator.Play(idleAnimationName);
    }

    private void Die()
    {
        // Change animation
        animator.Play(dyingAnimationName);

        // Play sound
        dyingAudioSource.Play();

        // Update game situation
        GameManager.instance.KillFruit();

        Debug.Log($"Fruit {gameObject.name} is dead OwO");
    }
}
