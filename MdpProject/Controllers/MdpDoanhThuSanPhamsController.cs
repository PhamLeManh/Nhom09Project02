using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MdpProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MdpProject.Controllers
{
    public class MdpDoanhThuSanPhamsController : Controller
    {
        private readonly Project2Context _context;

        public MdpDoanhThuSanPhamsController(Project2Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? year, int? month)
        {
            var doanhThuQuery = _context.MdpDoanhThuSanPhams
                .Include(d => d.MdpSanPham)
                .AsQueryable();

            if (year.HasValue)
                doanhThuQuery = doanhThuQuery.Where(d => d.MdpNam == year);

            if (month.HasValue)
                doanhThuQuery = doanhThuQuery.Where(d => d.MdpThang == month);

            var doanhThuList = await doanhThuQuery.ToListAsync();

            // Gom nhóm theo sản phẩm gốc
            var tongHopTheoSanPham = doanhThuList
                .GroupBy(d => d.MdpSanPham.MdpTenSanPham)
                .Select(g => new
                {
                    SanPham = g.Key,
                    TongSoLuong = g.Sum(x => x.MdpTongSoLuongBan ?? 0),
                    TongDoanhThu = g.Sum(x => x.MdpTongDoanhThu ?? 0)
                })
                .OrderByDescending(x => x.TongDoanhThu)
                .ToList();

            // Dữ liệu dropdown
            var years = await _context.MdpDoanhThuSanPhams
                .Select(d => d.MdpNam)
                .Distinct()
                .Where(y => y.HasValue)
                .OrderByDescending(y => y)
                .Select(y => y.Value)
                .ToListAsync();

            ViewBag.Years = years;
            ViewBag.Months = Enumerable.Range(1, 12).ToList();
            ViewBag.SelectedYear = year;
            ViewBag.SelectedMonth = month;

            // Tổng doanh thu toàn bộ
            ViewBag.TongDoanhThu = tongHopTheoSanPham.Sum(d => d.TongDoanhThu);

            // Dữ liệu biểu đồ
            ViewBag.ChartLabels = tongHopTheoSanPham.Select(c => c.SanPham).ToList();
            ViewBag.ChartValues = tongHopTheoSanPham.Select(c => c.TongDoanhThu).ToList();

            return View(tongHopTheoSanPham);
        }
    }
}
