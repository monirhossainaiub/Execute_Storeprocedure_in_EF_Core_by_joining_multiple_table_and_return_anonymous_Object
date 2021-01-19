using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProcFinal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProcFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        // GET: api/<EmployeesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var emp = await employeeRepository.GetEmployees();
            return Ok(emp);
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IDictionary<string, object> sqlParams = new Dictionary<string, object>();
            sqlParams.Add("Id", id);
            string spName = "procedure_name";
            var emp = employeeRepository.GetEmployeeFromSP(spName, sqlParams);
            return Ok(emp);
        }
        
    }
}
