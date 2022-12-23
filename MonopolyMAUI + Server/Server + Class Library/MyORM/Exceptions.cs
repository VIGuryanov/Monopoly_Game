using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM
{
    internal class TableAttributeException: Exception
    {
        readonly string message;

        public TableAttributeException(string msg = "DB_Table attribute not defined for specified class")
        {
            message = msg;
        }

        public override string ToString()
        {
            return $"TableAttributeException: {message}";
        }
    }

    internal class IdentityAttributeException: Exception
    {
        readonly string message;

        public IdentityAttributeException(string msg = "DB_Identity attribute not defined for specified class")
        {
            message = msg;
        }

        public override string ToString()
        {
            return $"TableAttributeException: {message}";
        }
    }

    internal class ChangeOfImmutableAttemptException: Exception
    {
        readonly string message;

        public ChangeOfImmutableAttemptException(string msg = "Attempt to change immutable value in DB")
        {
            message = msg;
        }

        public override string ToString()
        {
            return $"ImmutableChangeException: {message}";
        }
    }
}
