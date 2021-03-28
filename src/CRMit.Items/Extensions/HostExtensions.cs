﻿using CRMit.Items.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace CRMit.Items.Extensions
{
    public static class HostExtensions
    {
        public static async Task RunWithTasksAsync(this IHost host)
        {
            var tasks = host.Services.GetServices<IStartupTask>();
            foreach (var task in tasks)
            {
                await task.ExecuteAsync();
            }
            await host.RunAsync();
        }
    }
}
