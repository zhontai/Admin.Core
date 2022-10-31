using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;

namespace ZhonTai.Admin.Core.Db.Data;

public class PropsContractResolver : CamelCasePropertyNamesContractResolver
{
    private bool _ignore;
    private List<string> _propNames = null;

    public PropsContractResolver(List<string> propNames = null, bool ignore = true)
    {
        _propNames = propNames;
        _ignore = ignore;
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        if (_propNames != null && _propNames.Contains(member.Name))
        {
            return _ignore ? null : base.CreateProperty(member, memberSerialization);
        }

        return base.CreateProperty(member, memberSerialization);
    }
}
