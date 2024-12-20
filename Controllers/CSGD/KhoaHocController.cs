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
using System.IO;
using OfficeOpenXml;

namespace C500Hemis.Controllers.CSGD
{
    public class KhoaHocController : Controller
    {
        private readonly ApiServices ApiServices_;

        public KhoaHocController(ApiServices services)
        {
            ApiServices_ = services;
        }



        private async Task<List<TbKhoaHoc>> TbKhoaHocs()
        {
            List<TbKhoaHoc> tbKhoaHocs = await ApiServices_.GetAll<TbKhoaHoc>("/api/csgd/KhoaHoc");
            return tbKhoaHocs;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbKhoaHoc> getall = await TbKhoaHocs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdKhoaHoc)
        // Hiển thị bản ghi đó ở view Details
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbKhoaHocs = await TbKhoaHocs();
                var tbKhoaHoc = tbKhoaHocs.FirstOrDefault(m => m.IdKhoaHoc == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbKhoaHoc == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết KhoaHoc thành công
                return View(tbKhoaHoc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: KhoaHoc/Create
        // Hiển thị view Create để tạo một bản ghi KhoaHoc
        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
        public async Task<IActionResult> Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: KhoaHoc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Thêm một KhoaHoc mới vào Database nếu IdKhoaHoctruyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdKhoaHoc sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdKhoaHoc đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdKhoaHoc,TuNam,DenNam")] TbKhoaHoc tbKhoaHoc)
        {
            try
            {
                // Nếu trùng ID.DVLKDTGD sẽ báo lỗi                
                if (await TbKhoaHocExists(tbKhoaHoc.IdKhoaHoc)) ModelState.AddModelError("IdKhoaHoc", "Đã tồn tại Id này!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbKhoaHoc>("/api/csgd/KhoaHoc", tbKhoaHoc);
                    return RedirectToAction(nameof(Index));
                }
                return View(tbKhoaHoc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: KhoaHoc/Edit
        // Nếu không tìm thấy Id tương ứng sẽ báo lỗi NotFound
        // Phương thức này gần giống Create, nhưng nó nhập dữ liệu vào Id đã có trong API
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbKhoaHoc = await ApiServices_.GetId<TbKhoaHoc>("/api/csgd/KhoaHoc", id ?? 0);
                if (tbKhoaHoc == null)
                {
                    return NotFound();
                }
                return View(tbKhoaHoc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: KhoaHoc/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdKhoaHoc cần chỉnh sửa
        // Nó chỉ cập nhật khi ModelState hợp lệ
        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Edit(int id, [Bind("IdKhoaHoc,TuNam,DenNam")] TbKhoaHoc tbKhoaHoc)
        {
            try
            {
                if (id != tbKhoaHoc.IdKhoaHoc)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbKhoaHoc>("/api/csgd/KhoaHoc", id, tbKhoaHoc);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbKhoaHocExists(tbKhoaHoc.IdKhoaHoc) == false)
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
                return View(tbKhoaHoc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: KhoaHoc/Delete
        // Xóa một KhoaHoc khỏi Database
        // Lấy data KhoaHoc từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbKhoaHocs = await ApiServices_.GetAll<TbKhoaHoc>("/api/csgd/KhoaHoc");
                var tbKhoaHoc = tbKhoaHocs.FirstOrDefault(m => m.IdKhoaHoc == id);
                if (tbKhoaHoc == null)
                {
                    return NotFound();
                }

                return View(tbKhoaHoc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: KhoaHoc/Delete
        // Xóa KhoaHoc khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một KhoaHoc
        {
            try
            {
                await ApiServices_.Delete<TbKhoaHoc>("/api/csgd/KhoaHoc", id);
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
            List<TbKhoaHoc> data = await TbKhoaHocs();

            // Tạo một file Excel với EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách Khoa Học");

                // Hợp nhất và đặt tiêu đề lớn
                worksheet.Cells[1, 1, 1, 3].Merge = true;
                worksheet.Cells[1, 1].Value = "Khoa Học";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Tiêu đề bảng
                worksheet.Cells[2, 1].Value = "ID";
                worksheet.Cells[2, 2].Value = "Từ Năm";
                worksheet.Cells[2, 3].Value = "Đến Năm";
                

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[2, 1, 2, 3])
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
                    worksheet.Cells[row, 1].Value = item.IdKhoaHoc; // IDKhoaHoc
                    worksheet.Cells[row, 2].Value = item.TuNam; //Từ năm
                    worksheet.Cells[row, 3].Value = item.DenNam; //Đến năm
                     row++;
                }

                // Thêm viền cho toàn bộ bảng
                worksheet.Cells[2, 1, row - 1, 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells.AutoFitColumns();

                var stream = new System.IO.MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"DanhSachKhoaHoc_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
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
                return BadRequest("File không hợp lệ hoặc rỗng.");
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

                        // Kiểm tra nếu số cột không khớp
                        if (colCount != 3)
                        {
                            return BadRequest("File không đúng định dạng. Vui lòng kiểm tra lại.");
                        }

                        var tbKhoaHocs = new List<TbKhoaHoc>();

                        for (int row = 3; row <= rowCount; row++) // Bắt đầu từ dòng 3 (bỏ tiêu đề)
                        {
                            // Kiểm tra dữ liệu từng dòng
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text) ||
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text) ||
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].Text))
                            {
                                continue; // Bỏ qua dòng nếu dữ liệu không hợp lệ
                            }

                            var item = new TbKhoaHoc
                            {
                                IdKhoaHoc = int.Parse(worksheet.Cells[row, 1].Text.Trim()),
                                TuNam = worksheet.Cells[row, 2].Text.Trim(),
                                DenNam = worksheet.Cells[row, 3].Text.Trim(),
                            };

                            tbKhoaHocs.Add(item);
                        }

                        if (tbKhoaHocs.Count == 0)
                        {
                            return BadRequest("Không có dữ liệu hợp lệ trong file.");
                        }

                        // Gửi dữ liệu tới API để lưu vào database
                        foreach (var tbKhoaHoc in tbKhoaHocs)
                        {
                            await ApiServices_.Create<TbKhoaHoc>("/api/csgd/KhoaHoc", tbKhoaHoc);
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc hiển thị lỗi cụ thể nếu cần
                return BadRequest($"Đã xảy ra lỗi khi xử lý file: {ex.Message}");
            }
        }



        // GET: KhoaHoc/ChartData
        [HttpGet]
        public async Task<IActionResult> ChartData()
        {
            try
            {
                var data = await TbKhoaHocs();

                // Nhóm theo IdLoaiPhongBan và đếm số lượng
                var chartData = data.GroupBy(x => x.TuNam)
                    .Select(g => new
                    {
                        Label = g.Key,
                        Count = g.Count() // Đếm số lượng phòng ban cho mỗi loại
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
        private async Task<bool> TbKhoaHocExists(int id)
        {
            var tbKhoaHocs = await ApiServices_.GetAll<TbKhoaHoc>("/api/csgd/KhoaHoc");
            return tbKhoaHocs.Any(e => e.IdKhoaHoc == id);
        }

    }
}