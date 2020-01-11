using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfCoreParallelSample.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreParallelSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        SampleContext _context;
        public ValuesController(SampleContext context)
        {
            _context = context;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // An attempt was made to use the context while it is being configured.
            // A DbContext instance cannot be used inside OnConfiguring since it is still being configured at this point.
            // This can happen if a second operation is started on this context before a previous operation completed. 
            // Any instance members are not guaranteed to be thread safe.
            // for (int i = 0; i < 10; i++)
            // {
            //     _context.InventoryItems.Add(new InventoryItem() { Name = "test" + i });
            // }
            var list = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            Parallel.ForEach(list, async (i) =>
                {
                    await GetAsync(i);
                });

            _context.SaveChanges();
            return Ok(new string[] { "value1", "value2" });
        }

        public async Task GetAsync(int i)
        {
            var connectionstring = "Server=127.0.0.1,1433;Database=EfCoreSample;User Id=sa;password=Admin123;";

            var optionsBuilder = new DbContextOptionsBuilder<SampleContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            using (var _context = new SampleContext(optionsBuilder.Options))
            {
                var item = await _context.InventoryItems.Where(x => x.Id == i).FirstOrDefaultAsync();
                Console.WriteLine(item.Name);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
