using Mini_HR_app.Data.Models.Constants.Utils;
using System;

namespace Mini_HR_app.Data.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }

        private string _changedFields;
        public string ChangedFields
        {
            get
            {
                return _changedFields;
            }
            set
            {
                if (Action.Equals(AuditAction.Edit) || Action.Equals(AuditAction.ChangeStatus))
                {
                    _changedFields = value;
                }
            }
        }

        public DateTime Timestamp { get; set; }
    }
}
