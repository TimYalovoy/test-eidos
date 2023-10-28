using UnityEngine;
using UnityEngine.UI;
using Eyes;

namespace Following
{
    public class FollowingSystem : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button stopFollowing;
        [Header("Target")]
        [SerializeField] private Transform target;
        [Header("React objects")]
        // Используется массив, вместо Lis<T>, так как в документации Unity по оптимиазции было сказано, что массивы "под капотом" работают быстрее чем списки
        // Однако прелесть списков в том, что их очень удобно (а при правильном использовании) эффективно использовать в совокупности с LINQ
        [SerializeField] private Eye[] eyes = null;
        [SerializeField] private Head.Head head = null;
        [SerializeField] private Body.Body body = null;

        [Header("Body parts rotation speed")]
        [Range(0f, 50f)]
        [SerializeField] private float headRotationSpeed = 0f;
        [Range(0f, 50f)]
        [SerializeField] private float bodyRotationSpeed = 0f;

        private void Awake()
        {
            if (eyes is null)
            {
                throw new System.NullReferenceException("[FollowingSystem] - NullReferenceException: check the FollowingSystem Object in scene, and set links to Eye object");
            }
            foreach (var eye in eyes)
            {
                eye.FollowFor(target);
                stopFollowing.onClick.AddListener(eye.ToggleFollowingLogic);
                eye.BoundsIsReached += Eye_BoundsIsReached;
            }

            if (head is not null)
            {
                head.BoundsIsReached += Head_BoundsIsReached;
                stopFollowing.onClick.AddListener(head.ToggleFollowingLogic);
                head.RotationSpeed = headRotationSpeed;
            }

            if (body is not null)
            {
                stopFollowing.onClick.AddListener(body.ToggleFollowingLogic);
                body.RotationSpeed = bodyRotationSpeed;
            }
        }

        private void OnDestroy()
        {
            if (eyes is null)
            {
                stopFollowing.onClick.RemoveAllListeners();
            }
            else
            {
                foreach (var eye in eyes)
                {
                    stopFollowing.onClick.RemoveListener(eye.ToggleFollowingLogic);
                    eye.BoundsIsReached -= Eye_BoundsIsReached;
                }
            }

            if (head is not null)
            {
                head.BoundsIsReached -= Head_BoundsIsReached;
            }
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void ToggleFollowing()
        {
            stopFollowing.OnPointerClick(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current));
        }

        private void Eye_BoundsIsReached(Transform target)
        {
            head.LookAtTarget(target);
        }

        private void Head_BoundsIsReached(Transform target)
        {
            body.LookAtTarget(target);
        }
    }
}
