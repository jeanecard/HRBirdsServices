using AutoMapper;
using Dapper;
using HRBirdEntity;
using HRBirdRepository.Interface;
using HRBirdsEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBirdRepository
{
    public class HRBirdSubmissionRepository : IHRBirdSubmissionRepository
    {
        private readonly ILogger<HRBirdSubmissionRepository> _logger = null;
        private readonly IConfiguration _config = null;
        private readonly IMapper _mapper = null;
        private static readonly String _DBUSER = "HRCountries:Username";
        private static readonly String _DBPASSWORD = "HRCountries:Password";
        private static readonly String CONNECTION_STRING_KEY = "BordersConnection";

        public static string SQLQUERY_VERNACULAR_NAMES { get; } = " SELECT id FROM public.\"V_HRSubmitNames\" WHERE id iLIKE @Pattern ORDER BY id DESC  ";
        public static string SQLQUERY_BYID { get; } = "SELECT * FROM public.\"V_HRSubmitPictureList\"" + " WHERE " + "\"V_HRSubmitPictureList\".\"id\"=@Id";

        public static string SQLINSERT_PICTURE { get; } = "INSERT INTO public.\"HRSubmitBirdPicture\"(id, \"vernacularName\", \"ageType\", \"genderType\", \"sourceType\", credit, comment, \"thumbnailUrl\") VALUES(@id, @vernacularName, @ageType, @genderType, @sourceType, @credit, @comment, @thumbnailUrl);";
        public static string SQLUPDATE_PICTURE { get; } = "UPDATE public.\"HRSubmitBirdPicture\" SET " +
            "\"vernacularName\"=@VernacularName, " +
            "\"ageType\"=@AgeType, " +
            "\"genderType\"=@GenderType, " +
            "\"sourceType\"=@SourceType, " +
            "\"credit\"=@Credit, " +
            "\"thumbnailUrl\"=@ThumbnailUrl, " +
            "\"fullImageUrl\"=@FullImageUrl, " +
            "\"comment\"=@Comment " +
            "WHERE " +
            "id=@Id";
        public static string SQLUPDATE_THUMBNAIL { get; } = "UPDATE public.\"HRSubmitBirdPicture\" SET " +
            "\"thumbnailUrl\"=@ThumbnailUrl " +
            "WHERE " +
            "\"HRSubmitBirdPicture\".\"id\"=@Id";
        //UPDATE public."HRSubmitBirdPicture" SET "thumbnailUrl"='kk' WHERE "HRSubmitBirdPicture"."fullImageUrl" = 'http
        public static string SQLDELETE_PICTURE { get; } = "DELETE FROM public.\"HRSubmitBirdPicture\" WHERE id = @Id";
        public static string SQLQUERY_IMAGES { get; } = "SELECT * FROM public.\"V_HRSubmitPictureList\" WHERE \"vernacularName\" iLIKE @VernacularName ";
        public static String SQLQUERY_TYPE_AGES { get; } = "SELECT * FROM public.\"V_HRSubmitAges\"";
        public static String SQLQUERY_GENDERS { get; } = "SELECT * FROM public.\"V_HRSubmitGender\"";
        public static String SQLQUERY_SOURCE { get; } = "SELECT * FROM public.\"V_HRSubmitSource\"";
        public static String SQLQUERY_GENDERTYPE { get; } = "SELECT * FROM public.\"V_HRSubmitGender\" WHERE \"Id\"::text = @Id";
        public static String SQLQUERY_AGETYPE { get; } = "SELECT * FROM public.\"V_HRSubmitAges\" WHERE \"Id\"::text = @Id";
        public static String SQLQUERY_SOURCETYPE { get; } = "SELECT * FROM public.\"V_HRSubmitSource\" WHERE \"Id\"::text = @Id";

        private HRBirdSubmissionRepository()
        {
            // Dummy for DI.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="log"></param>
        public HRBirdSubmissionRepository(
            IConfiguration config, 
            ILogger<HRBirdSubmissionRepository> log,
            IMapper mapper)
        {
            _config = config;
            _logger = log;
            _mapper = mapper;
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
            using var conn = new NpgsqlConnection(cxString);

            conn.Open();
            try
            {
                using Task<IEnumerable<String>> retourTask = conn.QueryAsync<String>(SQLQUERY_VERNACULAR_NAMES, new { Pattern = "%" + pattern + "%" });

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
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogError(ex.Message);
                }
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public async Task<HRSubmitPictureOutput> AddPictureAsync(HRSubmitPictureInput picture)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                picture.Id = Guid.NewGuid();
                using Task<int> retourTask = conn.ExecuteAsync(SQLINSERT_PICTURE, picture);
                await retourTask;
                if (retourTask.IsCompletedSuccessfully)
                {
                    return _mapper.Map<HRSubmitPictureOutput>(picture); 
                }
                else
                { 
                    throw new Exception("ExecuteAsync : Can not complete Task.");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePictureAsync(Guid id)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {

                using var retourTask = conn.ExecuteAsync(SQLDELETE_PICTURE, new { Id = id });
                await retourTask;
                if (!retourTask.IsCompletedSuccessfully)
                {
                    throw new Exception("ExecuteAsync : Can not complete Task.");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vernacularName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitPictureListItem>> GetSubmittedPicturesAsync(string vernacularName)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<IEnumerable<HRSubmitPictureListItem>> retourTask = conn.QueryAsync<HRSubmitPictureListItem>(SQLQUERY_IMAGES, new { VernacularName = vernacularName });

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
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogError(ex.Message);
                }
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public async Task<HRSubmitPictureInput> UpdatePictureAsync(HRSubmitPictureInput picture)
        {
            if(picture == null
                || picture.Id == Guid.Empty
                || picture.AgeType == Guid.Empty
                || picture.GenderType == Guid.Empty
                || picture.SourceType == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<int> retourTask = conn.ExecuteAsync(SQLUPDATE_PICTURE, picture);
                await retourTask;
                if (retourTask.IsCompletedSuccessfully)
                {
                    return picture;
                } 
                else
                {
                    throw new Exception("ExecuteAsync : Can not complete Task.");
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitGender>> GetGenderTypesAsync()
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<IEnumerable<HRSubmitGender>> retourTask = conn.QueryAsync<HRSubmitGender>(SQLQUERY_GENDERS);

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
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogError(ex.Message);
                }
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitAge>> GetAgeTypesAsync()
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<IEnumerable<HRSubmitAge>> retourTask = conn.QueryAsync<HRSubmitAge>(SQLQUERY_TYPE_AGES);
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
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogError(ex.Message);
                }
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitSource>> GetSourcesAsync()
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<IEnumerable<HRSubmitSource>> retourTask = conn.QueryAsync<HRSubmitSource>(SQLQUERY_SOURCE);

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
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogError(ex.Message);
                }
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HRSubmitGender> GetGenderTypeAsync(Guid id)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<IEnumerable<HRSubmitGender>> retourTask = conn.QueryAsync<HRSubmitGender>(SQLQUERY_GENDERTYPE, new { Id = id.ToString() });

                await retourTask;
                if (retourTask.IsCompleted)
                {
                    return retourTask.Result.FirstOrDefault();
                }
                else
                {
                    throw new Exception("QueryAsync : Can not complete Task.");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HRSubmitAge> GetAgeTypeAsync(Guid id)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<IEnumerable<HRSubmitAge>> retourTask = conn.QueryAsync<HRSubmitAge>(SQLQUERY_AGETYPE, new { Id = id.ToString() });

                await retourTask;
                if (retourTask.IsCompleted)
                {
                    return retourTask.Result.FirstOrDefault();
                }
                else
                {
                    throw new Exception("QueryAsync : Can not complete Task.");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HRSubmitSource> GetSourceAsync(Guid id)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<IEnumerable<HRSubmitSource>> retourTask = conn.QueryAsync<HRSubmitSource>(SQLQUERY_SOURCETYPE, new { Id = id.ToString() });
                await retourTask;
                if (retourTask.IsCompleted)
                {
                    return retourTask.Result.FirstOrDefault();
                }
                else
                {
                    throw new Exception("QueryAsync : Can not complete Task.");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullImagePath"></param>
        /// <param name="thumbnailPath"></param>
        /// <returns></returns>
        public async Task UpdateThumbnailAsync(string id, string thumbnailPath)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<int> retourTask = conn.ExecuteAsync(
                    SQLUPDATE_THUMBNAIL, 
                    new { ThumbnailUrl = thumbnailPath, Id = new Guid(id) });
                await retourTask;
                if (!retourTask.IsCompletedSuccessfully)
                {
                    throw new Exception("ExecuteAsync : Can not complete Task.");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullImageUrl"></param>
        /// <returns></returns>
        public async Task<HRSubmitPictureListItem> GetSubmittedPicturesByID(string id)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using var conn = new NpgsqlConnection(cxString);
            conn.Open();
            try
            {
                using Task<IEnumerable<HRSubmitPictureListItem>> retourTask = conn.QueryAsync<HRSubmitPictureListItem>(SQLQUERY_BYID, new { Id = new Guid(id) });
                await retourTask;
                if (retourTask.IsCompletedSuccessfully)
                {
                    return retourTask.Result.FirstOrDefault();
                }
                else
                {
                    throw new Exception("ExecuteAsync : Can not complete Task.");
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
