using UnityEngine;

namespace Following
{
    public interface IFollowable
    {
        public virtual void FollowFor(Transform target) { }
        public virtual void ToggleFollowingLogic() { }
    }
}
