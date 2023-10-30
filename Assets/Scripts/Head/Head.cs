using Following;
using SaveSystem;
using UnityEngine;

namespace Head
{
    public sealed class Head : Follower, IFollowable
    {
        private const float f_minHorizontalConstraint = 90f;
        private const float f_maxHorizontalConstraint = -90f;

        private const float f_maxVerticalConstraint = 70f;
        private const float f_minVerticalConstraint = -65f;

        private void Awake()
        {
            if (_saveData is not null)
            {
                transform.localRotation = _saveData.CharacterTransform.EyeRotation;
            }

            _initialRotation = transform.rotation;
        }

        public override void LookAtTarget(Transform target)
        {
            base.LookAtTarget(target);
            
            if (!_isRotationInProgress)
                StartCoroutine(SmoothRotation());

            var verticalAngle = Vector3.SignedAngle(transform.up, transform.parent.up, _relativePos.normalized);
            if (verticalAngle > f_maxVerticalConstraint)
            {
                RaiseBoundsIsReached(target);
            }
            if (verticalAngle < f_minVerticalConstraint)
            {
                RaiseBoundsIsReached(target);
            }

            var horizontalAngle = Vector3.SignedAngle(transform.right, transform.parent.right, _relativePos.normalized);
            if (horizontalAngle > f_maxHorizontalConstraint)
            {
                RaiseBoundsIsReached(target);
            }
            if (horizontalAngle < f_minHorizontalConstraint)
            {
                RaiseBoundsIsReached(target);
            }
        }

        public override void ToggleFollowingLogic()
        {
            _isFollowingForTarget = !_isFollowingForTarget;
            StopAllCoroutines();

            _targetRotation = Quaternion.Inverse(Quaternion.identity);
            _isRotationInProgress = false;

            StartCoroutine(SmoothRotation());
        }

        public override void SetData(ISaver saver)
        {
            saver.Data.CharacterTransform.HeadRotation = transform.localRotation;
        }

        public override void SaveIsLoaded(ISaver saver)
        {
            base.SaveIsLoaded(saver);
            transform.localRotation = saver.Data.CharacterTransform.HeadRotation;

            StopCoroutine(SmoothRotation());
            _isRotationInProgress = false;
            _initialRotation = Quaternion.Euler(transform.TransformVector(transform.localRotation.eulerAngles));
            _targetRotation = _initialRotation;

            StartCoroutine(SmoothRotation());
        }
    }
}
