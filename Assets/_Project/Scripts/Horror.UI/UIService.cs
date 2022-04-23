using System.Collections.Generic;
using Horror.UI.Screens;

namespace Horror.UI
{
    public sealed class UIService : IUIService
    {
        private readonly Stack<UIScreen> _screens = new Stack<UIScreen>();
        private readonly List<UIScreen> _registeredScreens = new List<UIScreen>();
        private UIScreen _currentOpenedScreen;

        public UIScreen CurrentOpenedScreen => _currentOpenedScreen;

        public UIScreen OpenScreen(UIScreen uiScreen, OpenScreenMode openScreenMode = OpenScreenMode.Single)
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

            return uiScreen;
        }
        
        public UIScreen OpenScreen<T>(OpenScreenMode openScreenMode = OpenScreenMode.Single) where T : UIScreen
        {
            foreach (UIScreen registeredScreen in _registeredScreens)
            {
                if (registeredScreen is T)
                {
                    OpenScreen(registeredScreen);

                    return registeredScreen;
                }
            }

            return null;
        }

        public void CloseScreen(UIScreen uiScreen)
        {
            uiScreen.gameObject.SetActive(false);
            
            _screens.Pop();
        }
        
        public void CloseScreen<T>()
        {
            foreach (UIScreen uiScreen in _screens)
            {
                if (uiScreen is T)
                {
                    uiScreen.gameObject.SetActive(false);
                    
                    _screens.Pop();
                }
            }
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
        
        public void RegisterScreen(UIScreen uiScreen)
        {
            if (_registeredScreens.Contains(uiScreen))
            {
                return;
            }
            
            _registeredScreens.Add(uiScreen);
        }
        
        public void UnregisterScreen(UIScreen uiScreen)
        {
            if (!_registeredScreens.Contains(uiScreen))
            {
                return;
            }
            
            _registeredScreens.Remove(uiScreen);
        }

        private void CloseCurrentScreen()
        {
            if (_screens.Count <= 0)
            {
                return;
            }
            
            UIScreen currentScreen = _screens.Peek();

            if (currentScreen == null)
            {
                return;
            }

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

        public void Clear()
        {
            _screens.Clear();
            _registeredScreens.Clear();
        }

        public UIScreen GetScreenInStack<T>() where T : UIScreen
        {
            foreach (UIScreen uiScreen in _screens)
            {
                if (uiScreen is T)
                {
                    return uiScreen;
                }
            }
            
            return null;
        }
        
        public UIScreen GetRegisteredScreen<T>() where T : UIScreen
        {
            foreach (UIScreen registeredScreen in _registeredScreens)
            {
                if (registeredScreen is T)
                {
                    return registeredScreen;
                }
            }
            
            return null;
        }
    }
}
