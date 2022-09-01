using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_OP_Web.Models
{
    public class FilterModel
    {
        public string Name { get; set; }

        public int NumVStart { get; set; }
        public int NumVFinish { get; set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime DateTimeFinish { get; set; }

    }
}
