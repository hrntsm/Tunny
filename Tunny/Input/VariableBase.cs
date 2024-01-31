using System;

namespace Tunny.Input
{
    public class VariableBase
    {
        public string NickName { get; }
        public Guid InstanceId { get; }

        public VariableBase(string nickName, Guid id)
        {
            NickName = nickName;
            InstanceId = id;
        }
    }
}
