namespace LS_Report.Interfaces
{
    public interface IMessage
    {
        void LongAlert(string message);

        void ShortAlert(string message);
    }
}