using UnityEngine;

namespace Horror.Gameplay.Items
{
    public sealed class EmfView : ItemView
    {
        [Header("Emf")] 
        [SerializeField] private MeshRenderer[] _led;
        [SerializeField] private Material[] _originalMaterials;
        [SerializeField] private Material[] _glowedMaterials;
        
        public void TurnOff()
        {
            for (int i = 0; i < _led.Length; i++)
            {
                MeshRenderer ledRenderer = _led[i];

                ledRenderer.material = _originalMaterials[i];
            }
        }

        public void TurnOn(int emfScore)
        {
            TurnOff();
            
            for (int i = 0; i < emfScore; i++)
            {
                MeshRenderer ledRenderer = _led[i];
                
                ledRenderer.material = _glowedMaterials[i];
            }
        }
    }
}