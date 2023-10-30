using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    ///  лассы используещиес€ как оболочки об€вил в одном файле, дл€ удобного понимани€ вложенности данных
    /// </summary>

    [Serializable]
    public class SaveData
    {
        [SerializeField] public CharacterColorScheme CharacterColorScheme = new CharacterColorScheme();
        [SerializeField] public CharacterTransform CharacterTransform = new CharacterTransform();
    }

    [Serializable]
    public class CharacterColorScheme
    {
        [SerializeField] public Color EyesColor = Color.blue;
        [SerializeField] public Color HairColor = Color.black;
        [SerializeField] public Color SkinColor = Color.yellow;
        [SerializeField] public Color BodyColor = Color.red;
    }

    [Serializable]
    public class CharacterTransform
    {
        [SerializeField] public Vector3 Position;
        [SerializeField] public Quaternion Rotation;
    }
}
