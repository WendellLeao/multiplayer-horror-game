using Horror.UI.Screens;

namespace Horror.UI
{
    public interface IUIService
    {
        UIScreen CurrentOpenedScreen { get; }
        UIScreen OpenScreen(UIScreen uiScreen, float delay = 0, OpenScreenMode openScreenMode = OpenScreenMode.Single);
        UIScreen OpenScreen<T>(float delay = 0, OpenScreenMode openScreenMode = OpenScreenMode.Single) where T : UIScreen;
        void CloseTopScreen();
        void RegisterScreen(UIScreen uiScreen);
        void UnregisterScreen(UIScreen uiScreen);
        UIScreen GetRegisteredScreen<T>() where T : UIScreen;
    }
}