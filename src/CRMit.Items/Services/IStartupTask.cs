using System.Threading.Tasks;

namespace CRMit.Items.Services
{
    public interface IStartupTask
    {
        Task ExecuteAsync();
    }
}
