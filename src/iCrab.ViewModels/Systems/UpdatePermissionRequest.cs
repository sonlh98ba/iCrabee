using System.Collections.Generic;

namespace iCrabee.ViewModels.Systems
{
    public class UpdatePermissionRequest
    {
        public List<PermissionVM> Permissions { get; set; } = new List<PermissionVM>();
    }
}
