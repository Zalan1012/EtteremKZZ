using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace EtteremKZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendegController : ControllerBase
    {
        public string ConnectionString = "server=localhost;port=3307;database=etterem;uid=root;password=";

        [HttpGet("vendegadatok")]
        public object GetVendegAdatok([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"SELECT `name`, `email` FROM `vendeg` WHERE `id` = @id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            if (datareader.Read())
            {
                var vendeg = new
                {
                    Name = datareader.GetString(0),
                    Email = datareader.GetString(1)
                };
                connector.Close();
                return new { message = "Sikeres", result = vendeg };
            }

            return new { message = "Nincsen" };
        }


        [HttpGet("vendegrendelesei")]
        public object GetVendegRendelesei([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"SELECT vendeg.name, rendeles.dish, rendeles.description FROM vendeg, rendeles WHERE vendeg.id = rendeles.vendegId AND vendeg.id = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            if (datareader.Read())
            {
                var eredmeny = new
                {
                    Name = datareader.GetString(0),
                    Dish = datareader.GetString(1),
                    Description = datareader.GetString(2)
                };

                connector.Close();
                return new { message = "Sikeres.", result = eredmeny };
            }

            connector.Close();
            return new { message = "Nincsen" };
        }



        [HttpGet("osszesrendelesszama")]
        public object GetOsszesRendelesSzama()
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"SELECT COUNT(*) FROM `rendeles`;";
            var cmd = new MySqlCommand(sql, connector);

            int darab = Convert.ToInt32(cmd.ExecuteScalar());

            connector.Close();

            return new { message = "Sikeres.", result = darab };
        }



        [HttpGet("vendegrendelesszama")]
        public object GetVendegRendelesSzama([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"SELECT COUNT(*) FROM `rendeles` WHERE `vendegId` = @id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            int darab = Convert.ToInt32(cmd.ExecuteScalar());

            connector.Close();

            return new { message = "Sikeresen lekerdezvee", result = darab };
        }
    }
}
