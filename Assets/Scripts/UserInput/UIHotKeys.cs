using Following;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotKeys : MonoBehaviour
{
    [SerializeField] private KeyCode toggleFollowing = KeyCode.F;
    [SerializeField] private KeyCode save = KeyCode.K;
    [SerializeField] private KeyCode load = KeyCode.L;
    [SerializeField] private KeyCode recolor = KeyCode.R;

    private FollowingSystem followingSystem;

    private void Awake()
    {
        followingSystem = FindObjectOfType<FollowingSystem>();
    }

    void Update()
    {
        if (Input.GetKeyUp(toggleFollowing))
        {
            followingSystem.ToggleFollowing();
        }

        if (Input.GetKeyUp(save))
        {

        }
        else if (Input.GetKeyUp(load))
        {

        }

        if (Input.GetKeyUp(recolor))
        {

        }
    }
}
