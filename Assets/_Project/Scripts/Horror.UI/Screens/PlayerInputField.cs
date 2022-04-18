using UnityEngine;
using System;
using TMPro;

namespace Horror.UI.Screens
{
    public sealed class PlayerInputField : MonoBehaviour
    {
        public event Action<string> OnSubmitted;
        
        [SerializeField] private TMP_InputField _inputField;

        private const string PlayerNameKey = "PlayerName";
        
        private string _playerName;

        public string PlayerName => _playerName;

        public void Initialize()
        {
            _playerName = PlayerPrefs.GetString(PlayerNameKey);

            _inputField.text = _playerName;
            
            _inputField.onSubmit.AddListener(SavePlayerName);
            _inputField.onEndEdit.AddListener(SavePlayerName);
        }

        public void Dispose()
        {
            _inputField.onSubmit.RemoveListener(SavePlayerName);
            _inputField.onEndEdit.RemoveListener(SavePlayerName);
        }

        private void SavePlayerName(string text)
        {
            _playerName = text;

            PlayerPrefs.SetString(PlayerNameKey, _playerName);
            
            OnSubmitted?.Invoke(_playerName);
        }
    }
}