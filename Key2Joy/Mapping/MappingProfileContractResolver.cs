using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public class MappingProfileContractResolver : DefaultContractResolver
    {
        public new static readonly MappingProfileContractResolver Instance = new MappingProfileContractResolver();

        // I need something like the following, but sadly all Newtonsoft classes work with Type and it gets the Type with GetType (which fails):
        // https://github.com/JamesNK/Newtonsoft.Json/blob/0a2e291c0d9c0c7675d445703e51750363a549ef/Src/Newtonsoft.Json/Serialization/JsonSerializerInternalReader.cs#L71
        // Get real typename from remote class:
        //if (RemotingServices.IsTransparentProxy(mappedOption.Action))
        //{
        //    var test = RemotingServices.GetRealProxy(mappedOption.Action);
        //    var objRef = RemotingServices.GetObjRefForProxy(mappedOption.Action);
        //    var typeName = objRef.TypeInfo.TypeName;
        //}

    }
}
