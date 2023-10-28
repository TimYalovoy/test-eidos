using Moving;
using Target;
using UnityEngine;

namespace UserInput
{
    public class MovableTarget : MonoBehaviour, IMovable
    {
        [SerializeField] private Vector3 minBounds = Vector3.zero;
        [SerializeField] private Vector3 maxBounds = Vector3.zero;
        
        private float _speed = 0.2f;

        private void Awake()
        {

        }

        private void OnEnable()
        {

        }

        private void Start()
        {

        }

        private void Update()
        {
            Move();
        }
        public void Move()
        {
            var newPosition = Vector3.zero;
            if (Input.GetKey(KeyCode.A))
            {
                newPosition = new Vector3(newPosition.x - GetDelta(), newPosition.y, newPosition.z);
            }
            if (Input.GetKey(KeyCode.D))
            {
                newPosition = new Vector3(newPosition.x + GetDelta(), newPosition.y, newPosition.z);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                newPosition = new Vector3(newPosition.x, newPosition.y + GetDelta(), newPosition.z);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                newPosition = new Vector3(newPosition.x, newPosition.y - GetDelta(), newPosition.z);
            }
            if (Input.GetKey(KeyCode.W))
            {
                newPosition = new Vector3(newPosition.x, newPosition.y, newPosition.z + GetDelta());
            }
            if (Input.GetKey(KeyCode.S))
            {
                newPosition = new Vector3(newPosition.x, newPosition.y, newPosition.z - GetDelta());
            }

            transform.position += newPosition;
            CheckPositionForOutOfBounds();
        }

        private float GetDelta() => Time.fixedDeltaTime * _speed;

        private void CheckPositionForOutOfBounds()
        {
            if (transform.position.x < minBounds.x)
            {
                transform.position = new Vector3(minBounds.x, transform.position.y, transform.position.z);
            }
            if (transform.position.x > maxBounds.x)
            {
                transform.position = new Vector3(maxBounds.x, transform.position.y, transform.position.z);
            }

            if (transform.position.y < minBounds.y)
            {
                transform.position = new Vector3(transform.position.x, minBounds.y, transform.position.z);
            }
            if (transform.position.y > maxBounds.y)
            {
                transform.position = new Vector3(transform.position.x, maxBounds.y, transform.position.z);
            }

            if (transform.position.z < minBounds.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, minBounds.z);
            }
            if (transform.position.z > maxBounds.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, maxBounds.z);
            }
        }
    }
}
