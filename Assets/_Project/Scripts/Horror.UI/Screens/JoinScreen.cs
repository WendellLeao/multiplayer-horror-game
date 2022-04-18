using UnityEngine.SceneManagement;
using Horror.ServiceLocator;
using Castle.Core.Internal;
using Horror.Networking;
using UnityEngine;
using TMPro;

namespace Horror.UI.Screens
{
    public sealed class JoinScreen : UIScreen
    {
        [SerializeField] private TMP_InputField _ipInputField;
        [SerializeField] private HoverButton _joinButton;
        [SerializeField] private string _ipAddress;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _ipInputField.text = _ipAddress;
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            
            _ipInputField.onSubmit.AddListener(SubmitInputField);
            _ipInputField.onEndEdit.AddListener(SubmitInputField);
            
            _joinButton.OnButtonClicked += HandleJoinButtonClicked;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            
            _ipInputField.onSubmit.RemoveListener(SubmitInputField);
            _ipInputField.onEndEdit.RemoveListener(SubmitInputField);

            _joinButton.OnButtonClicked -= HandleJoinButtonClicked;
        }

        private void SubmitInputField(string ipAddress)
        {
            if (ipAddress.IsNullOrEmpty())
            {
                _joinButton.IsInteractable = false;
                
                return;
            }
            
            _joinButton.IsInteractable = true;

            _ipAddress = ipAddress;
        }

        private void HandleJoinButtonClicked()
        {
            INetworkService networkService = GameServices.GetService<INetworkService>();
            
            networkService.StartClient(_ipAddress);

            LoadNextScene();
        }
        
        private void LoadNextScene()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
