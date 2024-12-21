using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace C500Hemis.Models;

public partial class TbVanBanTuChu
{
    [Display(Name = "ID văn bản từ chữ")]
    public int IdVanBanTuChu { get; set; }

    [Display(Name = "Loại văn bản")]
    public string? LoaiVanBan { get; set; }

    [Display(Name = "Nội dung")]
    public string? NoiDungVanBan { get; set; }

    [Display(Name = "Số quyết định")]
    public string? QuyetDinhBanHanh { get; set; }

    [Display(Name = "Cơ quan ban hành")]
    public string? CoQuanQuyetDinhBanHanh { get; set; }
}
