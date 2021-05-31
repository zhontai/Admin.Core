using AutoMapper;
using Admin.Core.Common.Auth;

namespace Admin.Core.Service
{
    public abstract class BaseService
    {
        public IUser User { get; set; }
        public IMapper Mapper { get; set; }
    }
}
