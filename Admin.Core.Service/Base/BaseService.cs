using Admin.Core.Common.Auth;
using AutoMapper;

namespace Admin.Core.Service
{
    public abstract class BaseService
    {
        public IUser User { get; set; }
        public IMapper Mapper { get; set; }
    }
}