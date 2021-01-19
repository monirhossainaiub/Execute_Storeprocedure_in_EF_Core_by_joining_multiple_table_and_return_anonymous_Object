//Execute store procedure from joining multiple table   

public IEnumerable<dynamic> GetEmployeeFromSP(string spName, IDictionary<string, object> parameters)
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
        
        
        
        
        
        
        //call 
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IDictionary<string, object> sqlParams = new Dictionary<string, object>();
            sqlParams.Add("Id", id);
            //you can add multiple paramenter
            string spName = "procedure_name";
            var emp = employeeRepository.GetEmployeeFromSP(spName, sqlParams);
            return Ok(emp);
        }
        
