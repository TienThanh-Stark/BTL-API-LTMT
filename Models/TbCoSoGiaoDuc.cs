using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using C500Hemis.Models.DM;

namespace C500Hemis.Models;

public partial class TbCoSoGiaoDuc
{
    [Display(Name = "STT")]
    public int IdCoSoGiaoDuc { get; set; }
    [Display(Name = "MÃ ĐƠN VỊ")]
    public string? MaDonVi { get; set; }

    [Display(Name = "TÊN ĐƠN VỊ")]
    public string? TenDonVi { get; set; }

    [Display(Name = "TÊN TIẾNG ANH")]
    public string? TenTiengAnh { get; set; }

    [Display(Name = "HÌNH THỨC THÀNH LẬP")]
    public int? IdHinhThucThanhLap { get; set; }

    [Display(Name = "LOẠI HÌNH TRƯỜNG")]
    public int? IdLoaiHinhTruong { get; set; }

    [Display(Name = "SỐ QĐ CHUYỂN ĐỔI LOẠI HÌNH")]
    public string? SoQuyetDinhChuyenDoiLoaiHinh { get; set; }

    [Display(Name = "NGÀY RA QĐ CHUYỂN ĐỔI LOẠI HÌNH")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayKyQuyetDinhChuyenDoiLoaiHinh { get; set; }

    [Display(Name = "ĐẠI HỌC TRỰC THUỘC")]
    public string? TenDaiHocTrucThuoc { get; set; }

    [Display(Name = "SĐT")]
    public string? SoDienThoai { get; set; }

    [Display(Name = "FAX")]
    public string? Fax { get; set; }

    [Display(Name = "EMAIL")]
    public string? Email { get; set; }

    [Display(Name = "ĐỊA CHỈ WEB")]
    public string? DiaChiWebsite { get; set; }

    [Display(Name = "CƠ QUAN CHỦ QUÁN")]
    public int? IdCoQuanChuQuan { get; set; }

    [Display(Name = "SỐ QĐTL")]
    public string? SoQuyetDinhThanhLap { get; set; }

    [Display(Name = "NGÀY KÝ QĐTL")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayKyQuyetDinhThanhLap { get; set; }

    [Display(Name = "ĐỊA CHỈ")]
    public string? DiaChi { get; set; }

    [Display(Name = "TỈNH")]
    public int? IdTinh { get; set; }

    [Display(Name = "HUYỆN")]
    public int? IdHuyen { get; set; }

    [Display(Name = "XÃ")]
    public int? IdXa { get; set; }

    [Display(Name = "HOẠT ĐỘNG KHÔNG LỢI NHUẬN")]
    public int? HoatDongKhongLoiNhuan { get; set; }

    public string? SoQuyetDinhCapPhepHoatDong { get; set; }
    [Display(Name = "NGÀY RA ĐƯỢC CẤP PHÉP HĐ")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayDuocCapPhepHoatDong { get; set; }

    [Display(Name = "LOẠI HÌNH CSĐT")]
    public int? IdLoaiHinhCoSoDaoTao { get; set; }

    [Display(Name = "SỐ GV GDTC")]
    public int? SoGiaoVienGdtc { get; set; }

    [Display(Name = "PHÂN LOẠI CƠ SỞ")]
    public int? IdPhanLoaiCoSo { get; set; }

    [Display(Name = "TỰ CHỦ GDQPAN")]
    public int? TuChuGiaoDucQpan { get; set; }

    [Display(Name = "SỐ QĐ GIAO TỰ CHỦ")]
    public string? SoQuyetDinhGiaoTuChu { get; set; }

    [Display(Name = "ĐÀO TẠO SVQPAN1 NĂM")]
    public int? DaoTaoSvgdqpan1nam { get; set; }

    [Display(Name = "SỐ QĐ BAN HÀNH QCTC")]
    public string? SoQuyetDinhBanHanhQuyCheTaiChinh { get; set; }

    [Display(Name = "NGÀY KÝ QĐBH QCTC")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayKyQuyetDinhBanHanhQuyCheTaiChinh { get; set; }

    [Display(Name = "HOẠT ĐỘNG KHÔNG LỢI NHUẬN")]
    public virtual DmTuyChon? HoatDongKhongLoiNhuanNavigation { get; set; }

    [Display(Name = "CƠ QUAN CHỦ QUÁN")]
    public virtual DmCoQuanChuQuan? IdCoQuanChuQuanNavigation { get; set; }

    [Display(Name = "HÌNH THỨC THÀNH LẬP")]
    public virtual DmHinhThucThanhLap? IdHinhThucThanhLapNavigation { get; set; }

    [Display(Name = "HUYỆN")]
    public virtual DmHuyen? IdHuyenNavigation { get; set; }

    [Display(Name = "LOẠI HÌNH CƠ SỞ ĐÀO TẠO")]
    public virtual DmLoaiHinhCoSoDaoTao? IdLoaiHinhCoSoDaoTaoNavigation { get; set; }

    [Display(Name = "LOẠI HÌNH TRƯỜNG")]
    public virtual DmLoaiHinhTruong? IdLoaiHinhTruongNavigation { get; set; }

    [Display(Name = "PHÂN LOẠI CƠ SỞ")]
    public virtual DmPhanLoaiCoSo? IdPhanLoaiCoSoNavigation { get; set; }

    [Display(Name = "TỈNH")]
    public virtual DmTinh? IdTinhNavigation { get; set; }

    [Display(Name = "XÃ")]
    public virtual DmXa? IdXaNavigation { get; set; }

    public virtual ICollection<TbDonViLienKetDaoTaoGiaoDuc> TbDonViLienKetDaoTaoGiaoDucs { get; set; } = new List<TbDonViLienKetDaoTaoGiaoDuc>();

    [Display(Name = "TỤ CHỦ GD QPAN")]
    public virtual DmTuyChon? TuChuGiaoDucQpanNavigation { get; set; }
}
