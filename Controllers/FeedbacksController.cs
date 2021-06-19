using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FeedbackPortal.Models;
using FeedbackPortal.ViewModels;

namespace FeedbackPortal.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly FeedbackPortalContext _context;

        public FeedbacksController(FeedbackPortalContext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index(string feedbackType, string searchString)
        {
            IQueryable<Feedback> feedbacks = _context.Feedback.AsQueryable();
            IQueryable<string> typeQuery = _context.Feedback.OrderBy(f => f.Type).Select(f => f.Type).Distinct();
            if (!string.IsNullOrEmpty(searchString))
            {
                feedbacks = feedbacks.Where(s => s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(feedbackType))
            {
                feedbacks = feedbacks.Where(x => x.Type == feedbackType);
            }
            feedbacks = feedbacks.Include(m => m.Client)
            .Include(m => m.Product);
            var feedbackFilterVM = new FeedbackFilterViewModel
            {
                Types = new SelectList(await typeQuery.ToListAsync()),
                Feedbacks = await feedbacks.ToListAsync()
            };
            return View(feedbackFilterVM);
            // var feedbackPortalContext = _context.Feedback.Include(f => f.Client).Include(f => f.Product);
            //return View(await feedbackPortalContext.ToListAsync());
        }

        // GET: Feedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback
                .Include(f => f.Client)
                .Include(f => f.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Details,Type,ClientId,ProductId")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name", feedback.ClientId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", feedback.ProductId);
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name", feedback.ClientId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", feedback.ProductId);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Details,Type,ClientId,ProductId")] Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name", feedback.ClientId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", feedback.ProductId);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback
                .Include(f => f.Client)
                .Include(f => f.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _context.Feedback.FindAsync(id);
            _context.Feedback.Remove(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedback.Any(e => e.Id == id);
        }
    }
}
