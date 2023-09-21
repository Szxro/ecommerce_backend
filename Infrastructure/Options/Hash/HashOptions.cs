using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Options.Hash
{
    public class HashOptions
    {
        public int Iterations { get; set; }

        public int KeySize { get; set; }
    }
}
