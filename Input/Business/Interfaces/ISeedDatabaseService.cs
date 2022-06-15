using System.Threading.Tasks;

namespace Input.Business.Interfaces
{
    public interface ISeedDatabaseService
    {
            void CreateStartStatuses();
            Task CreateStartRoles();
            Task CreateStartAdmin();
    }
}