using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace sudo {
    class Program {
        private static Process proc;

        static void Main(string[] args) {
            if (args.Count() == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    sudo [command] ...\r\n");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var startInfo = new ProcessStartInfo() {
                FileName = args[0],
                UseShellExecute = false,
                Verb = "runAs",
                Arguments = new Func<IEnumerable<string>, string>((argsInShell) => {
                    return string.Join(" ", argsInShell);
                })(args.Skip(1)),
                RedirectStandardError = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = true
            };

            proc = new Process();
            proc.StartInfo = startInfo;
            proc.OutputDataReceived += OnDataReceived;
            proc.ErrorDataReceived += OnDataReceived;

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            proc.WaitForExit();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void OnDataReceived(object Sender, DataReceivedEventArgs e) {
            if (e.Data != null) {
                Console.WriteLine(e.Data);
            }
        }

    }
}
