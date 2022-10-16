using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        static List<TodoList> _todoList = new List<TodoList>();

        static TodoController()
        {
            _todoList.Add(new TodoList { Id = 1, Name = "John", IsComplete = false });
            _todoList.Add(new TodoList { Id = 2, Name = "Peter", IsComplete = true });
        }

        
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_todoList);
        }

        
        [HttpGet("{id}",Name = "GetTodo")]
        public ActionResult Get(int id)
        {
            var foundTodoItem = _todoList.Where(x => x.Id.Equals(id)).FirstOrDefault();

            if (foundTodoItem == null)
            {
                return NotFound();
            }
            return Ok(foundTodoItem);
        }

        
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var foundTodoList = _todoList.Where(x => x.Id.Equals(id)).FirstOrDefault();

            if (foundTodoList != null)
            {
                _todoList.Remove(foundTodoList);
            }
            return NoContent();
        }

        
        [HttpPost]
        public ActionResult Post([FromBody] TodoList todoList)
        {
           
            try
            {
                if (todoList == null) return BadRequest();
                _todoList.Add(todoList);
                return CreatedAtRoute("GetTodo", new { id = todoList.Id }, todoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put([FromBody] TodoList todoList, int id)
        {
            try
            {
                if (todoList == null || todoList.Id != id)
                {
                    return BadRequest();
                }
                var foundTodoList = _todoList.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (foundTodoList == null)
                {
                    return NotFound();
                }
                else
                {
                    foundTodoList.Name = todoList.Name;
                    foundTodoList.IsComplete = todoList.IsComplete;
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
