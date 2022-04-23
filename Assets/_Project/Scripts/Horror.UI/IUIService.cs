using Horror.UI.Screens;

namespace Horror.UI
{
    public interface IUIService
    {
        UIScreen CurrentOpenedScreen { get; }
        UIScreen OpenScreen(UIScreen uiScreen, OpenScreenMode openScreenMode = OpenScreenMode.Single);
        UIScreen OpenScreen<T>(OpenScreenMode openScreenMode = OpenScreenMode.Single) where T : UIScreen;
        void CloseScreen(UIScreen uiScreen);
        void CloseScreen<T>();
        void CloseTopScreen();
        void RegisterScreen(UIScreen uiScreen);
        void UnregisterScreen(UIScreen uiScreen);
        UIScreen GetScreenInStack<T>() where T : UIScreen;
        UIScreen GetRegisteredScreen<T>() where T : UIScreen;
        void Clear();
    }
}