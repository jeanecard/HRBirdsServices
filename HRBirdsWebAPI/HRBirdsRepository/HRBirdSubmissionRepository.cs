using Dapper;
using HRBirdEntity;
using HRBirdRepository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRBirdRepository
{
    public class HRBirdSubmissionRepository : IHRBirdSubmissionRepository
    {
        private readonly ILogger<HRBirdSubmissionRepository> _logger = null;
        private readonly IConfiguration _config = null;
        private static readonly String _DBUSER = "HRCountries:Username";
        private static readonly String _DBPASSWORD = "HRCountries:Password";
        private static readonly String CONNECTION_STRING_KEY = "BordersConnection";

        // V_HRSubmitGeneralInformation
        public static string SQLQUERY_VERNACULAR_NAMES { get; } = " SELECT id FROM public.\"V_HRSubmitNames\" WHERE id iLIKE @Pattern ORDER BY id DESC  ";
        public static string SQLINSERT_PICTURE { get; } = "INSERT INTO public.\"HRSubmitPictures\"(id, vernacular_name, type_age, type_gender, id_source, credit, image_data) VALUES(@Id, @Vernacular_Name, @Type_age, @Type_gender, @Id_source, @Credit, @Image_data);";



        private HRBirdSubmissionRepository()
        {
            // Dummy for DI.
        }


        public HRBirdSubmissionRepository(IConfiguration config, ILogger<HRBirdSubmissionRepository> log)
        {
            _config = config;
            _logger = log;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetMatchingVernacularNamesAsync(string pattern)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    using (Task<IEnumerable<String>> retourTask = conn.QueryAsync<String>(SQLQUERY_VERNACULAR_NAMES, new { Pattern = "%" + pattern + "%" }))
                    {
                        await retourTask;
                        if (retourTask.IsCompleted)
                        {
                            return retourTask.Result;
                        }
                        else
                        {
                            throw new Exception("QueryAsync : Can not complete Task.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.LogError(ex.Message);
                    }
                    throw;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public async Task AddPictureAsync(HRSubmitPicture picture)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    using (Task<int> retourTask = conn.ExecuteAsync(SQLINSERT_PICTURE, picture))
                    {
                        await retourTask;
                        if (!retourTask.IsCompletedSuccessfully)
                        {
                            throw new Exception("ExecuteAsync : Can not complete Task.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.LogError(ex.Message);
                    }
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePictureAsync(Guid id)
        {
            await Task.Delay(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vernacularName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitPicture>> GetSubmittedPicturesAsync(string vernacularName)
        {
            await Task.Delay(1);
            return new List<HRSubmitPicture>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public async Task<HRSubmitPicture> UpdatePictureAsync(HRSubmitPicture picture)
        {
            await Task.Delay(1);
            return picture;
        }
    }
}
