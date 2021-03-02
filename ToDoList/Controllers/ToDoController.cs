using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext toDoContext)
        {
            _context = toDoContext;
        }
        
        // GET /
        public async Task<ActionResult> Index()
        {
            IQueryable<TodoList> items = from i in _context.ToDoLists orderby i.Id select i;

            List<TodoList> todoLists = await items.ToListAsync();
            
            return View(todoLists);
        }
        
        // GET /todo/create
        public IActionResult Create() => View();
        
        // POST /todo/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoList item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();

                TempData["Succes"] = "The Item has been added";

                return RedirectToAction("Index");
            }
            
            return View(item);
        }
        
        // GET /todo/edit/{5}
        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await _context.ToDoLists.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            
            return View(item);
        }
        
        // POST /todo/edit/{5}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();

                TempData["Succes"] = "The Item has been updated";

                return RedirectToAction("Index");
            }
            
            return View(item);
        }
        
        // GET /todo/delete/{5}
        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await _context.ToDoLists.FindAsync(id);

            if (item == null)
            {
                TempData["Error"] = "The Item does not exist !";
            }
            else
            {
                _context.ToDoLists.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Succes"] = "The Item has been deleted";
            }
            
            return RedirectToAction("Index");
        }
        
    }
}