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
    public class DauMoiLienHeController : Controller
    {
        private readonly ApiServices ApiServices_;

        public DauMoiLienHeController(ApiServices services)
        {
            ApiServices_ = services;
        }
        private async Task<List<TbDauMoiLienHe>> TbDauMoiLienHes()
        {
            List<TbDauMoiLienHe> tbDauMoiLienHes = await ApiServices_.GetAll<TbDauMoiLienHe>("/api/csgd/DauMoiLienHe");
            List<DmDauMoiLienHe> dmDauMoiLienHes = await ApiServices_.GetAll<DmDauMoiLienHe>("/api/dm/DauMoiLienHe");
            tbDauMoiLienHes.ForEach(item => {
                item.IdLoaiDauMoiLienHeNavigation = dmDauMoiLienHes.FirstOrDefault(x => x.IdDauMoiLienHe == item.IdDauMoiLienHe);
            });
            return tbDauMoiLienHes;
        }

        // GET: DauMoiLienHe
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDauMoiLienHe> getall = await TbDauMoiLienHes();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: DauMoiLienHe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbDauMoiLienHes = await TbDauMoiLienHes();
                var tbDauMoiLienHe = tbDauMoiLienHes.FirstOrDefault(m => m.IdDauMoiLienHe == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDauMoiLienHe == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết DMLH thành công
                return View(tbDauMoiLienHe);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: DauMoiLienHe/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdLoaiDauMoiLienHe"] = new SelectList(await ApiServices_.GetAll<DmDauMoiLienHe>("/api/dm/DauMoiLienHe"), "IdDauMoiLienHe", "DauMoiLienHe");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: DauMoiLienHe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDauMoiLienHe,IdLoaiDauMoiLienHe,SoDienThoai,Email")] TbDauMoiLienHe tbDauMoiLienHe)
        {
            try
            {
                // Nếu trùng IDChuongTrinhDaoTao sẽ báo lỗi
                if (await TbDauMoiLienHeExists(tbDauMoiLienHe.IdDauMoiLienHe)) ModelState.AddModelError("IdDauMoiLienHe", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbDauMoiLienHe>("/api/csgd/DauMoiLienHe", tbDauMoiLienHe);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdLoaiDauMoiLienHe"] = new SelectList(await ApiServices_.GetAll<DmDauMoiLienHe>("/api/dm/DauMoiLienHe"), "IdDauMoiLienHe", "DauMoiLienHe", tbDauMoiLienHe.IdDauMoiLienHe);
                return View(tbDauMoiLienHe);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DauMoiLienHe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbDauMoiLienHe = await ApiServices_.GetId<TbDauMoiLienHe>("/api/csgd/DauMoiLienHe", id ?? 0);
                if (tbDauMoiLienHe == null)
                {
                    return NotFound();
                }
                ViewData["IdLoaiDauMoiLienHe"] = new SelectList(await ApiServices_.GetAll<DmDauMoiLienHe>("/api/dm/DauMoiLienHe"), "IdDauMoiLienHe", "DauMoiLienHe", tbDauMoiLienHe.IdDauMoiLienHe);
                return View(tbDauMoiLienHe);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: DauMoiLienHe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDauMoiLienHe,IdLoaiDauMoiLienHe,SoDienThoai,Email")] TbDauMoiLienHe tbDauMoiLienHe)
        {
            try
            {
                if (id != tbDauMoiLienHe.IdDauMoiLienHe)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbDauMoiLienHe>("/api/csgd/DauMoiLienHe", id, tbDauMoiLienHe);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbDauMoiLienHeExists(tbDauMoiLienHe.IdDauMoiLienHe) == false)
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
                ViewData["IdLoaiDauMoiLienHe"] = new SelectList(await ApiServices_.GetAll<DmDauMoiLienHe>("/api/dm/DauMoiLienHe"), "IdDauMoiLienHe", "DauMoiLienHe", tbDauMoiLienHe.IdDauMoiLienHe);
                return View(tbDauMoiLienHe);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: DauMoiLienHe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbDauMoiLienHes = await ApiServices_.GetAll<TbDauMoiLienHe>("/api/csgd/DauMoiLienHe");
                var tbDauMoiLienHe = tbDauMoiLienHes.FirstOrDefault(m => m.IdDauMoiLienHe == id);
                if (tbDauMoiLienHe == null)
                {
                    return NotFound();
                }

                return View(tbDauMoiLienHe);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: DauMoiLienHe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDauMoiLienHe>("/api/csgd/DauMoiLienHe", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> ExportToExcel()
        {
            // Lấy dữ liệu từ API hoặc database
            List<TbDauMoiLienHe> data = await TbDauMoiLienHes();

            // Tạo một file Excel với EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách Đầu Mối Liên Hệ");

                // Hợp nhất và đặt tiêu đề lớn
                worksheet.Cells[1, 1, 1, 4].Merge = true;
                worksheet.Cells[1, 1].Value = "Báo cáo danh sách Đầu Mối Liên Hệ";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Tiêu đề bảng
                worksheet.Cells[2, 1].Value = "ID";
                worksheet.Cells[2, 2].Value = "Số Điện Thoại";
                worksheet.Cells[2, 3].Value = "Email";
                worksheet.Cells[2, 4].Value = "Loại Đầu Mối Liên Hệ";

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[2, 1, 2, 4])
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
                    worksheet.Cells[row, 1].Value = item.IdDauMoiLienHe; // ID
                    worksheet.Cells[row, 2].Value = item.SoDienThoai; // Số Điện Thoại
                    worksheet.Cells[row, 3].Value = item.Email; // Email
                    worksheet.Cells[row, 4].Value = item.IdLoaiDauMoiLienHeNavigation?.DauMoiLienHe; // Loại Đầu Mối Liên Hệ
                    row++;
                }

                // Thêm viền cho toàn bộ bảng
                worksheet.Cells[2, 1, row - 1, 4].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 4].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 4].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 4].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells.AutoFitColumns();

                var stream = new System.IO.MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"DanhSachDauMoiLienHe_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
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

                        var tbDauMoiLienHes = new List<TbDauMoiLienHe>();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var dauMoiLienHeList = await ApiServices_.GetAll<DmDauMoiLienHe>("/api/dm/DauMoiLienHe");
                            var tenDauMoiLienHe = worksheet.Cells[row, 4].Text.Trim(); // Lấy tên từ Excel
                            var dauMoiLienHe = dauMoiLienHeList.FirstOrDefault(lpb => lpb.DauMoiLienHe == tenDauMoiLienHe);

                            var item = new TbDauMoiLienHe
                            {
                                IdDauMoiLienHe = int.Parse(worksheet.Cells[row, 1].Text),
                                SoDienThoai = worksheet.Cells[row, 2].Text,
                                Email = worksheet.Cells[row, 3].Text,
                                IdLoaiDauMoiLienHe = dauMoiLienHe?.IdDauMoiLienHe
                            };

                            tbDauMoiLienHes.Add(item);
                        }



                        // Gửi dữ liệu tới API để lưu vào database
                        foreach (var tbDauMoiLienHe in tbDauMoiLienHes)
                        {
                            await ApiServices_.Create<TbDauMoiLienHe>("/api/csgd/DauMoiLienHe", tbDauMoiLienHe);
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
                var data = await TbDauMoiLienHes();

                IEnumerable<dynamic> chartData;

                if (dataType == "DauMoiLienHe")
                {
                    // Nhóm theo IdLoaiDauMoiLienHe và đếm số lượng
                    chartData = data.GroupBy(x => x.IdLoaiDauMoiLienHeNavigation.DauMoiLienHe)
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


        private async Task<bool> TbDauMoiLienHeExists(int id)
        {
            var tbDauMoiLienHes = await ApiServices_.GetAll<TbDauMoiLienHe>("/api/csgd/DauMoiLienHe");
            return tbDauMoiLienHes.Any(e => e.IdDauMoiLienHe == id);
        }
    }
}