using UnityEngine.Rendering.HighDefinition;
using UnityEngine;
using Mirror;

namespace Horror.Gameplay.Items
{
    public sealed class FlashLight : Item, IIlluminable
    {
        [Header("FlashLight")] 
        [SerializeField] private FlashLightView _flashLightView;
        [SerializeField] private Light _light = default;
        [SerializeField] private HDAdditionalLightData _lightData = default;
        [SerializeField] private float _volumetricMultiplier = 0.02f;
        
        [Header("FlashLight Sphere")]
        [SerializeField] private MeshRenderer _lightSphere;
        [SerializeField] private Material _glowMaterial;
        [SerializeField] private Material _defaultMaterial;

        private float _defaultVolumetricMultiplier;
        private float _originalIntensity;
        private bool _isTurnedOn;

        public override void ExecuteAction()
        {
            _isTurnedOn = !_isTurnedOn;

            CmdSetFlashLightActive(_isTurnedOn);
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            _isTurnedOn = true;

            _defaultVolumetricMultiplier = _lightData.volumetricDimmer;

            _originalIntensity = _light.intensity;
            
            SetIntensity(_originalIntensity);
        }

        protected override void OnBegin()
        {
            base.OnBegin();

            _flashLightView.Begin();
            
            _lightData.volumetricDimmer = _volumetricMultiplier;
        }

        protected override void OnStop()
        {
            base.OnStop();
            
            _flashLightView.Stop();
            
            _lightData.volumetricDimmer = _defaultVolumetricMultiplier;
        }

        [Command(requiresAuthority = false)]
        private void CmdSetFlashLightActive(bool isTurnedOn)
        {
            RpcSetFlashLightActive(isTurnedOn);
        }
        
        [ClientRpc]
        private void RpcSetFlashLightActive(bool isTurnedOn)
        {
            _isTurnedOn = isTurnedOn;
            
            if (_isTurnedOn)
            {
                SetIntensity(_originalIntensity);

                _lightSphere.material = _glowMaterial; 

                return;
            }
           
            SetIntensity(0f);
            
            _lightSphere.material = _defaultMaterial;
        }

        public void SetIntensity(float endValue)
        {
            _light.intensity = endValue;
        }
    }
}
