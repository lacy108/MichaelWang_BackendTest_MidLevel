using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// MyOffice ACPD 使用者基本資訊
    /// </summary>
    public class MyOfficeAcpd
    {
        /// <summary>
        /// 使用者主鍵
        /// </summary>
        [Key]
        [StringLength(20)]
        public required string AcpdSid { get; set; }

        /// <summary>
        /// 中文名稱
        /// </summary>
        [StringLength(60)]
        public string? AcpdCname { get; set; }

        /// <summary>
        /// 英文名稱
        /// </summary>
        [StringLength(40)]
        public string? AcpdEname { get; set; }

        /// <summary>
        /// 簡稱
        /// </summary>
        [StringLength(40)]
        public string? AcpdSname { get; set; }

        /// <summary>
        /// 使用者信箱
        /// </summary>
        [EmailAddress]
        [StringLength(60)]
        public string? AcpdEmail { get; set; }

        /// <summary>
        /// 狀況 (0: 正常, 99: 不正常)
        /// </summary>
        public byte? AcpdStatus { get; set; }

        /// <summary>
        /// 是否停用/不可登入
        /// </summary>
        public bool? AcpdStop { get; set; }

        /// <summary>
        /// 停用原因
        /// </summary>
        [StringLength(60)]
        public string? AcpdStopMemo { get; set; }

        /// <summary>
        /// 登入帳號
        /// </summary>
        [StringLength(30)]
        public string? AcpdLoginId { get; set; }

        /// <summary>
        /// 登入密碼
        /// </summary>
        [StringLength(60)]
        public string? AcpdLoginPwd { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(600)]
        public string? AcpdMemo { get; set; }

        /// <summary>
        /// 新增日期
        /// </summary>
        public DateTime? AcpdNowDateTime { get; set; }

        /// <summary>
        /// 新增人員代碼
        /// </summary>
        [StringLength(20)]
        public string? AcpdNowId { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? AcpdUpddatetime { get; set; }

        /// <summary>
        /// 修改人員代碼
        /// </summary>
        [StringLength(20)]
        public string? AcpdUpdid { get; set; }
    }
}
