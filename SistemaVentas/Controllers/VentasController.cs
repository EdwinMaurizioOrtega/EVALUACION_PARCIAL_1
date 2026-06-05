using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.Models;

namespace SistemaVentas.Controllers
{
    public class VentasController : Controller
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Producto)
                .ToListAsync();
            return View(ventas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Producto)
                .FirstOrDefaultAsync(m => m.venta_id == id);

            if (venta == null) return NotFound();

            return View(venta);
        }

        public IActionResult Create()
        {
            ViewBag.Clientes = _context.Clientes.ToList()
                .Select(c => new SelectListItem { Value = c.cliente_id.ToString(), Text = $"{c.nombre} {c.apellido}" })
                .ToList();
            ViewBag.Productos = new SelectList(_context.Productos.Where(p => p.stock > 0).ToList(), "producto_id", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("cliente_id,producto_id,cantidad")] VentaModel venta)
        {
            var producto = await _context.Productos.FindAsync(venta.producto_id);

            if (producto == null)
            {
                ModelState.AddModelError("", "Producto no encontrado.");
            }
            else if (producto.stock < venta.cantidad)
            {
                ModelState.AddModelError("cantidad", $"Stock insuficiente. Disponible: {producto.stock}");
            }

            if (ModelState.IsValid)
            {
                venta.precio_unitario = producto!.precio;
                venta.total = venta.cantidad * venta.precio_unitario;
                venta.fecha_venta = DateTime.Now;

                producto.stock -= venta.cantidad;

                _context.Add(venta);
                _context.Update(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = _context.Clientes.ToList()
                .Select(c => new SelectListItem { Value = c.cliente_id.ToString(), Text = $"{c.nombre} {c.apellido}", Selected = c.cliente_id == venta.cliente_id })
                .ToList();
            ViewBag.Productos = new SelectList(_context.Productos.Where(p => p.stock > 0).ToList(), "producto_id", "nombre", venta.producto_id);
            return View(venta);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Producto)
                .FirstOrDefaultAsync(m => m.venta_id == id);

            if (venta == null) return NotFound();

            return View(venta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.Include(v => v.Producto).FirstOrDefaultAsync(v => v.venta_id == id);
            if (venta != null)
            {
                if (venta.Producto != null)
                {
                    venta.Producto.stock += venta.cantidad;
                    _context.Update(venta.Producto);
                }
                _context.Ventas.Remove(venta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
