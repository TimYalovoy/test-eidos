using System;
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
        [Header("Position")]
        [SerializeField] public Vector3 Position;
        [Header("Rotation")]
        [SerializeField] public Quaternion BodyRotation;
        [SerializeField] public Quaternion HeadRotation;
        [SerializeField] public Quaternion EyeRotation;
    }
}
