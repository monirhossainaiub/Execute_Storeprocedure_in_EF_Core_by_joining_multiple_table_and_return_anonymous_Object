using Microsoft.Data.SqlClient;
using ProcFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcFinal.DAL
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeViewModel>> GetEmployees();
        Task<IEnumerable<EmployeeViewModel>> GetEmployeesByDepartmentId(int id);
        //Task<IEnumerable<EmployeeViewModel>> GetEmployeesByDepartmentIdFromSP(int id);

        IEnumerable<dynamic> GetEmployeeFromSP(string spName, IDictionary<string, object> parameters);
        //IEnumerable<dynamic> GetEmployeeFromSP(string spName, int id);
    }
}
