using Horror.UI.Screens;

namespace Horror.UI
{
    public interface IUIService
    {
        void OpenScreen(UIScreen uiScreen, OpenScreenMode openScreenMode = OpenScreenMode.Single);
        void CloseScreen(UIScreen uiScreen);
        void CloseTopScreen();
    }
}