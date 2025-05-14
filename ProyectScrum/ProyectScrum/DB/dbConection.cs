using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace ProyectScrum.Data
{
    public class SqlDataAccess
    {
        private readonly string _connectionString;

        public SqlDataAccess()
        {
            //connection de Pc Jose
            //_connectionString = "Data Source=LAPTOP-PHCFNULN\\SQLEXPRESS;Initial Catalog=proyectoDBSS;Integrated Security=True;";

            //connection de Rene
            //_connectionString = "Data Source=AlePC\\SQLEXPRESS;Initial Catalog=proyectoDBS2;Integrated Security=True;";

            //connection de Pc ken
            //_connectionString = "Data Source=DESKTOP-OKJJS3Y\\SQLEXPRESS;Initial Catalog=proyectoDBS2;Integrated Security=True;";

            //connection de laptop ken
            _connectionString = "Data Source=PUTAKU\\SQLEXPRESS;Initial Catalog=proyectoDBS2;Integrated Security=True;";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
