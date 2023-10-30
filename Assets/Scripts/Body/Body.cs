using Following;
using SaveSystem;
using System.Collections;
using UnityEngine;

namespace Body
{
    public sealed class Body : Follower, IFollowable
    {
        private void Awake()
        {
            if (_saveData is not null)
            {
                transform.position = _saveData.CharacterTransform.Position;
                transform.localRotation = _saveData.CharacterTransform.EyeRotation;
            }

            _initialRotation = transform.rotation;
        }

        public override void LookAtTarget(Transform target)
        {
            base.LookAtTarget(target);

            if (!_isRotationInProgress)
                StartCoroutine(SmoothRotation());
        }

        protected override void CalculateRelativePosition(Transform target)
        {
            base.CalculateRelativePosition(target);
            _relativePos.y = 0f;
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
            saver.Data.CharacterTransform.BodyRotation = transform.localRotation;
            saver.Data.CharacterTransform.Position = transform.localPosition;
        }
        public override void SaveIsLoaded(ISaver saver)
        {
            base.SaveIsLoaded(saver);
            transform.localRotation = saver.Data.CharacterTransform.BodyRotation;
            if (!_isFollowingForTarget) transform.localPosition = saver.Data.CharacterTransform.Position;
        }
    }
}
