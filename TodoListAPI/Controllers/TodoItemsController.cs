﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoListAPI.Data;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public TodoItemsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public List<TodoItem> Get()
        {
            return _db.TodoItems.OrderBy(x => x.IsDone).ToList();
        }
        
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id)
        {
            TodoItem todoItem = _db.TodoItems.Find(id);
            if (todoItem == null) return NotFound();

            return todoItem;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                _db.TodoItems.Update(todoItem);
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest(ModelState);
        }


        [HttpPost]
        public ActionResult<TodoItem> Post(TodoItem todoItem)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _db.TodoItems.Add(todoItem);
            _db.SaveChanges();
            return todoItem;
        }

        [HttpDelete("{id}")] // get gibi çalışır. veri gönderilmez Query stringler ile haberleşir
        public IActionResult Delete(int id)
        {
            TodoItem todoItem = _db.TodoItems.Find(id);
            if (todoItem == null) return NotFound();
            _db.TodoItems.Remove(todoItem);
            _db.SaveChanges();
            return NoContent(); // 204 ile 200 farkına bak.
        }
    }
}
