using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.API.Controllers
{
    [ApiController]
    [Route("api/values")]
    public class ValuesController : ControllerBase
    {
        public static List<string> _Values = Enumerable
            .Range(1, 10)
            .Select(i => $"value-{i}")
            .ToList();

        [HttpGet]
        public IActionResult Get() => Ok(_Values);


        [HttpGet("{index}")]
        public ActionResult<string> GetByIndex(int index)
        {
            if (index < 0) { return BadRequest(); }
            if (index >= _Values.Count) { return NotFound(); }
            return _Values[index];
        }


        [HttpPost]
        public ActionResult Add(string s)
        {
            _Values.Add(s);
            return Ok();
        }

        [HttpPut("{index}")]
        [HttpPut("edit/{index}")]
        public ActionResult Edit(int index, string s)
        {
            if (index < 0) { return BadRequest(); }
            if (index >= _Values.Count) { return NotFound(); }
            _Values[index] = s;

            return Ok();
        }

        [HttpDelete("{index}")]
        public ActionResult Delete(int index)
        {
            if(index<0) { return BadRequest(); }
            if (index >= _Values.Count) { return NotFound(); }
            _Values.RemoveAt(index);
            return Ok();
        }
    }
}
