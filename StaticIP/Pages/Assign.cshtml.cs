using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using StaticIP.Models;

namespace StaticIP.Pages
{
    public class AssignModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public IPAddressModel IpAddress { get; set; }

        public AssignModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            IpAddress = GetById(id);

            return Page();
        }

        private IPAddressModel GetById(int id)
        {
            var IpRecord = new IPAddressModel();

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM ip_database WHERE Id = {id}";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    IpRecord.Id = reader.GetInt32(0);
                    IpRecord.VFourAddress = reader.GetString(1);
                    IpRecord.InUse = reader.GetBoolean(2);
                }

                return IpRecord;

            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"UPDATE ip_database SET VFourAddress ='{IpAddress.VFourAddress}', InUse = 1 WHERE Id = {IpAddress.Id}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToPage("./AssignIP");
        }
    }
}
