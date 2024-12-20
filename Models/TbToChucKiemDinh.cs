using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using C500Hemis.Models.DM;

namespace C500Hemis.Models;

public partial class TbToChucKiemDinh
{
    [Display(Name = "ID")]
    public int IdToChucKiemDinhCsdg { get; set; }

    [Display(Name = "Tổ chức kiểm định")]
    public int? IdToChucKiemDinh { get; set; }

    [Display(Name = "Kết quả")]
    public int? IdKetQua { get; set; }

    [Display(Name = "Số quyết Định")]
    public string? SoQuyetDinhKiemDinh { get; set; }

    [Display(Name = "Ngày cấp")]
    public DateOnly? NgayCapChungNhanKiemDinh { get; set; }

    [Display(Name = "Ngày hết hạn kiểm định")]
    public DateOnly? ThoiHanKiemDinh { get; set; }

    [Display(Name = "Kết quả")]
    public virtual DmKetQuaKiemDinh? IdKetQuaNavigation { get; set; }

    [Display(Name = "Tổ chức kiểm định")]
    public virtual DmToChucKiemDinh? IdToChucKiemDinhNavigation { get; set; }
}
