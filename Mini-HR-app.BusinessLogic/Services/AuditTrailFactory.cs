using Mini_HR_app.Data.Models;
using System;

namespace Mini_HR_app.BusinessLogic.Services
{
    public class AuditTrailFactory
    {
        public static AuditLog CreateAuditLog(string userId, string userRole, string action, string entityType, int entityId, string changedFields = null)
        {
            return new AuditLog
            {
                UserId = userId,
                UserRole = userRole,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                ChangedFields = changedFields,
                Timestamp = DateTime.Now
            };
        }
    }
}