using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace UsuariosApi.Test
{
    public static class TestHelper
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
    }
}
