using UnityEngine;
using UnityEngine.Accessibility;

namespace Following
{
    public interface IFollowable
    {
        public virtual void FollowFor(Transform target) { }
        public virtual void ToggleFollowingLogic() { }
    }
}
