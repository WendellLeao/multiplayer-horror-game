using Horror.UI.Screens;

namespace Horror.UI
{
    public interface IUIService
    {
        UIScreen CurrentOpenedScreen { get; }
        UIScreen OpenScreen(UIScreen uiScreen, OpenScreenMode openScreenMode = OpenScreenMode.Single, float delay = 0);
        UIScreen OpenScreen<T>(OpenScreenMode openScreenMode = OpenScreenMode.Single, float delay = 0) where T : UIScreen;
        void CloseTopScreen();
        void RegisterScreen(UIScreen uiScreen);
        void UnregisterScreen(UIScreen uiScreen);
        UIScreen GetRegisteredScreen<T>() where T : UIScreen;
    }
}