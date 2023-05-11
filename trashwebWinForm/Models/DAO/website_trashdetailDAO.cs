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
    public class website_trashdetailDAO
    {
        private readonly IDbConnection _dbConnection;

        public website_trashdetailDAO()
        {
            _dbConnection = new NpgsqlConnection(CONSTANTS.CONNECTIONSTRING);
        }

        public website_trashdetailDTO CreateTrashDetailByIdCustomer(website_trashdetailDTO website_TrashdetailDTO)
        {
            String query = "INSERT INTO public.website_trashdetail(id, recycle, dangerous, othergarbage, description, iduser_id)VALUES (@id, @recycle, @dangerous,@othergarbage, @description, @iduser_id)";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("id", website_TrashdetailDTO.ID);
            parameters.Add("recycle", website_TrashdetailDTO.Recycle);
            parameters.Add("dangerous", website_TrashdetailDTO.Dangerous);;
            parameters.Add("othergarbage", website_TrashdetailDTO.Othergarbage);
            parameters.Add("description", website_TrashdetailDTO.Description);
            parameters.Add("iduser_id", website_TrashdetailDTO.Iduser_id);

            return _dbConnection.QueryFirstOrDefault<website_trashdetailDTO>(query, parameters);
        }
    }
}
