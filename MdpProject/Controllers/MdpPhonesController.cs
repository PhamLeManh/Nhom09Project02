using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MdpProject.Models;

namespace MdpProject.Controllers
{
    public class MdpPhonesController : Controller
    {
        private readonly Project2Context _context;

        public MdpPhonesController(Project2Context context)
        {
            _context = context;
        }

        // GET: MdpPhones
        public async Task<IActionResult> Index()
        {
            var project2Context = _context.MdpPhones.Include(m => m.MdpSanPham);
            return View(await project2Context.ToListAsync());
        }

        // GET: MdpPhones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mdpPhone = await _context.MdpPhones
                .Include(m => m.MdpSanPham)
                .FirstOrDefaultAsync(m => m.MdpPhoneId == id);
            if (mdpPhone == null)
            {
                return NotFound();
            }

            return View(mdpPhone);
        }

        // GET: MdpPhones/Create
        public IActionResult Create()
        {
            ViewData["MdpSanPhamId"] = new SelectList(_context.MdpSanPhams, "MdpSanPhamId", "MdpTenSanPham");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MdpLaptopId,MdpSanPhamId,MdpTenLaptop,MdpCpu,MdpRam,MdpStorage,MdpGpu,MdpManHinh,MdpPin,MdpHeDieuHanh")] MdpLaptop mdpLaptop)
        {
            if (ModelState.IsValid)
            {
                mdpLaptop.MdpCreatedAt = DateTime.Now;
                _context.Add(mdpLaptop);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "✅ Thêm Laptop thành công!";
                return RedirectToAction("MdpAdminIndex");
            }

            TempData["ErrorMessage"] = "❌ Thêm Laptop thất bại!";
            ViewData["MdpSanPhamId"] = new SelectList(_context.MdpSanPhams, "MdpSanPhamId", "MdpTenSanPham", mdpLaptop.MdpSanPhamId);
            return View(mdpLaptop);
        }


        // GET: MdpPhones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mdpPhone = await _context.MdpPhones.FindAsync(id);
            if (mdpPhone == null)
            {
                return NotFound();
            }
            ViewData["MdpSanPhamId"] = new SelectList(_context.MdpSanPhams, "MdpSanPhamId", "MdpSanPhamId", mdpPhone.MdpSanPhamId);
            return View(mdpPhone);
        }

        // POST: MdpPhones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MdpPhoneId,MdpSanPhamId,MdpTenPhone,MdpManHinh,MdpCamera,MdpPin,MdpRam,MdpStorage,MdpChipset,MdpHeDieuHanh,MdpCreatedAt,MdpUpdatedAt")] MdpPhone mdpPhone)
        {
            if (id != mdpPhone.MdpPhoneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mdpPhone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MdpPhoneExists(mdpPhone.MdpPhoneId))
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
            ViewData["MdpSanPhamId"] = new SelectList(_context.MdpSanPhams, "MdpSanPhamId", "MdpSanPhamId", mdpPhone.MdpSanPhamId);
            return View(mdpPhone);
        }

        // GET: MdpPhones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mdpPhone = await _context.MdpPhones
                .Include(m => m.MdpSanPham)
                .FirstOrDefaultAsync(m => m.MdpPhoneId == id);
            if (mdpPhone == null)
            {
                return NotFound();
            }

            return View(mdpPhone);
        }

        // POST: MdpPhones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mdpPhone = await _context.MdpPhones.FindAsync(id);
            if (mdpPhone != null)
            {
                _context.MdpPhones.Remove(mdpPhone);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MdpPhoneExists(int id)
        {
            return _context.MdpPhones.Any(e => e.MdpPhoneId == id);
        }
    }
}
