using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace C500Hemis.Models;

public partial class TbLichSuDoiTenTruong
{
    [Display(Name = "ID Lịch sử đổi tên trường")]
    public int IdLichSuDoiTenTruong { get; set; }

    [Display(Name = "Tên trường cũ")]
    public string? TenTruongCu { get; set; }

    [Display(Name = "Tên trường cũ Tiếng Anh")]
    public string? TenTruongCuTiengAnh { get; set; }

    [Display(Name = "Số quyết định")]
    public string? SoQuyetDinhDoiTen { get; set; }

    [Display(Name = "Ngày ký")]
    public DateOnly? NgayKyQuyetDinhDoiTen { get; set; }
}
