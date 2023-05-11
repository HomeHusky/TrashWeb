using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trashwebWinForm.Models.DTO
{
    public class website_trashdetailDTO
    {
        public int ID { get; set; }
        public int Recycle { get; set; }
        public int Dangerous { get; set; }
        public int Othergarbage { get; set; }
        public string Description { get; set; }
        public string Iduser_id { get; set; }
    }
}
