using Following;
using SaveSystem;
using System;
using UnityEngine;

namespace Eyes
{
    public sealed class Eye : Follower, IFollowable
    {
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

            _initialRotation = transform.rotation;
        }

        public void FixedUpdate()
        {
            if (_isFollowingForTarget) LookAtTarget();
            else LookAtRandomPoint();
        }

        public override void LookAtTarget(Transform target = null)
        {
            base.LookAtTarget(target);

            // Vertical (rotation around transform.up (Y) vector)
            var verticallAngle = GetSignedAngle(_relativePos.normalized, transform.up, transform.parent.up);
            if (verticallAngle > f_maxVerticalConstraint)
            {
                RaiseBoundsIsReached(_target);
            }
            if (verticallAngle < f_minVerticalConstraint)
            {
                RaiseBoundsIsReached(_target);
            }

            // Horizontal (rotation around transform.right (X) vector)
            var horizontalAngle = GetSignedAngle(_relativePos.normalized, transform.right, transform.parent.right);
            if (horizontalAngle > f_maxHorizontalConstraint)
            {
                RaiseBoundsIsReached(_target);
            }
            if (horizontalAngle < f_minHorizontalConstraint)
            {
                RaiseBoundsIsReached(_target);
            }

            transform.rotation = _targetRotation;
        }

        private void LookAtRandomPoint()
        {
            
        }

        public override void ToggleFollowingLogic()
        {
            _isFollowingForTarget = !_isFollowingForTarget;
            transform.rotation = _initialRotation;
        }

        public override void SetData(ISaver saver)
        {
            saver.Data.CharacterTransform.EyeRotation = transform.localRotation;
        }

        public override void SaveIsLoaded(ISaver saver)
        {
            base.SaveIsLoaded(saver);
            transform.localRotation = saver.Data.CharacterTransform.EyeRotation;
        }

        private void OnDrawGizmos()
        {
            if (_target == null) return;

            Gizmos.DrawLine(transform.position, _target.position);
        }
    }
}
