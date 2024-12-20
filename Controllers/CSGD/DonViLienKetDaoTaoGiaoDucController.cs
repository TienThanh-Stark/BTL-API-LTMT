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
    public class DonViLienKetDaoTaoGiaoDucController : Controller
    {
        private readonly ApiServices ApiServices_;
       
        public DonViLienKetDaoTaoGiaoDucController(ApiServices services)
        {
            ApiServices_ = services;
        }

      

        private async Task<List<TbDonViLienKetDaoTaoGiaoDuc>> TbDonViLienKetDaoTaoGiaoDucs()
        {
            List<TbDonViLienKetDaoTaoGiaoDuc> tbDonViLienKetDaoTaoGiaoDucs = await ApiServices_.GetAll<TbDonViLienKetDaoTaoGiaoDuc>("/api/csgd/DonViLienKetDaoTaoGiaoDuc");
            List<TbCoSoGiaoDuc> TbCoSoGiaoDucs = await ApiServices_.GetAll<TbCoSoGiaoDuc>("/api/csgd/CoSoGiaoDuc");
            List<DmLoaiLienKet> dmLoaiLienKets = await ApiServices_.GetAll<DmLoaiLienKet>("/api/dm/LoaiLienKet");
            tbDonViLienKetDaoTaoGiaoDucs.ForEach(item => {
                item.IdCoSoGiaoDucNavigation = TbCoSoGiaoDucs.FirstOrDefault(x => x.IdCoSoGiaoDuc == item.IdCoSoGiaoDuc);
                item.IdLoaiLienKetNavigation = dmLoaiLienKets.FirstOrDefault(x => x.IdLoaiLienKet == item.IdLoaiLienKet);
               
            });
            return tbDonViLienKetDaoTaoGiaoDucs;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDonViLienKetDaoTaoGiaoDuc> getall = await TbDonViLienKetDaoTaoGiaoDucs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
               
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdDonViLienKetDaoTaoGiaoDuc)
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
                var tbDonViLienKetDaoTaoGiaoDucs = await TbDonViLienKetDaoTaoGiaoDucs();
                var tbDonViLienKetDaoTaoGiaoDuc = tbDonViLienKetDaoTaoGiaoDucs.FirstOrDefault(m => m.IdDonViLienKetDaoTaoGiaoDuc == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDonViLienKetDaoTaoGiaoDuc == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết DonViLienKetDaoTaoGiaoDuc thành công
                return View(tbDonViLienKetDaoTaoGiaoDuc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DonViLienKetDaoTaoGiaoDuc/Create
        // Hiển thị view Create để tạo một bản ghi DonViLienKetDaoTaoGiaoDuc
        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
        public async Task<IActionResult> Create()
        {
            try
            { 
                ViewData["IdCoSoGiaoDuc"] = new SelectList(await ApiServices_.GetAll<TbCoSoGiaoDuc>("/api/csgd/CoSoGiaoDuc"), "IdCoSoGiaoDuc", "TenDonVi");
                ViewData["IdLoaiLienKet"] = new SelectList(await ApiServices_.GetAll<DmLoaiLienKet>("/api/dm/LoaiLienKet"), "IdLoaiLienKet", "LoaiLienKet");
                 return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: DonViLienKetDaoTaoGiaoDuc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Thêm một CSGD mới vào Database nếu IdDonViLienKetDaoTaoGiaoDuc truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdDonViLienKetDaoTaoGiaoDuc sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdDonViLienKetDaoTaoGiaoDuc đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdDonViLienKetDaoTaoGiaoDuc,IdCoSoGiaoDuc,DiaChi,DienThoai,IdLoaiLienKet")] TbDonViLienKetDaoTaoGiaoDuc tbDonViLienKetDaoTaoGiaoDuc)
        {
            try
            {
                // Nếu trùng IDDonViLienKetDaoTaoGiaoDuc sẽ báo lỗi                
                if (await TbDonViLienKetDaoTaoGiaoDucExists(tbDonViLienKetDaoTaoGiaoDuc.IdDonViLienKetDaoTaoGiaoDuc)) ModelState.AddModelError("IdDonViLienKetDaoTaoGiaoDuc", "Đã tồn tại Id này!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create< TbDonViLienKetDaoTaoGiaoDuc > ("/api/csgd/DonViLienKetDaoTaoGiaoDuc", tbDonViLienKetDaoTaoGiaoDuc);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdCoSoGiaoDuc"] = new SelectList(await ApiServices_.GetAll<TbCoSoGiaoDuc>("/api/csgd/CoSoGiaoDuc"), "IdCoSoGiaoDuc", "TenDonVi",tbDonViLienKetDaoTaoGiaoDuc.IdCoSoGiaoDuc);
                ViewData["IdLoaiLienKet"] = new SelectList(await ApiServices_.GetAll<DmLoaiLienKet>("/api/dm/LoaiLienKet"), "IdLoaiLienKet", "LoaiLienKet", tbDonViLienKetDaoTaoGiaoDuc.IdLoaiLienKet);
                return View(tbDonViLienKetDaoTaoGiaoDuc);
            }
            catch (Exception ex)    
            {
                return BadRequest();
            }

        }

        // GET: DonViLietKetDaoTaoGiaoDuc/Edit
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

                var tbDonViLienKetDaoTaoGiaoDuc = await ApiServices_.GetId<TbDonViLienKetDaoTaoGiaoDuc>("/api/csgd/DonViLienKetDaoTaoGiaoDuc", id ?? 0);
                if (tbDonViLienKetDaoTaoGiaoDuc == null)
                {
                    return NotFound();
                }
                ViewData["IdCoSoGiaoDuc"] = new SelectList(await ApiServices_.GetAll<TbCoSoGiaoDuc>("/api/csgd/CoSoGiaoDuc"), "IdCoSoGiaoDuc", "TenDonVi" );
                ViewData["IdLoaiLienKet"] = new SelectList(await ApiServices_.GetAll<DmLoaiLienKet>("/api/dm/LoaiLienKet"), "IdLoaiLienKet", "LoaiLienKet");
                return View(tbDonViLienKetDaoTaoGiaoDuc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: DonViLienKetDaoTaoGiaoDuc/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdDonViLienKetDaoTaoGiaoDuc cần chỉnh sửa
        // Nó chỉ cập nhật khi ModelState hợp lệ
        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Edit (int id, [Bind("IdDonViLienKetDaoTaoGiaoDuc,IdCoSoGiaoDuc,DiaChi,DienThoai,IdLoaiLienKet")] TbDonViLienKetDaoTaoGiaoDuc tbDonViLienKetDaoTaoGiaoDuc)
        {
            try
            {
                if (id != tbDonViLienKetDaoTaoGiaoDuc.IdDonViLienKetDaoTaoGiaoDuc)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbDonViLienKetDaoTaoGiaoDuc>("/api/csgd/DonViLienKetDaoTaoGiaoDuc", id, tbDonViLienKetDaoTaoGiaoDuc);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbDonViLienKetDaoTaoGiaoDucExists(tbDonViLienKetDaoTaoGiaoDuc.IdDonViLienKetDaoTaoGiaoDuc) == false)
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
                ViewData["IdCoSoGiaoDuc"] = new SelectList(await ApiServices_.GetAll<TbCoSoGiaoDuc>("/api/csgd/CoSoGiaoDuc"), "IdCoSoGiaoDuc", "TenDonVi", tbDonViLienKetDaoTaoGiaoDuc.IdCoSoGiaoDuc);
                ViewData["IdLoaiLienKet"] = new SelectList(await ApiServices_.GetAll<DmLoaiLienKet>("/api/dm/LoaiLienKet"), "IdLoaiLienKet", "LoaiLienKet", tbDonViLienKetDaoTaoGiaoDuc.IdLoaiLienKet);
                return View(tbDonViLienKetDaoTaoGiaoDuc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DonViLienKetDaoTaoGiaoDuc/Delete
        // Xóa một DonViLienKetDaoTaoGiaoDuc khỏi Database
        // Lấy data DonViLienKetDaoTaoGiaoDuc từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbDonViLienKetDaoTaoGiaoDucs = await ApiServices_.GetAll<TbDonViLienKetDaoTaoGiaoDuc>("/api/csgd/DonViLienKetDaoTaoGiaoDuc");
                var tbDonViLienKetDaoTaoGiaoDuc = tbDonViLienKetDaoTaoGiaoDucs.FirstOrDefault(m => m.IdDonViLienKetDaoTaoGiaoDuc == id);
               
                if (tbDonViLienKetDaoTaoGiaoDuc == null)
                {
                    return NotFound();
                }
                return View(tbDonViLienKetDaoTaoGiaoDuc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: DonViLienKetDaoTaoGiaoDuc/Delete
        // Xóa CTĐT khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một DonViLienKetDaoTaoGiaoDuc
        {
            try
            {
                await ApiServices_.Delete<TbDonViLienKetDaoTaoGiaoDuc>("/api/csgd/DonViLienKetDaoTaoGiaoDuc", id);
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
            List<TbDonViLienKetDaoTaoGiaoDuc> data = await TbDonViLienKetDaoTaoGiaoDucs();

            // Tạo một file Excel với EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Đơn Vị Liên Kết Đào Tạo Giáo Dục");

                // Hợp nhất và đặt tiêu đề lớn
                worksheet.Cells[1, 1, 1, 5].Merge = true;
                worksheet.Cells[1, 1].Value = "Báo cáo danh sách Đơn Vị Liên Kết Đào Tạo Giáo Dục";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Tiêu đề bảng
                worksheet.Cells[2, 1].Value = "ID";
                worksheet.Cells[2, 2].Value = "Địa chỉ";
                worksheet.Cells[2, 3].Value = "Điện Thoại";
                worksheet.Cells[2, 4].Value = "Cơ Sở Giáo Dục";
                worksheet.Cells[2, 5].Value = "loại Liên Kết";
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
                    worksheet.Cells[row, 1].Value = item.IdDonViLienKetDaoTaoGiaoDuc; // ID
                    worksheet.Cells[row, 2].Value = item.DiaChi; // địa chỉ
                    worksheet.Cells[row, 3].Value = item.DienThoai; //điện thoại
                    worksheet.Cells[row, 4].Value = item.IdCoSoGiaoDucNavigation.TenDonVi; // Tên Đơn vị ứng với id CSGD
                    worksheet.Cells[row, 5].Value = item.IdLoaiLienKetNavigation.LoaiLienKet; // Loại liên kết ứng với idLoaiLienket
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

                string excelName = $"DanhSachDonViLienKetDaoTaoGiaoDuc{DateTime.Now:yyyyMMddHHmmss}.xlsx";
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

                        var tbDonViLienKetDaoTaoGiaoDucs = new List<TbDonViLienKetDaoTaoGiaoDuc>();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var coSoGiaoDucList = await ApiServices_.GetAll<TbCoSoGiaoDuc>("/api/csgd/CoSoGiaoDuc");
                            var tenCoSoGiaoDuc = worksheet.Cells[row, 7].Text.Trim(); // Lấy tên từ Excel
                            var coSoGiaoDuc = coSoGiaoDucList.FirstOrDefault(lpb => lpb.TenDonVi == tenCoSoGiaoDuc);

                            var loaiLienKetList = await ApiServices_.GetAll<DmLoaiLienKet>("/api/dm/LoaiLienKet");
                            var tenLoaiLienKet = worksheet.Cells[row, 8].Text.Trim(); // Lấy tên từ Excel
                            var loaiLienKet = loaiLienKetList.FirstOrDefault(lpb => lpb.LoaiLienKet == tenLoaiLienKet);

                            var item = new TbDonViLienKetDaoTaoGiaoDuc
                            {
                                IdDonViLienKetDaoTaoGiaoDuc = int.Parse(worksheet.Cells[row, 1].Text),
                                DiaChi = worksheet.Cells[row, 2].Text,
                                DienThoai = worksheet.Cells[row, 3].Text,
                                IdCoSoGiaoDuc = coSoGiaoDuc?.IdCoSoGiaoDuc,
                                IdLoaiLienKet = loaiLienKet?.IdLoaiLienKet
                            };

                            tbDonViLienKetDaoTaoGiaoDucs.Add(item);
                        }



                        // Gửi dữ liệu tới API để lưu vào database
                        foreach (var tbDonViLienKetDaoTaoGiaoDuc in tbDonViLienKetDaoTaoGiaoDucs)
                        {
                            await ApiServices_.Create<TbDonViLienKetDaoTaoGiaoDuc>("/api/csgd/DonViLienKetDaoTaoGiaoDuc", tbDonViLienKetDaoTaoGiaoDuc);
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
                var data = await TbDonViLienKetDaoTaoGiaoDucs();

                IEnumerable<dynamic> chartData;

                if (dataType == "LoaiLienKet")
                {
                    // Nhóm theo IdLoaiPhongBan và đếm số lượng
                    chartData = data.GroupBy(x => x.IdLoaiLienKetNavigation.LoaiLienKet)
                        .Select(g => new
                        {
                            Label = g.Key,
                            Count = g.Count()
                        }).ToList();
                }
                else if (dataType == "TenDonVi")
                {
                    // Nhóm theo IdTrangThai và đếm số lượng
                    chartData = data.GroupBy(x => x.IdCoSoGiaoDucNavigation.TenDonVi)
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
        private async Task<bool> TbDonViLienKetDaoTaoGiaoDucExists(int id)
        {
            var tbDonViLienKetDaoTaoGiaoDucs = await ApiServices_.GetAll<TbDonViLienKetDaoTaoGiaoDuc>("/api/csgd/DonViLienKetDaoTaoGiaoDuc");
            return tbDonViLienKetDaoTaoGiaoDucs.Any(e => e.IdDonViLienKetDaoTaoGiaoDuc == id);
        }

    }
}