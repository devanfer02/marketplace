using System.Diagnostics;

namespace Marketplace.Packages.HostedServices
{
    public class TailwindHostedService : IHostedService
    {
        private Process _process;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c npm run css:build",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            })!;

            _process.EnableRaisingEvents = true;
            _process.OutputDataReceived += (_, e) => Console.WriteLine($"Tailwind: {e.Data}");
            _process.ErrorDataReceived += (_, e) => Console.WriteLine($"Tailwind: {e.Data}");

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Tailwind: STOP");
            _process?.Kill(entireProcessTree: true);
            return Task.CompletedTask;
        }
    }
}
