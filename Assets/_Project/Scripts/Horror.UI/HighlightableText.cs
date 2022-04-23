using UnityEngine;
using TMPro;

namespace Horror.UI
{
    public sealed class HighlightableText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text = default;
        [SerializeField] private Material _originalMaterial;
        [SerializeField] private Material _glowMaterial;
        [SerializeField] private Material _blockedMaterial;

        public void EnableGlow()
        {
            _text.fontMaterial = _glowMaterial;
        }

        public void DisableGlow()
        {
            _text.fontMaterial = _originalMaterial;
        }

        public void BlockGlow()
        {
            _text.fontMaterial = _blockedMaterial;
        }
        
        public void UnblockGlow()
        {
            _text.fontMaterial = _originalMaterial;
        }
    }
}