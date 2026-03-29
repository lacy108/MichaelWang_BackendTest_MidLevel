using Dapper;
using Microsoft.Data.SqlClient;
using WebApi.Models;

namespace WebApi.Services
{
    /// <summary>
    /// MyOffice ACPD 資料存取服務
    /// </summary>
    public interface IMyOfficeAcpdService
    {
        /// <summary>
        /// 取得所有人事資料。
        /// </summary>
        /// <returns>人事資料集合。</returns>
        Task<IEnumerable<MyOfficeAcpd>> GetAllAsync();

        /// <summary>
        /// 依人事代碼取得單筆資料。
        /// </summary>
        /// <param name="id">人事代碼。</param>
        /// <returns>對應的人事資料；若不存在則為 null。</returns>
        Task<MyOfficeAcpd?> GetByIdAsync(string id);

        /// <summary>
        /// 新增一筆人事資料。
        /// </summary>
        /// <param name="entity">要新增的人事資料。</param>
        /// <returns>受影響筆數。</returns>
        Task<int> CreateAsync(MyOfficeAcpd entity);

        /// <summary>
        /// 更新一筆人事資料。
        /// </summary>
        /// <param name="entity">要更新的人事資料。</param>
        /// <returns>受影響筆數。</returns>
        Task<int> UpdateAsync(MyOfficeAcpd entity);

        /// <summary>
        /// 刪除一筆人事資料。
        /// </summary>
        /// <param name="id">人事代碼。</param>
        /// <returns>受影響筆數。</returns>
        Task<int> DeleteAsync(string id);
    }

    /// <summary>
    /// 使用 Dapper 實作的 MyOffice ACPD 資料存取服務
    /// </summary>
    public class MyOfficeAcpdService : IMyOfficeAcpdService
    {
        private readonly string _connectionString;
        private const string TableName = "MyOffice_ACPD";

        /// <summary>
        /// 初始化 MyOffice ACPD 服務。
        /// </summary>
        /// <param name="configuration">應用程式設定。</param>
        public MyOfficeAcpdService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MyOfficeAcpd>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = $@"
                    SELECT 
                        ACPD_SID AS AcpdSid,
                        ACPD_Cname AS AcpdCname,
                        ACPD_Ename AS AcpdEname,
                        ACPD_Sname AS AcpdSname,
                        ACPD_Email AS AcpdEmail,
                        ACPD_Status AS AcpdStatus,
                        ACPD_Stop AS AcpdStop,
                        ACPD_StopMemo AS AcpdStopMemo,
                        ACPD_LoginID AS AcpdLoginId,
                        ACPD_LoginPWD AS AcpdLoginPwd,
                        ACPD_Memo AS AcpdMemo,
                        ACPD_NowDateTime AS AcpdNowDateTime,
                        ACPD_NowID AS AcpdNowId,
                        ACPD_UPDDateTime AS AcpdUpddatetime,
                        ACPD_UPDID AS AcpdUpdid
                    FROM {TableName}
                    ORDER BY ACPD_SID";

                return await connection.QueryAsync<MyOfficeAcpd>(sql);
            }
        }

        /// <inheritdoc/>
        public async Task<MyOfficeAcpd?> GetByIdAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = $@"
                    SELECT 
                        ACPD_SID AS AcpdSid,
                        ACPD_Cname AS AcpdCname,
                        ACPD_Ename AS AcpdEname,
                        ACPD_Sname AS AcpdSname,
                        ACPD_Email AS AcpdEmail,
                        ACPD_Status AS AcpdStatus,
                        ACPD_Stop AS AcpdStop,
                        ACPD_StopMemo AS AcpdStopMemo,
                        ACPD_LoginID AS AcpdLoginId,
                        ACPD_LoginPWD AS AcpdLoginPwd,
                        ACPD_Memo AS AcpdMemo,
                        ACPD_NowDateTime AS AcpdNowDateTime,
                        ACPD_NowID AS AcpdNowId,
                        ACPD_UPDDateTime AS AcpdUpddatetime,
                        ACPD_UPDID AS AcpdUpdid
                    FROM {TableName}
                    WHERE ACPD_SID = @Id";

                return await connection.QueryFirstOrDefaultAsync<MyOfficeAcpd>(sql, new { Id = id });
            }
        }

        /// <inheritdoc/>
        public async Task<int> CreateAsync(MyOfficeAcpd entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = $@"
                    INSERT INTO {TableName} 
                    (ACPD_SID, ACPD_Cname, ACPD_Ename, ACPD_Sname, ACPD_Email, ACPD_Status, 
                     ACPD_Stop, ACPD_StopMemo, ACPD_LoginID, ACPD_LoginPWD, ACPD_Memo, 
                     ACPD_NowDateTime, ACPD_NowID, ACPD_UPDDateTime, ACPD_UPDID)
                    VALUES 
                    (@AcpdSid, @AcpdCname, @AcpdEname, @AcpdSname, @AcpdEmail, @AcpdStatus,
                     @AcpdStop, @AcpdStopMemo, @AcpdLoginId, @AcpdLoginPwd, @AcpdMemo,
                     @AcpdNowDateTime, @AcpdNowId, @AcpdUpddatetime, @AcpdUpdid)";

                return await connection.ExecuteAsync(sql, entity);
            }
        }

        /// <inheritdoc/>
        public async Task<int> UpdateAsync(MyOfficeAcpd entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = $@"
                    UPDATE {TableName}
                    SET 
                        ACPD_Cname = @AcpdCname,
                        ACPD_Ename = @AcpdEname,
                        ACPD_Sname = @AcpdSname,
                        ACPD_Email = @AcpdEmail,
                        ACPD_Status = @AcpdStatus,
                        ACPD_Stop = @AcpdStop,
                        ACPD_StopMemo = @AcpdStopMemo,
                        ACPD_LoginID = @AcpdLoginId,
                        ACPD_LoginPWD = @AcpdLoginPwd,
                        ACPD_Memo = @AcpdMemo,
                        ACPD_UPDDateTime = @AcpdUpddatetime,
                        ACPD_UPDID = @AcpdUpdid
                    WHERE ACPD_SID = @AcpdSid";

                return await connection.ExecuteAsync(sql, entity);
            }
        }

        /// <inheritdoc/>
        public async Task<int> DeleteAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = $"DELETE FROM {TableName} WHERE ACPD_SID = @Id";

                return await connection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
