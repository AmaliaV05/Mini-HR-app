using Mini_HR_app.Data.Models;

namespace Mini_HR_app.BusinessLogic.Services
{
    public class AuditTrailService
    {
        private readonly AuditTrailFactory _auditTrailFactory;

        public AuditTrailService(AuditTrailFactory auditTrailFactory) 
        {
            _auditTrailFactory = auditTrailFactory;
        }     

        private static AuditTrailService _instance;
        private static readonly object _lock = new();

        public static AuditTrailService Instance(AuditTrailFactory auditTrailFactory)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AuditTrailService(auditTrailFactory);
                    }
                }
            }

            return _instance;
        }

        public AuditLog LogChange(string userId, string userRole, string action, string entityType, int entityId, string changedFields = null)
        {
            return AuditTrailFactory.CreateAuditLog(userId, userRole, action, entityType, entityId, changedFields);
        }
    }
}
