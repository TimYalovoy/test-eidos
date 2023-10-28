using Following;
using System.Collections;
using UnityEngine;

namespace Body
{
    public class Body : MonoBehaviour, IFollowable
    {
        public float RotationSpeed { get; set; } = 16f;
        
        private Transform _target;
        private Quaternion _initialRotation;
        private Quaternion _targetRotation;

        private bool _isRotationInProgress = false;

        void Start()
        {
            _initialRotation = transform.rotation;
        }

        void FixedUpdate()
        {

        }

        public void LookAtTarget(Transform target)
        {
            Vector3 targetDirection = transform.position - target.position;
            targetDirection.y = 0f;

            _targetRotation = Quaternion.LookRotation(targetDirection);

            if (!_isRotationInProgress)
                StartCoroutine(SmoothRotation());
        }

        // smooth rotation to target rotation
        private IEnumerator SmoothRotation()
        {
            if (_isRotationInProgress) yield break;
            _isRotationInProgress = true;

            yield return new WaitWhile(() =>
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, RotationSpeed * Time.fixedDeltaTime);
                return _targetRotation != transform.rotation;
            });

            _isRotationInProgress = false;
        }

        public void FollowFor(Transform target)
        {
            _target = target;
        }

        public void ToggleFollowingLogic()
        {
            StopCoroutine(SmoothRotation());
            _targetRotation = _initialRotation;
            _isRotationInProgress = false;

            StartCoroutine(SmoothRotation());
        }
    }
}
