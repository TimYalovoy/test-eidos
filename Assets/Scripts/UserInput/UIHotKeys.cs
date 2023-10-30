using Following;
using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserInput
{
    public class UIHotKeys : MonoBehaviour
    {
        [SerializeField] private KeyCode toggleFollowing = KeyCode.F;
        [SerializeField] private KeyCode save = KeyCode.K;
        [SerializeField] private KeyCode load = KeyCode.L;
        [SerializeField] private KeyCode recolor = KeyCode.R;
        [Space]
        [SerializeField] private Button recolorButton;

        private FollowingSystem _followingSystem;
        private Save _saveSystem;

        public event Action Recolor;
        
        private void Awake()
        {
            _followingSystem = FindObjectOfType<FollowingSystem>();
            _saveSystem = FindAnyObjectByType<Save>();

            recolorButton.onClick.AddListener(RaiseRecolorEvent);
        }

        void Update()
        {
            if (Input.GetKeyUp(toggleFollowing))
            {
                _followingSystem.ToggleFollowing();
            }

            if (Input.GetKeyUp(save))
            {
                _saveSystem.SaveData();
            }
            else if (Input.GetKeyUp(load))
            {
                _saveSystem.LoadData();
            }

            if (Input.GetKeyUp(recolor))
            {
                RaiseRecolorEvent();
            }
        }

        private void RaiseRecolorEvent()
        {
            Recolor.Invoke();
        }
    }
}
