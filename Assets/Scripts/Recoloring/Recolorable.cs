using SaveSystem;
using UnityEngine;
using UserInput;
using Random = UnityEngine.Random;

namespace Recoloring
{
    internal enum PartOfBody
    {
        NONE = 0,
        BODY = 1,
        SKIN = 2,
        HAIR = 3,
        EYES = 4
    }

    public class Recolorable : MonoBehaviour
    {
        [SerializeField] private PartOfBody partOfBody;

        [SerializeField] private Save _saveSystem;
        private MeshRenderer _meshRenderer;

        private UIHotKeys _hotKeys;

        private void Awake()
        {
            _saveSystem = FindAnyObjectByType<Save>();
            _saveSystem.SaveIsLoaded += SaveSystem_SaveIsLoaded;

            _hotKeys = FindAnyObjectByType<UIHotKeys>();

            if (TryGetComponent<MeshRenderer>(out _meshRenderer))
            {
                _hotKeys.Recolor += OnRecolor;
            }
            else
            {
                Debug.LogWarning("Object for is not contain MeshRenderer component. Recolor logic will not be applyed to this object");
            }
        }

        private void OnDestroy()
        {
            _saveSystem.SaveIsLoaded -= SaveSystem_SaveIsLoaded;
            _hotKeys.Recolor -= OnRecolor;
        }

        private void SaveSystem_SaveIsLoaded(SaveData save)
        {
            switch (partOfBody)
            {
                case PartOfBody.NONE:
                    break;

                case PartOfBody.BODY:
                    _meshRenderer.material.color = save.CharacterColorScheme.BodyColor;
                    break;
                case PartOfBody.SKIN:
                    _meshRenderer.material.color = save.CharacterColorScheme.SkinColor;
                    break;
                case PartOfBody.HAIR:
                    _meshRenderer.material.color = save.CharacterColorScheme.HairColor;
                    break;
                case PartOfBody.EYES:
                    _meshRenderer.material.color = save.CharacterColorScheme.EyesColor;
                    break;
            }
        }

        public void OnRecolor()
        {
            _meshRenderer.material.color = GenerateRandomColor();

            switch (partOfBody)
            {
                case PartOfBody.NONE:
                    break;
                
                case PartOfBody.BODY:
                    _saveSystem.Data.CharacterColorScheme.BodyColor = _meshRenderer.material.color;
                    break;
                case PartOfBody.SKIN:
                    _saveSystem.Data.CharacterColorScheme.SkinColor = _meshRenderer.material.color;
                    break;
                case PartOfBody.HAIR:
                    _saveSystem.Data.CharacterColorScheme.HairColor = _meshRenderer.material.color;
                    break;
                case PartOfBody.EYES:
                    _saveSystem.Data.CharacterColorScheme.EyesColor = _meshRenderer.material.color;
                    break;
            }
        }

        private Color GenerateRandomColor()
        {
            return new Color(GetRandomBetweenZeroToOne(), GetRandomBetweenZeroToOne(), GetRandomBetweenZeroToOne(), 1f);
        }

        private float GetRandomBetweenZeroToOne() => Random.Range(0f,1f);
    }
}
