using Audit.EntityFramework;

namespace ShBarcelona.DAL.Entities
{
    [AuditIgnore]
    public class AreaEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ResponsableId { get; set; }

    }
}