using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using StaticIP.Models;

namespace StaticIP.Pages
{
    public class AddIPModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<IPAddressModel> Addresses { get; set; }

        public AddIPModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Addresses = GetOpenAddresses();
        }

        private List<IPAddressModel> GetOpenAddresses()
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM ip_database WHERE InUse = 0 ";

                var tableData= new List<IPAddressModel>();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    tableData.Add(
                        new IPAddressModel()
                        {
                            Id = reader.GetInt32(0),
                            VFourAddress = reader.GetString(1),
                            InUse = reader.GetBoolean(2)
                        });
                }

                return tableData;
            }
        }
    }
}
