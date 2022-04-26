using Horror.UI.Screens;
using UnityEngine;
using Horror.UI;

namespace Horror.Gameplay.UI
{
    public sealed class PlayerHUD : UIScreen
    {
        [SerializeField] private UIFader _uiFader;

        protected override void OnOpen()
        {
            base.OnOpen();

            float endValue = 1f;
            
            _uiFader.Fade(endValue);
        }
    }
}
