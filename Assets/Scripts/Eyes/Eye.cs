using Following;
using SaveSystem;
using System.Collections;
using UnityEngine;

namespace Eyes
{
    public sealed class Eye : Follower, IFollowable
    {
        public Vector3 RandomPoint;

        private float f_maxHorizontalConstraint = 62f;
        private float f_minHorizontalConstraint = -62f;

        private float f_maxVerticalConstraint = 50f;
        private float f_minVerticalConstraint = -70f;

        private void Awake()
        {
            if (_saveData is not null)
            {
                transform.localRotation = _saveData.CharacterTransform.EyeRotation;
            }

            _initialRotation = Quaternion.identity;
        }

        private void Start()
        {
            StartCoroutine(LookAtTargetCo());
        }

        private IEnumerator LookAtTargetCo()
        {
            yield return new WaitWhile(() =>
            {
                LookAtTarget();
                return _isFollowingForTarget;
            });
        }

        public override void LookAtTarget(Transform target = null)
        {
            base.LookAtTarget(target);

            CheckConstraints(_relativePos);

            transform.rotation = _targetRotation;
        }

        public void CheckConstraints(Vector3 relPos)
        {
            // Vertical (rotation around transform.up (Y) vector)
            var verticallAngle = Vector3.SignedAngle(transform.up, transform.parent.up, relPos.normalized);
            if (verticallAngle > f_maxVerticalConstraint || verticallAngle < f_minVerticalConstraint)
            {
                RaiseBoundsIsReached(_target);
            }

            // Horizontal (rotation around transform.right (X) vector)
            var horizontalAngle = Vector3.SignedAngle(transform.right, transform.parent.right, relPos.normalized);
            if (horizontalAngle > f_maxHorizontalConstraint || horizontalAngle < f_minHorizontalConstraint)
            {
                RaiseBoundsIsReached(_target);
            }
        }

        private IEnumerator LookAtRandomPointCo()
        {
            yield return new WaitWhile(() =>
            {
                LookAtRandomPoint();
                return !_isFollowingForTarget;
            });
        }

        public void LookAtRandomPoint()
        {
            var relPos = transform.position - RandomPoint;
            Quaternion rotation = Quaternion.LookRotation(relPos.normalized);

            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        }

        public override void ToggleFollowingLogic()
        {
            _isFollowingForTarget = !_isFollowingForTarget;
            StopAllCoroutines();
            
            _targetRotation = Quaternion.Inverse(_initialRotation);
            transform.rotation = _targetRotation;

            if (_isFollowingForTarget)
            {
                StartCoroutine(LookAtTargetCo());
            }
            else
            {
                StartCoroutine(LookAtRandomPointCo());
            }
        }

        public override void SetData(ISaver saver)
        {
            saver.Data.CharacterTransform.EyeRotation = transform.localRotation;
        }

        public override void SaveIsLoaded(ISaver saver)
        {
            base.SaveIsLoaded(saver);
            transform.localRotation = saver.Data.CharacterTransform.EyeRotation;

            StopCoroutine(SmoothRotation());
            _isRotationInProgress = false;
            _initialRotation = Quaternion.Euler(transform.TransformVector(transform.localRotation.eulerAngles));
            _targetRotation = _initialRotation;
            StartCoroutine(SmoothRotation());
        }

        private void OnDrawGizmos()
        {
            if (_target == null) return;

            Gizmos.DrawLine(transform.position, _target.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, RandomPoint);

            Gizmos.color = Color.white;
        }
    }
}
