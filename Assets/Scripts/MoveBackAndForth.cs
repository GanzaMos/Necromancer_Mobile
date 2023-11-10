using UnityEngine;


public class MoveBackAndForth : MonoBehaviour
{
    public float speed = 0.05f;      // Movement speed
    public float timePeriod = 3;
    
    float _timer = 0;

    bool _firstIteration = true;

    Rigidbody _rb;
    Vector3 _initialPosition;
    
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPosition = transform.position;
        _rb.velocity = new Vector3(speed, 0f, 0f);  // Initial movement to the right
        timePeriod = timePeriod / 2;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= timePeriod)
        {
            if (_firstIteration)
            {
                _firstIteration = !_firstIteration;
                timePeriod *= 2;
            }

            _timer = 0;
            _rb.velocity = new Vector3(-_rb.velocity.x, 0f, 0f);
        }
    }
}