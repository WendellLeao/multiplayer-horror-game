using Horror.ServiceLocator;
using Horror.Networking;
using UnityEngine;
using TMPro;

namespace Horror.UI.Screens.MainMenu
{
    public sealed class JoinScreen : UIScreen
    {
        [Header("UI")]
        [SerializeField] private HoverButton _joinButton;
        [SerializeField] private HoverButton _backButton;
        [SerializeField] private TMP_InputField _ipInputField;
        
        [Header("Server")]
        [SerializeField] private string _ipAddress;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _ipInputField.text = _ipAddress;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            
            _joinButton.SetInteractable(true);
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            
            _ipInputField.onSubmit.AddListener(SubmitInputField);
            _ipInputField.onEndEdit.AddListener(SubmitInputField);
            
            _joinButton.OnButtonClicked += HandleJoinButtonClicked;
            _backButton.OnButtonClicked += HandleBackButtonClicked;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            
            _ipInputField.onSubmit.RemoveListener(SubmitInputField);
            _ipInputField.onEndEdit.RemoveListener(SubmitInputField);

            _joinButton.OnButtonClicked -= HandleJoinButtonClicked;
            _backButton.OnButtonClicked -= HandleBackButtonClicked;
        }

        private void SubmitInputField(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                _joinButton.SetInteractable(false);
                
                return;
            }
            
            _joinButton.SetInteractable(true);

            _ipAddress = ipAddress;
        }

        private void HandleJoinButtonClicked()
        {
            INetworkService networkService = GameServices.GetService<INetworkService>();
            
            networkService.StartClient(_ipAddress);
            
            _joinButton.SetInteractable(false);
        }

        private void HandleBackButtonClicked()
        {
            UIService.CloseTopScreen();
        }
    }
}
