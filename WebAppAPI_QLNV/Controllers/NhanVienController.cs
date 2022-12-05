using Microsoft.AspNetCore.Mvc;
using WebAppAPI_QLNV.Data;
using WebAppAPI_QLNV.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly MyDbContext _context;

        public NhanVienController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hàm danh sách nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpGet("DSNV")]
        public IActionResult GetAll()
        {
            var dsNhanVien = _context.NhanViens.ToList();
            return Ok(dsNhanVien);
        }

        /// <summary>
        /// Hàm thêm nhân viên
        /// </summary>
        /// <param name="nhanVien">Phần tử thêm vào DSNV</param>
        /// <returns></returns>
        [HttpPost("Create")]
        public IActionResult Create(NhanVienModel nhanVien)
        {
            try
            {
                var nhanvien = new NhanVien
                {
                    MaNV = nhanVien.MaNV,
                    HoTen = nhanVien.HoTen,
                    NamSinh = nhanVien.NamSinh,
                    GioiTinh = nhanVien.GioiTinh
                };
                var check = _context.NhanViens.SingleOrDefault(nv => nv.MaNV == nhanvien.MaNV);
                if (check != null)
                {
                    return BadRequest("MaNV đã tồn tại!!!");
                }

                if (nhanvien.MaNV == "" || nhanvien.HoTen == "")
                {
                    return BadRequest("Không được để trống!!!");
                }
                var dt = DateTime.Now;
                string year = dt.ToString("yyyy");
                if (nhanvien.NamSinh > int.Parse(year) || nhanvien.NamSinh < 1900)
                {
                    return BadRequest("Năm sinh không thực!!!");
                }
                _context.Add(nhanvien);
                _context.SaveChanges();
                return Ok(new
                {
                    Success = true,
                    Data = nhanvien
                });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Hàm tìm nhân viên theo mã nhân viên 
        /// </summary>
        /// <param name="id">MaNV cần tìm</param>
        /// <returns></returns>
        [HttpGet("FindByMaNV:{id}")]
        public IActionResult FindByMaNV(string id)
        {
            try
            {
                NhanVien nhanvien = _context.NhanViens.SingleOrDefault(nv => nv.MaNV == id);
                if (nhanvien == null)
                {
                    return NotFound();
                }
                return Ok(nhanvien);

            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Hàm tìm nhân viên theo Keyword
        /// </summary>
        /// <param name="id">MaNV cần tìm</param>
        /// <returns></returns>
        [HttpGet("FindByKeyword:{id}")]
        public IActionResult FindByKeyword(string id)
        {
            try
            {
                List<NhanVien> nhanViens = new List<NhanVien>();
                nhanViens.AddRange(_context.NhanViens.Where(nv => nv.MaNV == id || nv.HoTen.Contains(id)
                                                            || nv.NamSinh.ToString() == id));
                if (nhanViens.Count == 0)
                {
                    nhanViens.AddRange(_context.NhanViens.Where(nv => nv.GioiTinh == Convert.ToBoolean(id)));
                    return Ok(nhanViens);
                }
                return Ok(new
                {
                    Success = nhanViens.Count,
                    Data = nhanViens
                });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Cập nhập thông tin nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nhanVienEdit">MaNV cần cập nhập</param>
        /// <returns></returns>
        [HttpPut("Edit/{id}")]
        public IActionResult Edit(string id, [FromBody] NhanVien nhanVienEdit)
        {
            try
            {
                var nhanvien = _context.NhanViens.SingleOrDefault(nv => nv.MaNV == id);

                if (id != nhanvien.MaNV)
                {
                    return BadRequest();
                }
                if (nhanvien == null)
                {
                    return NotFound();
                }
                else
                {
                    //Update
                    nhanvien.HoTen = nhanVienEdit.HoTen;
                    nhanvien.NamSinh = nhanVienEdit.NamSinh;
                    nhanvien.GioiTinh = nhanVienEdit.GioiTinh;
                    _context.SaveChanges();
                    return Ok(nhanvien);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Xóa thông tin nhân viên 
        /// </summary>
        /// <param name="id">MaNV cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Remove:{id}")]
        public IActionResult Remove(string id)
        {
            try
            {
                var nhanvien = _context.NhanViens.SingleOrDefault(nv => nv.MaNV == id);
                if (nhanvien == null)
                {
                    return NotFound();
                }
                _context.Remove(nhanvien);
                _context.SaveChanges();
                return Ok(new
                {
                    Success = true,
                    Data = nhanvien
                });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Hàm sắp xếp DSNV theo HoTen
        /// </summary>
        /// <returns></returns>
        [HttpGet("SortHoTen")]
        public IActionResult SapXepHoTen()
        {
            var sapxep = _context.NhanViens.OrderBy(nv => nv.HoTen);
            List<NhanVien> nhanViens = new List<NhanVien>();
            nhanViens = sapxep.ToList();
            return Ok(nhanViens);
        }

        /// <summary>
        /// Hàm sắp xếp DSNV theo NamSinh
        /// </summary>
        /// <returns></returns>
        [HttpGet("SortNamSinh")]
        public IActionResult SapXepNamSinh()
        {
            var sapxep = _context.NhanViens.OrderBy(nv => nv.NamSinh);
            List<NhanVien> nhanViens = new List<NhanVien>();
            nhanViens = sapxep.ToList();
            return Ok(nhanViens);
        }
        
    }
}
