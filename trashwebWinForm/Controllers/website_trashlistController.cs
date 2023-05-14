using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trashwebWinForm.Models.DAO;
using trashwebWinForm.Models.DTO;

namespace trashwebWinForm.Controllers
{
    public class website_trashlistController
    {
        website_trashlistDAO website_TrashlistDAO = new website_trashlistDAO();

        public website_trashlistDTO CreateTrashListByIdCustomer(website_trashlistDTO website_TrashlistDTO)
        {
            return website_TrashlistDAO.CreateTrashListByIdCustomer(website_TrashlistDTO);
        }

        public List<double> GetTotalScoreById(string iduser_id)
        {
            return website_TrashlistDAO.GetTotalScoreById(iduser_id);
        }
    }
}
