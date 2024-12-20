using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using C500Hemis.Models.DM;

namespace C500Hemis.Models;

public partial class TbDauMoiLienHe
{
    [Display(Name = "ID ĐẦU MỐI LIÊN HỆ")]
    public int IdDauMoiLienHe { get; set; }

    [Display(Name = "ID LOẠI ĐẦU MỐI LIÊN HỆ")]
    public int? IdLoaiDauMoiLienHe { get; set; }

    [Display(Name = "SỐ ĐIỆN THOẠI")]
    public string? SoDienThoai { get; set; }

    [Display(Name = "EMAIL")]
    public string? Email { get; set; }

    [Display(Name = "ID LOẠI ĐẦU MỐI LIÊN HỆ")]
    public virtual DmDauMoiLienHe? IdLoaiDauMoiLienHeNavigation { get; set; }
}
