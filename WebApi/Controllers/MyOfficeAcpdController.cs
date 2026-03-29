using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    /// <summary>
    /// MyOffice ACPD 人事資料 API
    /// </summary>
    [ApiController]
    [Route("api/myofficeacpd")]
    [Produces("application/json")]
    public class MyOfficeAcpdController : ControllerBase
    {
        private readonly IMyOfficeAcpdService _service;
        private readonly ILogger<MyOfficeAcpdController> _logger;

        /// <summary>
        /// 初始化 MyOffice ACPD 控制器。
        /// </summary>
        /// <param name="service">資料存取服務。</param>
        /// <param name="logger">記錄器。</param>
        public MyOfficeAcpdController(IMyOfficeAcpdService service, ILogger<MyOfficeAcpdController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// 查詢所有人事資料
        /// </summary>
        /// <returns>人事資料清單</returns>
        /// <response code="200">成功返回所有人事資料</response>
        /// <response code="500">伺服器內部錯誤</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MyOfficeAcpd>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                _logger.LogInformation($"查詢所有人事資料成功，共 {result.Count()} 筆");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"查詢人事資料時發生錯誤: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "查詢資料時發生錯誤", message = ex.Message });
            }
        }

        /// <summary>
        /// 查詢單筆人事資料
        /// </summary>
        /// <param name="id">人事代碼</param>
        /// <returns>單筆人事資料</returns>
        /// <response code="200">成功返回人事資料</response>
        /// <response code="404">人事資料不存在</response>
        /// <response code="500">伺服器內部錯誤</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MyOfficeAcpd>> GetById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { error = "人事代碼不能為空" });
                }

                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning($"查詢人事資料失敗: ID={id} 不存在");
                    return NotFound(new { error = "人事資料不存在", id = id });
                }

                _logger.LogInformation($"查詢人事資料成功: ID={id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"查詢人事資料時發生錯誤: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "查詢資料時發生錯誤", message = ex.Message });
            }
        }

        /// <summary>
        /// 新增人事資料
        /// </summary>
        /// <param name="request">新人事資料</param>
        /// <returns>建立的人事資料</returns>
        /// <response code="201">資源成功建立</response>
        /// <response code="400">請求參數有誤</response>
        /// <response code="500">伺服器內部錯誤</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MyOfficeAcpd>> Create([FromBody] MyOfficeAcpd request)
        {
            try
            {
                // 驗證必填欄位
                if (string.IsNullOrWhiteSpace(request.AcpdSid))
                {
                    return BadRequest(new { error = "人事代碼不能為空" });
                }

                // 檢查是否已存在
                var existing = await _service.GetByIdAsync(request.AcpdSid);
                if (existing != null)
                {
                    return BadRequest(new { error = "該人事代碼已存在", id = request.AcpdSid });
                }

                // 設置建立和更新時間
                request.AcpdNowDateTime = DateTime.Now;
                request.AcpdUpddatetime = DateTime.Now;

                var result = await _service.CreateAsync(request);
                if (result > 0)
                {
                    _logger.LogInformation($"新增人事資料成功: ID={request.AcpdSid}");
                    return CreatedAtAction(nameof(GetById), new { id = request.AcpdSid }, request);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "新增資料失敗" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"新增人事資料時發生錯誤: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "新增資料時發生錯誤", message = ex.Message });
            }
        }

        /// <summary>
        /// 更新人事資料
        /// </summary>
        /// <param name="id">人事代碼</param>
        /// <param name="request">更新的人事資料</param>
        /// <returns>更新結果</returns>
        /// <response code="200">請求成功</response>
        /// <response code="400">請求參數有誤</response>
        /// <response code="404">人事資料不存在</response>
        /// <response code="500">伺服器內部錯誤</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MyOfficeAcpd>> Update(string id, [FromBody] MyOfficeAcpd request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { error = "人事代碼不能為空" });
                }

                // 檢查資料是否存在
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning($"更新人事資料失敗: ID={id} 不存在");
                    return NotFound(new { error = "人事資料不存在", id = id });
                }

                // 確保 ID 一致
                request.AcpdSid = id;
                request.AcpdUpddatetime = DateTime.Now;

                var result = await _service.UpdateAsync(request);
                if (result > 0)
                {
                    _logger.LogInformation($"更新人事資料成功: ID={id}");
                    return Ok(new { message = "更新成功", id = id, data = request });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "更新資料失敗" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"更新人事資料時發生錯誤: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "更新資料時發生錯誤", message = ex.Message });
            }
        }

        /// <summary>
        /// 刪除人事資料
        /// </summary>
        /// <param name="id">人事代碼</param>
        /// <returns>刪除結果</returns>
        /// <response code="204">請求成功但無回傳內容</response>
        /// <response code="404">人事資料不存在</response>
        /// <response code="500">伺服器內部錯誤</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { error = "人事代碼不能為空" });
                }

                // 檢查資料是否存在
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning($"刪除人事資料失敗: ID={id} 不存在");
                    return NotFound(new { error = "人事資料不存在", id = id });
                }

                var result = await _service.DeleteAsync(id);
                if (result > 0)
                {
                    _logger.LogInformation($"刪除人事資料成功: ID={id}");
                    return NoContent();
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "刪除資料失敗" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"刪除人事資料時發生錯誤: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "刪除資料時發生錯誤", message = ex.Message });
            }
        }
    }
}
