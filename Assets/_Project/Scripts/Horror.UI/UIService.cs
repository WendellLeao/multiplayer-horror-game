using System.Collections.Generic;
using Horror.UI.Screens;
using UnityEngine;

namespace Horror.UI
{
    public sealed class UIService : IUIService
    {
        private Stack<UIScreen> _screens = new Stack<UIScreen>();
        private UIScreen _currentOpenedScreen;

        public UIScreen CurrentOpenedScreen => _currentOpenedScreen;

        public void OpenScreen(UIScreen uiScreen, OpenScreenMode openScreenMode = OpenScreenMode.Single)
        {
            if (openScreenMode == OpenScreenMode.Single)
            {
                CloseCurrentScreen();
            }
            
            if (!_screens.Contains(uiScreen))
            {
                _screens.Push(uiScreen); 
            }

            uiScreen.gameObject.SetActive(true);

            _currentOpenedScreen = uiScreen;
            
            Debug.Log("Open Screen: " + uiScreen.name);
        }

        public void CloseScreen(UIScreen uiScreen)
        {
            uiScreen.gameObject.SetActive(false);
            
            _screens.Pop();
        }
        
        public void CloseTopScreen()
        {
            if (_screens.Count <= 0)
            {
                return;
            }

            UIScreen currentScreen = _screens.Pop();
            
            currentScreen.gameObject.SetActive(false);

            OpenPreviousScreen();
        }

        private void CloseCurrentScreen()
        {
            if (_screens.Count <= 0)
            {
                return;
            }
            
            UIScreen currentScreen = _screens.Peek();

            currentScreen.gameObject.SetActive(false);
        }
        
        private void OpenPreviousScreen()
        {
            if (_screens.Count <= 0)
            {
                return;
            }
            
            UIScreen previousScreen = _screens.Peek();

            OpenScreen(previousScreen);
        }
    }
}
