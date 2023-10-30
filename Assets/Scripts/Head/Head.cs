using Following;
using SaveSystem;
using System;
using System.Collections;
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

            var verticalAngle = GetSignedAngle(_relativePos.normalized, transform.up, transform.parent.up);
            if (verticalAngle > f_maxVerticalConstraint)
            {
                RaiseBoundsIsReached(target);
            }
            if (verticalAngle < f_minVerticalConstraint)
            {
                RaiseBoundsIsReached(target);
            }

            var horizontalAngle = GetSignedAngle(_relativePos.normalized, transform.right, transform.parent.right);
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
            StopCoroutine(SmoothRotation());
            _targetRotation = _initialRotation;
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
        }
    }
}
