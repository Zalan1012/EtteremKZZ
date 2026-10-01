using EtteremKZ.Models;
using EtteremKZ.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Reflection.Metadata.Ecma335;

namespace EtteremKZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RendelesController : ControllerBase
    {
        public string ConnectionString = "server=localhost;port=3307;database=etterem;uid=root;password=";

        [HttpGet("all")]
        public object GetAllRendeles()
        {
            List<Rendeles> rendelesek = new List<Rendeles>();

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"SELECT * FROM `rendeles`";
            var cmd = new MySqlCommand(sql, connector);
            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var rendeles = new Rendeles
                {
                    Id = datareader.GetInt32(0),
                    Dish = datareader.GetString(1),
                    Description = datareader.GetString(2),
                    OrderTime = datareader.GetDateTime(3),
                    UpdateTime = datareader.GetDateTime(4),
                    VendegId = datareader.GetInt32(5)
                };

                rendelesek.Add(rendeles);
            }

            connector.Close();
            return new { message = "Sikeruklht a lekrdezes", result = rendelesek };

        }

        [HttpGet("byId")]
        public object GetRendelesById([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"SELECT * FROM `rendeles` WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            datareader.Read();

            var rendeles = new Rendeles
            {
                Id = datareader.GetInt32(0),
                Dish = datareader.GetString(1),
                Description = datareader.GetString(2),
                OrderTime = datareader.GetDateTime(3),
                UpdateTime = datareader.GetDateTime(4),
                VendegId = datareader.GetInt32(5)
            };

            connector.Close();

            return new { message = "sikerese talalat.", result = rendeles };
        }

        [HttpPost("add")]
        public object AddNewRendeles([FromBody] CreateRendelesDto createRendelesDto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"INSERT INTO `rendeles`(`dish`, `description`, `orderTime`, `updateTime`, `vendegId`) VALUES (@dish, @description, @orderTime, @updateTime, @vendegId)";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@dish", createRendelesDto.Dish);
            cmd.Parameters.AddWithValue("@description", createRendelesDto.Description);
            cmd.Parameters.AddWithValue("@orderTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@vendegId", createRendelesDto.VendegId);

            cmd.ExecuteNonQuery();

            connector.Close();

            return new { message = "Hozza letta adva.", result = createRendelesDto };
        }

        [HttpPut("update")]
        public object UpdateRendeles([FromQuery] int id, [FromBody] UpdateRendelesDto updateRendelesDto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"UPDATE `rendeles` SET `dish`=@dish, `description`=@description, `updateTime`=@updateTime, `vendegId`=@vendegId WHERE `id`= @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@dish", updateRendelesDto.Dish);
            cmd.Parameters.AddWithValue("@description", updateRendelesDto.Description);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@vendegId", updateRendelesDto.VendegId);
            cmd.Parameters.AddWithValue("@id", id);

            object result = cmd.ExecuteNonQuery() > 0 ? new { message = "Frisstive." } : new { message = "Nincs ilyen" };

            connector.Close();

            return result;
        }


        [HttpDelete("delete")]
        public object DeleteRendeles([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = @"DELETE FROM `rendeles` WHERE `id`= @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@id", id);

            object result = cmd.ExecuteNonQuery() > 0 ? new { message = "Torolve." } : new { message = "Nincs ilyen" };

            connector.Close();

            return result;

        }
    }
}
