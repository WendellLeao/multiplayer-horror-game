using UnityEngine;
using TMPro;

namespace Horror.UI
{
    public sealed class HighlightableText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text = default;
        [SerializeField] private TMP_FontAsset _originalMaterial;
        [SerializeField] private TMP_FontAsset _glowMaterial;

        public void EnableGlow()
        {
            _text.fontMaterial = _glowMaterial.material;
        }

        public void DisableGlow()
        {
            _text.fontMaterial = _originalMaterial.material;
        }
    }
}