using BestStore.Data;
using BestStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BestStore.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext context;

        public InvoicesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: /Invoices
        public IActionResult Index(string? searchString)
        {
            var invoices = context.Invoices.AsQueryable();

            //if (!string.IsNullOrEmpty(searchString))
          //  {
              //  invoices = invoices.Where(i =>
            //        i.ClientName.Contains(searchString) ||
                //    i.Number.Contains(searchString) ||
                  //  i.Service.Contains(searchString));
           // }

           // ViewData["CurrentFilter"] = searchString;

            return View(invoices.ToList());
        }



        // GET: /Invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InvoiceDTO invoiceDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(invoiceDTO);
            }

            var invoice = new Invoice
            {
                Number = invoiceDTO.Number,
                Status = invoiceDTO.Status,
                IssueDate = invoiceDTO.IssueDate,
                DueDate = invoiceDTO.DueDate,
                Service = invoiceDTO.Service,
                UnitPrice = invoiceDTO.UnitPrice,
                Quantity = invoiceDTO.Quantity,
                ClientName = invoiceDTO.ClientName,
                Email = invoiceDTO.Email,
                Phone = invoiceDTO.Phone,
                Address = invoiceDTO.Address ?? ""
            };

            context.Invoices.Add(invoice);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Invoices/Edit/5
        public IActionResult Edit(int id)
        {
            var invoice = context.Invoices.Find(id);
            if (invoice == null)
            {
                return NotFound();
            }

            var dto = new InvoiceDTO
            {
                Id = invoice.Id,
                Number = invoice.Number,
                Status = invoice.Status,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                Service = invoice.Service,
                UnitPrice = invoice.UnitPrice,
                Quantity = invoice.Quantity,
                ClientName = invoice.ClientName,
                Email = invoice.Email,
                Phone = invoice.Phone,
                Address = invoice.Address
            };

            return View(dto);
        }

        // POST: /Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, InvoiceDTO dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var invoice = context.Invoices.Find(id);
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.Number = dto.Number;
            invoice.Status = dto.Status;
            invoice.IssueDate = dto.IssueDate;
            invoice.DueDate = dto.DueDate;
            invoice.Service = dto.Service;
            invoice.UnitPrice = dto.UnitPrice;
            invoice.Quantity = dto.Quantity;
            invoice.ClientName = dto.ClientName;
            invoice.Email = dto.Email;
            invoice.Phone = dto.Phone;
            invoice.Address = dto.Address;

            context.Update(invoice);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Invoices/Delete/5
        public IActionResult Delete(int id)
        {
            var invoice = context.Invoices.Find(id);
            if (invoice != null)
            {
                context.Invoices.Remove(invoice);
                context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
