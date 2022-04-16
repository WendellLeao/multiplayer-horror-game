using Horror.UI.Screens;

namespace Horror.UI
{
    public interface IUIService
    {
        void OpenScreen<T>() where T : UIScreen;
        void CloseScreen<T>() where T : UIScreen;
    }
}