using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProcFinal.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcFinal.DAL
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<IEnumerable<EmployeeViewModel>> GetEmployees()
        {
            List<EmployeeViewModel> products = new List<EmployeeViewModel>();
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                string queryString = @"SELECT e.Id, e.Name, d.Name as Department FROM	Employees e
                                LEFT JOIN Departments d ON e.DepartmentId  = d.Id";


                command.CommandText = queryString;
               // command.Parameters.Add(new SqlParameter("@Id", publisherId));
                //command.Parameters.Add(new SqlParameter("@isPublished", isPublished));

                context.Database.OpenConnection();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            EmployeeViewModel product = new EmployeeViewModel();
                            product.Id = Convert.ToInt32(reader["Id"]);
                            product.Name = reader["Name"].ToString();
                            product.Department = product.Name = reader["Department"].ToString();
                           
                            products.Add(product);
                        }

                    }

                }

            }

            return products;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesByDepartmentId(int id)
        {
            

            List<EmployeeViewModel> products = new List<EmployeeViewModel>();
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                string queryString = @"SELECT e.Id, e.Name, d.Name as Department FROM	Employees e
                            LEFT JOIN Departments d ON e.DepartmentId  = d.Id
                            WHERE d.Id = @Id";

             
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@Id", id));

                context.Database.OpenConnection();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            EmployeeViewModel product = new EmployeeViewModel();
                            product.Id = Convert.ToInt32(reader["Id"]);
                            product.Name = reader["Name"].ToString();
                            product.Department = product.Name = reader["Department"].ToString();

                            products.Add(product);
                        }

                    }

                }

            }

            return products;
        }


        

        public IEnumerable<dynamic> GetEmployeeFromSP(string spName, IDictionary<string, object> parameters)
        //public IEnumerable<dynamic> GetEmployeeFromSP(string spName, int id)
        {
            
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                
                command.CommandText = spName;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // Create the dynamic result for each row
                
                {
                    foreach (var parameter in parameters)
                    {
                        var key = parameter.Key;
                        var value = parameter.Value;
                        command.Parameters.Add(new SqlParameter($"@{key}", value));
                        
                    }
                }

                using (command)
                {
                   

                    if (command.Connection.State == System.Data.ConnectionState.Closed)
                        command.Connection.Open();
                    //await command.Connection.OpenAsync();
                    

                    try
                    {
                        //using (var reader = await command.ExecuteReaderAsync())
                        using (var reader = command.ExecuteReader())
                        {
                            // List for column names
                            var names = new List<string>();

                            if (reader.HasRows)
                            {
                                // Add column names to list
                                for (var i = 0; i < reader.VisibleFieldCount; i++)
                                {
                                    names.Add(reader.GetName(i));
                                }

                                while (reader.Read())
                                {
                                    //// Create the dynamic result for each row
                                    var result = new ExpandoObject() as IDictionary<string, object>;

                                    foreach (var name in names)
                                    {
                                        result.Add(name, reader[name]);
                                    }

                                    yield return result;
                                }

                            }

                        }
                    }
                    finally
                    {
                        command.Connection.Close();
                    }
                }
            }
        }
    }
}
