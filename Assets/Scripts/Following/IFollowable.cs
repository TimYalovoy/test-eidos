using UnityEngine;

namespace Following
{
    public interface IFollowable
    {
        public void FollowFor(Transform target);
        public void ToggleFollowingLogic();
    }
}
