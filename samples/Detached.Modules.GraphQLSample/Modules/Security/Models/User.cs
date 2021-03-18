using System;

namespace Detached.Modules.GraphQLSample.Modules.Security.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }
    }
}
