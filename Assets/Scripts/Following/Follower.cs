using SaveSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Following
{
    public abstract class Follower : MonoBehaviour, IFollowable, ISaveable
    {
        public float RotationSpeed { get; set; } = 1f;
        
        protected Transform _target;
        protected Vector3 _relativePos = Vector3.zero;

        protected Quaternion _targetRotation;
        protected Quaternion _initialRotation;

        protected bool _isRotationInProgress = false;
        protected SaveData _saveData;
        protected bool _isFollowingForTarget = true;

        public event Action<Transform> BoundsIsReached;

        public virtual void LookAtTarget(Transform target = null)
        {
            if (target == null)
            {
                target = _target;
            }

            CalculateRelativePosition(target);

            _targetRotation = Quaternion.LookRotation(_relativePos);
        }

        protected virtual void CalculateRelativePosition(Transform target)
        {
            _relativePos = transform.position - target.position;
        }

        protected virtual IEnumerator SmoothRotation()
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

        protected virtual float GetSignedAngle(Vector3 normal, Vector3 selfAxis, Vector3 parentAxis)
        {
            var angle = Vector3.Angle(selfAxis, parentAxis);
            var cross = Vector3.Cross(selfAxis, parentAxis);
            var dot = Vector3.Dot(normal, cross);
            var sign = Mathf.Sign(dot);
            var signedAngle = angle * sign;
            return signedAngle;
        }

        public void RaiseBoundsIsReached(Transform target)
        {
            BoundsIsReached?.Invoke(target);
        }

        public virtual void FollowFor(Transform target)
        {
            _target = target;
        }

        public virtual void ToggleFollowingLogic()
        {
        }

        public virtual void SetData(ISaver saver)
        {
        }

        public virtual void SaveIsLoaded(ISaver saver)
        {
            _saveData = saver.Data;
        }
    }
}
