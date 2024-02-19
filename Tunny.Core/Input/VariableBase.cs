using System;

using Tunny.Core.Util;

namespace Tunny.Core.Input
{
    public class VariableBase
    {
        public string NickName { get; }
        public Guid InstanceId { get; }

        public VariableBase(string nickName, Guid id)
        {
            TLog.MethodStart();
            NickName = nickName;
            InstanceId = id;
        }
    }
}
