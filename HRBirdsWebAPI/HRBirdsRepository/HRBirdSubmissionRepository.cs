using Dapper;
using HRBirdEntity;
using HRBirdRepository.Interface;
using HRBirdsEntities;
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
        public static string SQLINSERT_PICTURE { get; } = "INSERT INTO public.\"HRSubmitBirdPicture\"(id, vernacular_name, type_age, type_gender, id_source, credit, url_fullsize, url_thumbnail, comment) VALUES(@Id, @Vernacular_Name, @Type_age, @Type_gender, @Id_source, @Credit, @Url_fullsize, @Url_thumbnail, @Comment);";
        public static string SQLQUERY_IMAGES { get; } = "SELECT * FROM public.\"V_HRSubmitBirdPicture\" WHERE vernacular_name iLIKE @VernacularName ";

        public static String SQLQUERY_TYPE_AGES {get;} = "SELECT * FROM public.\"V_HRSubmitAges\"";
        public static String SQLQUERY_GENDERS { get; } = "SELECT * FROM public.\"V_HRSubmitGender\"";
        public static String SQLQUERY_SOURCE { get; } = "SELECT * FROM public.\"V_HRSubmitSource\"";
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
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    using (Task<IEnumerable<HRSubmitPicture>> retourTask = conn.QueryAsync<HRSubmitPicture>(SQLQUERY_IMAGES, new { VernacularName = vernacularName }))
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
        public async Task<HRSubmitPicture> UpdatePictureAsync(HRSubmitPicture picture)
        {
            await Task.Delay(1);
            return picture;
        }

        public async Task<IEnumerable<HRSubmitGender>> GetGenderTypesAsync()
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    using (Task<IEnumerable<HRSubmitGender>> retourTask = conn.QueryAsync<HRSubmitGender>(SQLQUERY_GENDERS))
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

        public async Task<IEnumerable<HRSubmitAge>> GetAgeTypesAsync()
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    using (Task<IEnumerable<HRSubmitAge>> retourTask = conn.QueryAsync<HRSubmitAge>(SQLQUERY_TYPE_AGES))
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

        public async Task<IEnumerable<HRSubmitSource>> GetSourcesAsync()
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    using (Task<IEnumerable<HRSubmitSource>> retourTask = conn.QueryAsync<HRSubmitSource>(SQLQUERY_SOURCE))
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
    }
}
