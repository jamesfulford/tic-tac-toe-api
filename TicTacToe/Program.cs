using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TicTacToe {
    /// <summary>
    /// A program which runs the Tic Tac Toe API server.
    /// </summary>
    public class Program {
        /// <summary>
        /// Builds and runs the Tic Tac Toe API server
        /// </summary>
        /// <param name="args"></param>
        public static void Main (string[] args) {
            CreateWebHostBuilder (args).Build ().Run ();
        }

        /// <summary>
        /// Prepares a default web host builder configuration.
        /// </summary>
        /// <param name="args">CLI args to pass to the default builder.</param>
        /// <returns>An IWebHostBuilder configuration.</returns>
        public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseStartup<Startup> ();
    }
}
