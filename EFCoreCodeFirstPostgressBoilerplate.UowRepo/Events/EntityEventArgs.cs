using System;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.Events
{
    public class EntityEventArgs : EventArgs
    {
        public Type[] Types { get; }

        public EntityEventArgs(Type[] types)
        {
            Types = types;
        }
    }
}
