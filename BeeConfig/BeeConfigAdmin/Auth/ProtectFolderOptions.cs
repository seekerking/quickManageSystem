using Microsoft.AspNetCore.Http;

namespace BeeConfigAdmin.Auth
{
    public class ProtectFolderOptions
    {
        public PathString Path { get; set; }
        public string PolicyName { get; set; }
    }
}
