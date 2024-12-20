using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace C500Hemis.Models;

public partial class TbKhoaHoc
{
    [Display(Name = "Số ID")]
    [Required(ErrorMessage = "ID Khoa Học là bắt buộc")]
    public int IdKhoaHoc { get; set; }
    [Display(Name = "Từ Năm")]
    public string? TuNam { get; set; }
    [Display(Name = "Đến Năm")]
    public string? DenNam { get; set; }
}
