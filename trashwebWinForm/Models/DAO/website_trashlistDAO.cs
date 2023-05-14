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
    public class website_trashlistDAO
    {
        private readonly IDbConnection _dbConnection;

        public website_trashlistDAO()
        {
            _dbConnection = new NpgsqlConnection(CONSTANTS.CONNECTIONSTRING);
        }

        public website_trashlistDTO CreateTrashListByIdCustomer(website_trashlistDTO website_TrashlistDTO)
        {
            String query = "INSERT INTO public.website_trashlist(id, createat, numoftrash, totalscore, description, iduser_id, trash_detail_id)VALUES (@id, @createat, @numoftrash, @totalscore, @description, @iduser_id, @trash_detail_id);";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("id", website_TrashlistDTO.ID);
            parameters.Add("createat", website_TrashlistDTO.Createat);
            parameters.Add("numoftrash", website_TrashlistDTO.Numoftrash);
            parameters.Add("totalscore", website_TrashlistDTO.Totalscore);
            parameters.Add("description", website_TrashlistDTO.Description);
            parameters.Add("iduser_id", website_TrashlistDTO.Iduser_id);
            parameters.Add("trash_detail_id", website_TrashlistDTO.Trash_detail_id);
            
            return _dbConnection.QueryFirstOrDefault<website_trashlistDTO>(query, parameters);
        }

        public List<double> GetTotalScoreById(string iduser_id)
        {
            String query = "SELECT totalscore FROM public.website_trashlist WHERE iduser_id = @iduser_id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("iduser_id", iduser_id);

            return _dbConnection.Query<double>(query, parameters).ToList();
        }
    }
}
