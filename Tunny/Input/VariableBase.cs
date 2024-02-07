using System;

using Tunny.Util;

namespace Tunny.Input
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
