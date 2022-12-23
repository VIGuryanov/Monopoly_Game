using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DB_Identity : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class DB_Field : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class DB_Table : Attribute { }
}
