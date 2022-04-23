using Horror.UI.Screens;

namespace Horror.UI
{
    public interface IUIService
    {
        UIScreen GetScreen<T>() where T : UIScreen;
        UIScreen CurrentOpenedScreen { get; }
        UIScreen OpenScreen(UIScreen uiScreen, OpenScreenMode openScreenMode = OpenScreenMode.Single);
        void CloseScreen(UIScreen uiScreen);
        void CloseTopScreen();
        void Clear();
    }
}