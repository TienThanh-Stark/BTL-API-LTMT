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
    public class ToChucKiemDinhController : Controller
    {
        private readonly ApiServices ApiServices_;

        public ToChucKiemDinhController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbToChucKiemDinh>> TbToChucKiemDinhs()
        {
            List<TbToChucKiemDinh> tbToChucKiemDinhs = await ApiServices_.GetAll<TbToChucKiemDinh>("/api/csgd/ToChucKiemDinh");
            List<DmToChucKiemDinh> dmToChucKiemDinhs = await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh");
            List<DmKetQuaKiemDinh> dmKetQuaKiemDinhs = await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh");
            tbToChucKiemDinhs.ForEach(item => {
                item.IdKetQuaNavigation = dmKetQuaKiemDinhs.FirstOrDefault(x => x.IdKetQuaKiemDinh == item.IdKetQua);
                item.IdToChucKiemDinhNavigation = dmToChucKiemDinhs.FirstOrDefault(x => x.IdToChucKiemDinh == item.IdToChucKiemDinh);
            });
            return tbToChucKiemDinhs;
        }

        // GET: ToChucKiemDinh
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbToChucKiemDinh> getall = await TbToChucKiemDinhs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: ToChucKiemDinh/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbToChucKiemDinhs = await TbToChucKiemDinhs();
                var tbToChucKiemDinh = tbToChucKiemDinhs.FirstOrDefault(m => m.IdToChucKiemDinhCsdg == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbToChucKiemDinh == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết ToChucKiemDinh thành công
                return View(tbToChucKiemDinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: ToChucKiemDinh/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");
                ViewData["IdKetQua"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ToChucKiemDinh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdToChucKiemDinhCsdg,IdToChucKiemDinh,IdKetQua,SoQuyetDinhKiemDinh,NgayCapChungNhanKiemDinh,ThoiHanKiemDinh")] TbToChucKiemDinh tbToChucKiemDinh)
        {
            try
            {
                // Nếu trùng IDToChucKiemDinh sẽ báo lỗi
                if (await TbToChucKiemDinhExists(tbToChucKiemDinh.IdToChucKiemDinhCsdg)) ModelState.AddModelError("IdToChucKiemDinhCsdg", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbToChucKiemDinh>("/api/csgd/ToChucKiemDinh", tbToChucKiemDinh);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdKetQua"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh", tbToChucKiemDinh.IdKetQua);
                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh", tbToChucKiemDinh.IdToChucKiemDinh);
                return View(tbToChucKiemDinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: ToChucKiemDinh/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbToChucKiemDinh = await ApiServices_.GetId<TbToChucKiemDinh>("/api/csgd/ToChucKiemDinh", id ?? 0);
                if (tbToChucKiemDinh == null)
                {
                    return NotFound();
                }
                ViewData["IdKetQua"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh", tbToChucKiemDinh.IdKetQua);
                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh", tbToChucKiemDinh.IdToChucKiemDinh);
                return View(tbToChucKiemDinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: ToChucKiemDinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdToChucKiemDinhCsdg,IdToChucKiemDinh,IdKetQua,SoQuyetDinhKiemDinh,NgayCapChungNhanKiemDinh,ThoiHanKiemDinh")] TbToChucKiemDinh tbToChucKiemDinh)
        {
            try
            {
                if (id != tbToChucKiemDinh.IdToChucKiemDinhCsdg)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbToChucKiemDinh>("/api/csgd/ToChucKiemDinh", id, tbToChucKiemDinh);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbToChucKiemDinhExists(tbToChucKiemDinh.IdToChucKiemDinhCsdg) == false)
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
                ViewData["IdKetQua"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh", tbToChucKiemDinh.IdKetQua);
                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh", tbToChucKiemDinh.IdToChucKiemDinh);
                return View(tbToChucKiemDinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: ToChucKiemDinh/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbToChucKiemDinhs = await ApiServices_.GetAll<TbToChucKiemDinh>("/api/csgd/ToChucKiemDinh");
                var tbToChucKiemDinh = tbToChucKiemDinhs.FirstOrDefault(m => m.IdToChucKiemDinhCsdg == id);
                if (tbToChucKiemDinh == null)
                {
                    return NotFound();
                }

                return View(tbToChucKiemDinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: ToChucKiemDinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbToChucKiemDinh>("/api/csgd/ToChucKiemDinh", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbToChucKiemDinhExists(int id)
        {
            var tbToChucKiemDinhs = await ApiServices_.GetAll<TbToChucKiemDinh>("/api/csgd/ToChucKiemDinh");
            return tbToChucKiemDinhs.Any(e => e.IdToChucKiemDinhCsdg == id);
        }

        public async Task<IActionResult> ExportToExcel()
        {
            // Lấy dữ liệu từ API hoặc database
            List<TbToChucKiemDinh> data = await TbToChucKiemDinhs();

            // Tạo một file Excel với EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách tổ chức kiểm định");

                // Hợp nhất và đặt tiêu đề lớn
                worksheet.Cells[1, 1, 1, 6].Merge = true;
                worksheet.Cells[1, 1].Value = "Báo cáo danh sách tổ chức kiểm định";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Tiêu đề bảng
                worksheet.Cells[2, 1].Value = "ID";
                worksheet.Cells[2, 2].Value = "Số quyết Định";
                worksheet.Cells[2, 3].Value = "Ngày cấp";
                worksheet.Cells[2, 4].Value = "Ngày hết hạn kiểm định";
                worksheet.Cells[2, 5].Value = "Tổ chức kiểm định";
                worksheet.Cells[2, 6].Value = "Kết quả";

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[2, 1, 2, 6])
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
                    worksheet.Cells[row, 1].Value = item.IdToChucKiemDinhCsdg;
                    worksheet.Cells[row, 2].Value = item.SoQuyetDinhKiemDinh;
                    worksheet.Cells[row, 3].Value = item.NgayCapChungNhanKiemDinh;
                    worksheet.Cells[row, 3].Style.Numberformat.Format = "dd/MM/yyyy";
                    worksheet.Cells[row, 4].Value = item.ThoiHanKiemDinh;
                    worksheet.Cells[row, 4].Style.Numberformat.Format = "dd/MM/yyyy";
                    worksheet.Cells[row, 5].Value = item.IdToChucKiemDinhNavigation.ToChucKiemDinh;                   
                    worksheet.Cells[row, 6].Value = item.IdKetQuaNavigation.KetQuaKiemDinh;
                    
                    row++;
                }

                // Thêm viền cho toàn bộ bảng
                worksheet.Cells[2, 1, row - 1, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 6].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 6].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells.AutoFitColumns();

                var stream = new System.IO.MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"DanhSachToChucKiemDinh.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

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

                        var tbToChucKiemDinhs = new List<TbToChucKiemDinh>();

                        for (int row = 3; row <= rowCount; row++)
                        {
                            var ToChucKiemDinhList = await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh");
                            var tenToChucKiemDinh = worksheet.Cells[row, 5].Text.Trim(); // Lấy tên từ Excel
                            var ToChucKiemDinh = ToChucKiemDinhList.FirstOrDefault(lpb => lpb.ToChucKiemDinh == tenToChucKiemDinh);

                            var KetQuaKiemDinhList = await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh");
                            var tenKetQuaKiemDinh = worksheet.Cells[row, 6].Text.Trim(); // Lấy tên từ Excel
                            var KetQuaKiemDinh = KetQuaKiemDinhList.FirstOrDefault(lpb => lpb.KetQuaKiemDinh == tenKetQuaKiemDinh);

                            var item = new TbToChucKiemDinh
                            {
                                IdToChucKiemDinhCsdg = int.Parse(worksheet.Cells[row, 1].Text),
                                SoQuyetDinhKiemDinh = worksheet.Cells[row, 2].Text,
                                NgayCapChungNhanKiemDinh = DateOnly.FromDateTime(DateTime.Parse(worksheet.Cells[row, 3].Text)),
                                ThoiHanKiemDinh = DateOnly.FromDateTime(DateTime.Parse(worksheet.Cells[row, 4].Text)),
                                IdToChucKiemDinh = ToChucKiemDinh?.IdToChucKiemDinh,
                                IdKetQua = KetQuaKiemDinh?.IdKetQuaKiemDinh,
                            };

                            tbToChucKiemDinhs.Add(item);
                        }



                        // Gửi dữ liệu tới API để lưu vào database
                        foreach (var tbToChucKiemDinh in tbToChucKiemDinhs)
                        {
                            await ApiServices_.Create<TbToChucKiemDinh>("/api/csgd/ToChucKiemDinh", tbToChucKiemDinh);
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
        public async Task<IActionResult> ChartData(string dataType)
        {
            try
            {
                var data = await TbToChucKiemDinhs();

                IEnumerable<dynamic> chartData;

                if (dataType == "KetQuaKiemDinh")
                {
                    // Nhóm theo IdKetQua và đếm số lượng
                    chartData = data.GroupBy(x => x.IdKetQuaNavigation.KetQuaKiemDinh)
                        .Select(g => new
                        {
                            Label = g.Key,
                            Count = g.Count()
                        }).ToList();
                }
                else if (dataType == "ToChucKiemDinh")
                {
                    // Nhóm theo IdToChucKiemDinh và đếm số lượng
                    chartData = data.GroupBy(x => x.IdToChucKiemDinhNavigation.ToChucKiemDinh)
                        .Select(g => new
                        {
                            Label = g.Key,
                            Count = g.Count()
                        }).ToList();
                }
                else
                {
                    return BadRequest("Loại dữ liệu không hợp lệ.");
                }

                return Json(new
                {
                    labels = chartData.Select(x => x.Label).ToArray(),
                    values = chartData.Select(x => x.Count).ToArray()
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
