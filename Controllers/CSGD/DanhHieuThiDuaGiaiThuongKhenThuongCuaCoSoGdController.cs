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
    public class DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdController : Controller
    {
        private readonly ApiServices ApiServices_;

        public DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdController(ApiServices services)
        {
            ApiServices_ = services;
        }
        private async Task<List<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>> TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds()
        {
            List<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd> tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds = await ApiServices_.GetAll<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>("/api/csgd/DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGD");
            List<DmCapKhenThuong> dmCapKhenThuongs = await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong");
            List<DmThiDuaGiaiThuongKhenThuong> dmThiDuaGiaiThuongKhenThuongs= await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong");
            List<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong> dmLoaiDanhHieuThiDuaGiaiThuongKhenThuongs = await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong");
            List<DmPhuongThucKhenThuong> dmPhuongThucKhenThuongs = await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong");
            tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.ForEach(item => {
                item.IdCapKhenThuongNavigation = dmCapKhenThuongs.FirstOrDefault(x => x.IdCapKhenThuong == item.IdCapKhenThuong);
                item.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdNavigation = dmThiDuaGiaiThuongKhenThuongs.FirstOrDefault(x => x.IdThiDuaGiaiThuongKhenThuong == item.IdDanhHieuThiDuaGiaiThuongKhenThuong);
                item.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation = dmLoaiDanhHieuThiDuaGiaiThuongKhenThuongs.FirstOrDefault(x => x.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong == item.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
                item.IdPhuongThucKhenThuongNavigation= dmPhuongThucKhenThuongs.FirstOrDefault(x => x.IdPhuongThucKhenThuong == item.IdPhuongThucKhenThuong);
            });
            return tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds;
        }

        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd> getall = await TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds = await TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds();
                var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd = tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.FirstOrDefault(m => m.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCapKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong");
                ViewData["IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd"] = new SelectList(await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong"), "IdThiDuaGiaiThuongKhenThuong", "ThiDuaGiaiThuongKhenThuong");
                ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong"), "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "LoaiDanhHieuThiDuaGiaiThuongKhenThuong");
                ViewData["IdPhuongThucKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong"), "IdPhuongThucKhenThuong", "PhuongThucKhenThuong");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd,IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong,IdDanhHieuThiDuaGiaiThuongKhenThuong,SoQuyetDinhKhenThuong,IdPhuongThucKhenThuong,NamKhenThuong,IdCapKhenThuong")] TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd)
        {
            try
            {
                // Nếu trùng IDChuongTrinhDaoTao sẽ báo lỗi
                if (await TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdExists(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd)) ModelState.AddModelError("IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>("/api/csgd/DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGD", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdCapKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdCapKhenThuong);
                ViewData["IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd"] = new SelectList(await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong"), "IdThiDuaGiaiThuongKhenThuong", "ThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuong);
                ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong"), "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "LoaiDanhHieuThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
                ViewData["IdPhuongThucKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong"), "IdPhuongThucKhenThuong", "PhuongThucKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdPhuongThucKhenThuong);
                return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd = await ApiServices_.GetId<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>("/api/csgd/DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGD", id ?? 0);
                if (tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == null)
                {
                    return NotFound();
                }
                ViewData["IdCapKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdCapKhenThuong);
                ViewData["IdDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong"), "IdThiDuaGiaiThuongKhenThuong", "ThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuong);
                ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong"), "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "LoaiDanhHieuThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
                ViewData["IdPhuongThucKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong"), "IdPhuongThucKhenThuong", "PhuongThucKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdPhuongThucKhenThuong);
                return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd,IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong,IdDanhHieuThiDuaGiaiThuongKhenThuong,SoQuyetDinhKhenThuong,IdPhuongThucKhenThuong,NamKhenThuong,IdCapKhenThuong")] TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd)
        {
            try
            {
                if (id != tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>("/api/csgd/DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGD", id, tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdExists(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd) == false)
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
                ViewData["IdCapKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdCapKhenThuong);
                ViewData["IdDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong"), "IdThiDuaGiaiThuongKhenThuong", "ThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuong);
                ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong"), "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "LoaiDanhHieuThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
                ViewData["IdPhuongThucKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong"), "IdPhuongThucKhenThuong", "PhuongThucKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdPhuongThucKhenThuong);
                return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds = await ApiServices_.GetAll<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>("/api/csgd/DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGD");
                var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd = tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.FirstOrDefault(m => m.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == id);
                if (tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == null)
                {
                    return NotFound();
                }

                return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>("/api/csgd/DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGD", id);
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
            List<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd> data = await TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds();

            // Tạo một file Excel với EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách danh hiệu thi đua giải thưởng, khen thưởng");

                // Hợp nhất và đặt tiêu đề lớn
                worksheet.Cells[1, 1, 1, 7].Merge = true;
                worksheet.Cells[1, 1].Value = "Báo cáo sách danh hiệu thi đua giải thưởng, khen thưởng";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Tiêu đề bảng
                worksheet.Cells[2, 1].Value = "ID";
                worksheet.Cells[2, 2].Value = "Số Quyết Định Khen Thưởng";
                worksheet.Cells[2, 3].Value = "Năm Khen Thưởng";
                worksheet.Cells[2, 4].Value = "Cấp Khen Thưởng";
                worksheet.Cells[2, 5].Value = "Danh Hiệu Thi Đua Giải Thưởng, Khen Thưởng";
                worksheet.Cells[2, 6].Value = "Loại Danh Hiệu Thi Đua Giải Thưởng, Khen Thưởng";
                worksheet.Cells[2, 7].Value = "Phương Thức Khen Thưởng";

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[2, 1, 2, 7])
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
                    worksheet.Cells[row, 1].Value = item.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd; // ID
                    worksheet.Cells[row, 2].Value = item.SoQuyetDinhKhenThuong; // Mã Phòng Đơn Vị
                    worksheet.Cells[row, 3].Value = item.NamKhenThuong; // Mã Phòng Đơn Vị Cha
                    worksheet.Cells[row, 4].Value = item.IdCapKhenThuongNavigation?.IdCapKhenThuong; // Tên Phòng Ban Đơn Vị
                    worksheet.Cells[row, 5].Value = item.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdNavigation?.IdThiDuaGiaiThuongKhenThuong; // Số Quyết Định Thành Lập
                    worksheet.Cells[row, 6].Value = item.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation?.LoaiDanhHieuThiDuaGiaiThuongKhenThuong; // Ngày Ra Quyết Định
                    worksheet.Cells[row, 7].Value = item.IdPhuongThucKhenThuongNavigation?.PhuongThucKhenThuong; // Loại Phòng Ban
                    row++;
                }

                // Thêm viền cho toàn bộ bảng
                worksheet.Cells[2, 1, row - 1, 7].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 7].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 7].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[2, 1, row - 1, 7].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells.AutoFitColumns();

                var stream = new System.IO.MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"DanhSachDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGD_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
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

                        var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds = new List<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var capKhenThuongList = await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong");
                            var tenCapKhenThuong = worksheet.Cells[row, 4].Text.Trim(); // Lấy tên từ Excel
                            var capKhenThuong = capKhenThuongList.FirstOrDefault(lpb => lpb.CapKhenThuong == tenCapKhenThuong);

                            var thiDuaGiaiThuongKhenThuongList = await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong");
                            var tenThiDuaGiaiThuongKhenThuong = worksheet.Cells[row, 5].Text.Trim(); // Lấy tên từ Excel
                            var thiDuaGiaiThuongKhenThuong = thiDuaGiaiThuongKhenThuongList.FirstOrDefault(lpb => lpb.ThiDuaGiaiThuongKhenThuong == tenThiDuaGiaiThuongKhenThuong);

                            var loaiDanhHieuThiDuaGiaiThuongKhenThuongList = await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong");
                            var tenLoaiDanhHieuThiDuaGiaiThuongKhenThuong = worksheet.Cells[row, 6].Text.Trim(); // Lấy tên từ Excel
                            var loaiDanhHieuThiDuaGiaiThuongKhenThuong = loaiDanhHieuThiDuaGiaiThuongKhenThuongList.FirstOrDefault(lpb => lpb.LoaiDanhHieuThiDuaGiaiThuongKhenThuong == tenLoaiDanhHieuThiDuaGiaiThuongKhenThuong);

                            var phuongThucKhenThuongList = await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong");
                            var tenPhuongThucKhenThuong = worksheet.Cells[row, 7].Text.Trim(); // Lấy tên từ Excel
                            var phuongThucKhenThuong = phuongThucKhenThuongList.FirstOrDefault(lpb => lpb.PhuongThucKhenThuong == tenPhuongThucKhenThuong);

                            var item = new TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd
                            {
                                IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd = int.Parse(worksheet.Cells[row, 1].Text),
                                SoQuyetDinhKhenThuong = worksheet.Cells[row, 2].Text,
                                NamKhenThuong = worksheet.Cells[row, 3].Text,
                                IdCapKhenThuong = capKhenThuong?.IdCapKhenThuong,
                                IdDanhHieuThiDuaGiaiThuongKhenThuong = thiDuaGiaiThuongKhenThuong?.IdThiDuaGiaiThuongKhenThuong,
                                IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong = loaiDanhHieuThiDuaGiaiThuongKhenThuong?.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong,
                                IdPhuongThucKhenThuong = phuongThucKhenThuong?.IdPhuongThucKhenThuong
                            };

                            tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.Add(item);
                        }



                        // Gửi dữ liệu tới API để lưu vào database
                        foreach (var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd in tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds)
                        {
                            await ApiServices_.Create<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>("/api/csgd/DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGD", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
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
                var data = await TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds();

                IEnumerable<dynamic> chartData;

                if (dataType == "CapKhenThuong")
                {
                    // Nhóm theo IdLoaiPhongBan và đếm số lượng
                    chartData = data.GroupBy(x => x.IdCapKhenThuongNavigation.CapKhenThuong)
                        .Select(g => new
                        {
                            Label = g.Key,
                            Count = g.Count()
                        }).ToList();
                }
                else if (dataType == "DanhHieuThiDuaGiaiThuongKhenThuong")
                {
                    // Nhóm theo IdTrangThai và đếm số lượng
                    chartData = data.GroupBy(x => x.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdNavigation.ThiDuaGiaiThuongKhenThuong)
                        .Select(g => new
                        {
                            Label = g.Key,
                            Count = g.Count()
                        }).ToList();
                }
                else if (dataType == "LoaiDanhHieuThiDuaGiaiThuongKhenThuong")
                {
                    // Nhóm theo IdTrangThai và đếm số lượng
                    chartData = data.GroupBy(x => x.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation.LoaiDanhHieuThiDuaGiaiThuongKhenThuong)
                        .Select(g => new
                        {
                            Label = g.Key,
                            Count = g.Count()
                        }).ToList();
                }
                else if (dataType == "PhuongThucKhenThuong")
                {
                    // Nhóm theo IdTrangThai và đếm số lượng
                    chartData = data.GroupBy(x => x.IdPhuongThucKhenThuongNavigation.PhuongThucKhenThuong)
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


        private async Task<bool> TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdExists(int id)
        {
            var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds = await ApiServices_.GetAll<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>("/api/csgd/DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGD");
            return tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.Any(e => e.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd== id);
        }
    }
}

//﻿using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using C500Hemis.Models;

//namespace C500Hemis.Controllers.CSGD
//{
//    public class DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdController : Controller
//    {
//        private readonly HemisContext _context;

//        public DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdController(HemisContext context)
//        {
//            _context = context;
//        }

//        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd
//        public async Task<IActionResult> Index()
//        {
//            var hemisContext = _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.Include(t => t.IdCapKhenThuongNavigation).Include(t => t.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdNavigation).Include(t => t.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation).Include(t => t.IdPhuongThucKhenThuongNavigation);
//            return View(await hemisContext.ToListAsync());
//        }

//        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd = await _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds
//                .Include(t => t.IdCapKhenThuongNavigation)
//                .Include(t => t.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdNavigation)
//                .Include(t => t.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation)
//                .Include(t => t.IdPhuongThucKhenThuongNavigation)
//                .FirstOrDefaultAsync(m => m.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == id);
//            if (tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == null)
//            {
//                return NotFound();
//            }

//            return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//        }

//        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Create
//        public IActionResult Create()
//        {
//            ViewData["IdCapKhenThuong"] = new SelectList(_context.DmCapKhenThuongs, "IdCapKhenThuong", "IdCapKhenThuong");
//            ViewData["IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd"] = new SelectList(_context.DmThiDuaGiaiThuongKhenThuongs, "IdThiDuaGiaiThuongKhenThuong", "IdThiDuaGiaiThuongKhenThuong");
//            ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(_context.DmLoaiDanhHieuThiDuaGiaiThuongKhenThuongs, "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong");
//            ViewData["IdPhuongThucKhenThuong"] = new SelectList(_context.DmPhuongThucKhenThuongs, "IdPhuongThucKhenThuong", "IdPhuongThucKhenThuong");
//            return View();
//        }

//        // POST: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd,IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong,IdDanhHieuThiDuaGiaiThuongKhenThuong,SoQuyetDinhKhenThuong,IdPhuongThucKhenThuong,NamKhenThuong,IdCapKhenThuong")] TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    // Thêm đối tượng vào context
//                    _context.Add(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);

//                    // Lưu thay đổi
//                    await _context.SaveChangesAsync();

//                    // Chuyển hướng về trang Index sau khi lưu thành công
//                    return RedirectToAction(nameof(Index));
//                }
//                catch (Exception ex)
//                {
//                    // Bắt lỗi nếu xảy ra khi lưu dữ liệu
//                    Console.WriteLine(ex.Message);  // Bạn có thể log lỗi ra file log hoặc hiển thị ra giao diện
//                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
//                }
//            }
//            else
//            {
//                // In ra lỗi của ModelState nếu không hợp lệ
//                var errors = ModelState.Values.SelectMany(v => v.Errors);
//                foreach (var error in errors)
//                {
//                    Console.WriteLine(error.ErrorMessage);
//                }
//            }

//            // Tạo lại các SelectList cho View trong trường hợp dữ liệu không hợp lệ
//            ViewData["IdCapKhenThuong"] = new SelectList(_context.DmCapKhenThuongs, "IdCapKhenThuong", "TenCapKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdCapKhenThuong);
//            ViewData["IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd"] = new SelectList(_context.DmThiDuaGiaiThuongKhenThuongs, "IdThiDuaGiaiThuongKhenThuong", "TenThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//            ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(_context.DmLoaiDanhHieuThiDuaGiaiThuongKhenThuongs, "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "TenLoaiDanhHieuThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
//            ViewData["IdPhuongThucKhenThuong"] = new SelectList(_context.DmPhuongThucKhenThuongs, "IdPhuongThucKhenThuong", "TenPhuongThucKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdPhuongThucKhenThuong);

//            // Quay lại view Create với dữ liệu hiện tại
//            return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//        }


//        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd = await _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.FindAsync(id);
//            if (tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == null)
//            {
//                return NotFound();
//            }
//            ViewData["IdCapKhenThuong"] = new SelectList(_context.DmCapKhenThuongs, "IdCapKhenThuong", "IdCapKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdCapKhenThuong);
//            ViewData["IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd"] = new SelectList(_context.DmThiDuaGiaiThuongKhenThuongs, "IdThiDuaGiaiThuongKhenThuong", "IdThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//            ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(_context.DmLoaiDanhHieuThiDuaGiaiThuongKhenThuongs, "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
//            ViewData["IdPhuongThucKhenThuong"] = new SelectList(_context.DmPhuongThucKhenThuongs, "IdPhuongThucKhenThuong", "IdPhuongThucKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdPhuongThucKhenThuong);
//            return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//        }

//        // POST: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd,IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong,IdDanhHieuThiDuaGiaiThuongKhenThuong,SoQuyetDinhKhenThuong,IdPhuongThucKhenThuong,NamKhenThuong,IdCapKhenThuong")] TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd)
//        {
//            if (id != tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdExists(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["IdCapKhenThuong"] = new SelectList(_context.DmCapKhenThuongs, "IdCapKhenThuong", "IdCapKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdCapKhenThuong);
//            ViewData["IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd"] = new SelectList(_context.DmThiDuaGiaiThuongKhenThuongs, "IdThiDuaGiaiThuongKhenThuong", "IdThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//            ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(_context.DmLoaiDanhHieuThiDuaGiaiThuongKhenThuongs, "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
//            ViewData["IdPhuongThucKhenThuong"] = new SelectList(_context.DmPhuongThucKhenThuongs, "IdPhuongThucKhenThuong", "IdPhuongThucKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd.IdPhuongThucKhenThuong);
//            return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//        }

//        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd = await _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds
//                .Include(t => t.IdCapKhenThuongNavigation)
//                .Include(t => t.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdNavigation)
//                .Include(t => t.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation)
//                .Include(t => t.IdPhuongThucKhenThuongNavigation)
//                .FirstOrDefaultAsync(m => m.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == id);
//            if (tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == null)
//            {
//                return NotFound();
//            }

//            return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//        }

//        // POST: DanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd = await _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.FindAsync(id);
//            if (tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd != null)
//            {
//                _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.Remove(tbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGdExists(int id)
//        {
//            return _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds.Any(e => e.IdDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd == id);
//        }
//    }
//}