using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace JAY.DAL
{
    public partial class iDialogDataEntities  : DbContext
    {
        public iDialogDataEntities(string connectionString) : base(connectionString)
{
        }
    }
}
