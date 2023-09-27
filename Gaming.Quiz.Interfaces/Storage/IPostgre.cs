using System;

namespace  Gaming.Quiz.Interfaces.Storage
{
    public interface IPostgre
    {
        String Schema { get; }
        String SchemaAdmin { get; }
        String SchemaService { get; }

        String SchemaRank { get; }
        String SchemaAchievement { get; }
        String ConnectionString { get; }
        String PointCalConnectionString { get; }

        void Establish();
        void Dispose();
    }
}
