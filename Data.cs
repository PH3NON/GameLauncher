using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncher
{
    class Data
    {
        public string name { get; set; }
        public string path { get; set; }

        public Data(string line)
        {
            string[] m = line.Split(';');
            name = m[0];
            path = m[1];
        }
    }
}
