using UnityEngine;
using Mirror;
using TMPro;

namespace Horror.UI.Lobby
{
    public sealed class LobbyPlayer : NetworkBehaviour
    {
        [SyncVar(hook = nameof(UpdatePlayerName))] 
        public string PlayerName;
        [SyncVar(hook = nameof(UpdateReadiness))] 
        public bool IsReady;

        [Header("UI")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _readinessText;
        [SerializeField] private Color _readyTextColor;
        [SerializeField] private Color _unreadyTextColor;

        private const string UnreadyText = "Unready";
        private const string ReadyText = "Ready";
        
        public void Setup(string playerName, bool isReady)
        {
            CmdUpdatePlayerName(playerName);
            CmdUpdateReadiness(isReady);
        }
        
        [Command]
        private void CmdUpdatePlayerName(string playerName)
        {
            RpcUpdatePlayerName(playerName);
        }

        [ClientRpc]
        private void RpcUpdatePlayerName(string playerName)
        {
            _nameText.text = playerName;

            PlayerName = playerName;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdUpdateReadiness(bool isReady)
        {
            RpcUpdateReadiness(isReady);
        }

        [ClientRpc]
        private void RpcUpdateReadiness(bool isReady)
        {
            IsReady = isReady;
            
            if (isReady)
            {
                _readinessText.text = ReadyText;
                _readinessText.color = _readyTextColor;
    
                return;
            }
            
            _readinessText.text = UnreadyText;
            _readinessText.color = _unreadyTextColor;
        }

        private void UpdatePlayerName(string oldPlayerName, string newPlayerName)
        {
            _nameText.text = newPlayerName;
        }

        private void UpdateReadiness(bool oldValue, bool isReady)
        {
            CmdUpdateReadiness(isReady);
        }
    }
}