using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trashwebWinForm.Models.DTO
{
    public class website_trashlistDTO
    {
        public int ID { get; set; }
        public DateTime Createat { get; set; }
        public int Numoftrash { get; set; }
        public double Totalscore { get; set; }
        public string Description { get; set; }
        public string Iduser_id { get; set; }
        public int Trash_detail_id { get; set; }
    }
}
