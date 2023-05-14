using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trashwebWinForm.Models.DAO;
using trashwebWinForm.Models.DTO;

namespace trashwebWinForm.Controllers
{
    public class website_customerController
    {
        website_customerDAO website_CustomerDAO = new website_customerDAO();

        public website_customerDTO GetCustomerById(string id)
        {
            return website_CustomerDAO.GetCustomerById(id);
        }

        public website_customerDTO UpdateCustomerPointById(double point, string id)
        {
            return website_CustomerDAO.UpdateCustomerPointById(point, id);
        }
    }

}
