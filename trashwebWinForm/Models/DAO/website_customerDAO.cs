using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trashwebWinForm.Common.Const;
using trashwebWinForm.Models.DTO;

namespace trashwebWinForm.Models.DAO
{
    public class website_customerDAO
    {
        private readonly IDbConnection _dbConnection;

        public website_customerDAO()
        {
            _dbConnection = new NpgsqlConnection(CONSTANTS.CONNECTIONSTRING);
        }

        //Get người dùng bằng tên đăng nhập
        public website_customerDTO GetCustomerById(string id)
        {
            String query = "SELECT * FROM public.website_customer WHERE id = @id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("id", id);

            return _dbConnection.QueryFirstOrDefault<website_customerDTO>(query, parameters);
        }
    }
}
