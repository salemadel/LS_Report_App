using System.Threading.Tasks;

namespace LS_Report.Interfaces
{
    public interface IDialog
    {
        public Task<bool> AlertAsync(string Title, string Message, string Positif, string Negatif);
    }
}