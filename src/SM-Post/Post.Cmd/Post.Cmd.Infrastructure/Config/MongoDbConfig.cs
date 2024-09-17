using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Config
{
    public class MongoDbConfig
    {
        public required string ConnectionString { get; set; }
        public required string Database { get; set; }
        public required string Collection { get; set; }
    }
}
