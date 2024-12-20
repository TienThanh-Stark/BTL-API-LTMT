using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using C500Hemis.Models;
using C500Hemis.API;
using C500Hemis.Models.DM;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using OfficeOpenXml;

namespace C500Hemis.Controllers.CSGD
{
    public class LichSuDoiTenTruongController : Controller
    {
        private readonly ApiServices ApiServices_;

        public LichSuDoiTenTruongController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbLichSuDoiTenTruong>> TbLichSuDoiTenTruongs()
        {
            List<TbLichSuDoiTenTruong> tbLichSuDoiTenTruongs = await ApiServices_.GetAll<TbLichSuDoiTenTruong>("/api/csgd/LichSuDoiTenTruong");
            return tbLichSuDoiTenTruongs;
        }

        // GET: LichSuDoiTenTruong
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbLichSuDoiTenTruong> getall = await TbLichSuDoiTenTruongs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: LichSuDoiTenTruong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbLichSuDoiTenTruongs = await TbLichSuDoiTenTruongs();
                var tbLichSuDoiTenTruong = tbLichSuDoiTenTruongs.FirstOrDefault(m => m.IdLichSuDoiTenTruong == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbLichSuDoiTenTruong == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết LichSuDoiTenTruong thành công
                return View(tbLichSuDoiTenTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: LichSuDoiTenTruong/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LichSuDoiTenTruong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLichSuDoiTenTruong,TenTruongCu,TenTruongCuTiengAnh,SoQuyetDinhDoiTen,NgayKyQuyetDinhDoiTen")] TbLichSuDoiTenTruong tbLichSuDoiTenTruong)
        {
            try
            {
                // Nếu trùng IDLichSuDoiTenTruong sẽ báo lỗi
                if (await TbLichSuDoiTenTruongExists(tbLichSuDoiTenTruong.IdLichSuDoiTenTruong)) ModelState.AddModelError("IdLichSuDoiTenTruong", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbLichSuDoiTenTruong>("/api/csgd/LichSuDoiTenTruong", tbLichSuDoiTenTruong);
                    return RedirectToAction(nameof(Index));
                }
                return View(tbLichSuDoiTenTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: LichSuDoiTenTruong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbLichSuDoiTenTruong = await ApiServices_.GetId<TbLichSuDoiTenTruong>("/api/csgd/LichSuDoiTenTruong", id ?? 0);
                if (tbLichSuDoiTenTruong == null)
                {
                    return NotFound();
                }
                return View(tbLichSuDoiTenTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: LichSuDoiTenTruong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLichSuDoiTenTruong,TenTruongCu,TenTruongCuTiengAnh,SoQuyetDinhDoiTen,NgayKyQuyetDinhDoiTen")] TbLichSuDoiTenTruong tbLichSuDoiTenTruong)
        {
            try
            {
                if (id != tbLichSuDoiTenTruong.IdLichSuDoiTenTruong)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbLichSuDoiTenTruong>("/api/csgd/LichSuDoiTenTruong", id, tbLichSuDoiTenTruong);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbLichSuDoiTenTruongExists(tbLichSuDoiTenTruong.IdLichSuDoiTenTruong) == false)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(tbLichSuDoiTenTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: LichSuDoiTenTruong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbLichSuDoiTenTruongs = await ApiServices_.GetAll<TbLichSuDoiTenTruong>("/api/csgd/LichSuDoiTenTruong");
                var tbLichSuDoiTenTruong = tbLichSuDoiTenTruongs.FirstOrDefault(m => m.IdLichSuDoiTenTruong == id);
                if (tbLichSuDoiTenTruong == null)
                {
                    return NotFound();
                }

                return View(tbLichSuDoiTenTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: LichSuDoiTenTruong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbLichSuDoiTenTruong>("/api/csgd/LichSuDoiTenTruong", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbLichSuDoiTenTruongExists(int id)
        {
            var tbLichSuDoiTenTruongs = await ApiServices_.GetAll<TbLichSuDoiTenTruong>("/api/csgd/LichSuDoiTenTruong");
            return tbLichSuDoiTenTruongs.Any(e => e.IdLichSuDoiTenTruong == id);
        }

        // Xuất Excel
        public async Task<IActionResult> ExportToExcel()
        {
            // Lấy dữ liệu từ API hoặc database
            List<TbLichSuDoiTenTruong> data = await TbLichSuDoiTenTruongs();

            // Tạo một file Excel với EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách lịch sử đổi tên trường");

                // Hợp nhất và đặt tiêu đề lớn
                worksheet.Cells[1, 1, 1, 5].Merge = true;
                worksheet.Cells[1, 1].Value = "Báo cáo danh sách lịch sử đổi tên trường";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Tiêu đề bảng
                worksheet.Cells[2, 1].Value = "ID Lịch sử đổi tên trường";
                worksheet.Cells[2, 2].Value = "Tên trường cũ";
                worksheet.Cells[2, 3].Value = "Tên trường cũ Tiếng Anh";
                worksheet.Cells[2, 4].Value = "Số quyết định";
                worksheet.Cells[2, 5].Value = "Ngày ký";

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[2, 1, 2, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                }
                // Thêm dữ liệu vào bảng
                int row = 3;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.IdLichSuDoiTenTruong;
                    worksheet.Cells[row, 2].Value = item.TenTruongCu;
                    worksheet.Cells[row, 3].Value = item.TenTruongCuTiengAnh;
                    worksheet.Cells[row, 4].Value = item.SoQuyetDinhDoiTen;
                    worksheet.Cells[row, 5].Value = item.NgayKyQuyetDinhDoiTen;
                    worksheet.Cells[row, 5].Style.Numberformat.Format = "dd/MM/yyyy";
                    row++;
                }

                // Thêm viền cho toàn bộ bảng
                worksheet.Cells[2, 1, row - 1, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells.AutoFitColumns();

                var stream = new System.IO.MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"DanhSachLichSuDoiTenTruong.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        // nhập từ excel
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
            {
                return BadRequest("File không hợp lệ.");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    // Sử dụng EPPlus để đọc file Excel
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
                        int rowCount = worksheet.Dimension.Rows; // Số lượng dòng
                        int colCount = worksheet.Dimension.Columns; // Số lượng cột

                        var tbLichSuDoiTenTruongs = new List<TbLichSuDoiTenTruong>();

                        // Duyệt qua từng dòng (bỏ qua dòng tiêu đề nếu có)
                        for (int row = 3; row <= rowCount; row++) // Giả sử dòng 1 là tiêu đề
                        {
                            var item = new TbLichSuDoiTenTruong
                            {
                                IdLichSuDoiTenTruong = int.Parse(worksheet.Cells[row, 1].Text),
                                TenTruongCu = worksheet.Cells[row, 2].Text,
                                TenTruongCuTiengAnh = worksheet.Cells[row, 3].Text,
                                SoQuyetDinhDoiTen = worksheet.Cells[row, 4].Text,
                                NgayKyQuyetDinhDoiTen = DateOnly.FromDateTime(DateTime.Parse(worksheet.Cells[row, 5].Text)),
                            };
                            tbLichSuDoiTenTruongs.Add(item);
                        }

                        // Gửi dữ liệu tới API để lưu vào database
                        foreach (var tbLichSuDoiTenTruong in tbLichSuDoiTenTruongs)
                        {
                            await ApiServices_.Create<TbLichSuDoiTenTruong>("/api/csgd/LichSuDoiTenTruong", tbLichSuDoiTenTruong);
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Đã xảy ra lỗi khi xử lý file.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChartData()
        {
            try
            {
                var data = await TbLichSuDoiTenTruongs();

                // Nhóm theo IdLichSuDoiTenTruong và đếm số lượng
                var chartData = data.GroupBy(x => x.SoQuyetDinhDoiTen)
                    .Select(g => new
                    {
                        Label = g.Key,
                        Count = g.Count() // Đếm số lượng cho mỗi loại
                    }).ToList();

                return Json(new
                {
                    labels = chartData.Select(x => x.Label).ToArray(),
                    values = chartData.Select(x => x.Count).ToArray() // Số lượng tương ứng
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}