using System.Threading.Tasks;

namespace PBL5.Data;

public interface IPBL5DbSchemaMigrator
{
    Task MigrateAsync();
}
