using UnityEngine;
using TMPro;

namespace Horror.UI
{
    public sealed class HighlightableText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text = default;
        [SerializeField] private TMP_FontAsset _originalMaterial;
        [SerializeField] private TMP_FontAsset _glowMaterial;
        [SerializeField] private TMP_FontAsset _blockedMaterial;

        public void EnableGlow()
        {
            _text.fontMaterial = _glowMaterial.material;
        }

        public void DisableGlow()
        {
            _text.fontMaterial = _originalMaterial.material;
        }

        public void BlockGlow()
        {
            _text.fontMaterial = _blockedMaterial.material;
        }
        
        public void UnblockGlow()
        {
            _text.fontMaterial = _originalMaterial.material;
        }
    }
}