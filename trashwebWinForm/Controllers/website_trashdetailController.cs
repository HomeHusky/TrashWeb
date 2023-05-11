using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trashwebWinForm.Models.DAO;
using trashwebWinForm.Models.DTO;

namespace trashwebWinForm.Controllers
{
    public class website_trashdetailController
    {
        website_trashdetailDAO website_TrashdetailDAO = new website_trashdetailDAO();

        public website_trashdetailDTO CreateTrashDetailByIdCustomer(website_trashdetailDTO website_TrashdetailDTO)
        {
            return website_TrashdetailDAO.CreateTrashDetailByIdCustomer(website_TrashdetailDTO);
        }
    }
}
