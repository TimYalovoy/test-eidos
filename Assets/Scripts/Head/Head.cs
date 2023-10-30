using Eyes;
using Following;
using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace Head
{
    public class Head : MonoBehaviour, IFollowable
    {
        private const float f_minHorizontalConstraint = 90f;
        private const float f_maxHorizontalConstraint = -90f;

        private const float f_maxVerticalConstraint = 70f;
        private const float f_minVerticalConstraint = -65f;

        public event Action<Transform> BoundsIsReached;

        public float RotationSpeed { get; set; } = 24f;

        private Transform _target;
        private Quaternion _initialRotation;
        private Quaternion _targetRotation;

        private bool _isRotationInProgress = false;

        void Start()
        {
            _initialRotation = transform.rotation;
        }

        void Update()
        {

        }
        public void LookAtTarget(Transform target)
        {
            var relativePos = transform.position - target.position;

            _targetRotation = Quaternion.LookRotation(relativePos);
            
            if (!_isRotationInProgress)
                StartCoroutine(SmoothRotation());

            var verticalAngle = GetSignedAngle(relativePos.normalized, transform.up, transform.parent.up);
            if (verticalAngle > f_maxVerticalConstraint)
            {
                BoundsIsReached.Invoke(target);
            }
            if (verticalAngle < f_minVerticalConstraint)
            {
                BoundsIsReached.Invoke(target);
            }

            var horizontalAngle = GetSignedAngle(relativePos.normalized, transform.right, transform.parent.right);
            if (horizontalAngle > f_maxHorizontalConstraint)
            {
                BoundsIsReached.Invoke(target);
            }
            if (horizontalAngle < f_minHorizontalConstraint)
            {
                BoundsIsReached.Invoke(target);
            }

        }

        private float GetSignedAngle(Vector3 normal, Vector3 selfAxis, Vector3 parentAxis)
        {
            var angle = Vector3.Angle(selfAxis, parentAxis);
            var cross = Vector3.Cross(selfAxis, parentAxis);
            var dot = Vector3.Dot(normal, cross);
            var sign = Mathf.Sign(dot);
            var signedAngle = angle * sign;
            Debug.Log($"signedAngle: {signedAngle} = {angle} * {sign}");
            return signedAngle;
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

        public void ToggleFollowingLogic()
        {
            StopCoroutine(SmoothRotation());
            _targetRotation = _initialRotation;
            _isRotationInProgress = false;

            StartCoroutine(SmoothRotation());
        }

        public void FollowFor(Transform target)
        {
            _target = target;
        }
    }
}
