using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using C500Hemis.Models.DM;

namespace C500Hemis.Models;

public partial class TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd
{
    [Display(Name = "STT")]
    public int IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd { get; set; }

    [Display(Name = "LOẠI DANH HIỆU THI ĐUA KHEN THƯỞNG KHEN THƯỞNG")]
    public int? IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong { get; set; }

    [Display(Name = "DANH HIỆU THI ĐUA KHEN THƯỞNG KHEN THƯỞNG")]
    public int? IdDanhHieuThiDuaGiaiThuongKhenThuong { get; set; }

    [Display(Name = "SỐ QUYẾT ĐỊNH KHEN THƯỞNG")] 
    public string? SoQuyetDinhKhenThuong { get; set; }

    [Display(Name = "PHƯƠNG THỨC KHEN THƯỞNG")]
    public int? IdPhuongThucKhenThuong { get; set; }

    [Display(Name = "NĂM KHEN THƯỞNG")]
    public string? NamKhenThuong { get; set; }

    [Display(Name = "CẤP KHEN THƯỞNG")]
    public int? IdCapKhenThuong { get; set; }

    [Display(Name = "CẤP KHEN THƯỞNG")]
    public virtual DmCapKhenThuong? IdCapKhenThuongNavigation { get; set; }

    [Display(Name = "DANH HIỆU THI ĐUA KHEN THƯỞNG KHEN THƯỞNG")]
    public virtual DmThiDuaGiaiThuongKhenThuong? IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdNavigation { get; set; }

    [Display(Name = "LOẠI DANH HIỆU THI ĐUA KHEN THƯỞNG KHEN THƯỞNG")]
    public virtual DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong? IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation { get; set; }

    [Display(Name = "PHƯƠNG THỨC KHEN THƯỞNG")]
    public virtual DmPhuongThucKhenThuong? IdPhuongThucKhenThuongNavigation { get; set; }
}
