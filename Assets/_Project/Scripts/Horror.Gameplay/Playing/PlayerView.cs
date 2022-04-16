using UnityEngine;

namespace Horror.Gameplay.Playing
{
    public sealed class PlayerView : MonoBehaviour
    {
        [SerializeField] private PlayerAnimationsController _playerAnimationsController;
        [SerializeField] private GameObject _characterModel;
        [SerializeField] private Light _light;

        public PlayerAnimationsController PlayerAnimationsController => _playerAnimationsController;
        
        public void Initialize()
        {
            _playerAnimationsController.Initialize();
            
            _characterModel.SetActive(false);
                
            _light.gameObject.SetActive(true);
        }

        public void Dispose()
        {
            _characterModel.SetActive(true);
            
            _light.gameObject.SetActive(false);
        }

        public void Tick(float deltaTime)
        { }
    }
}