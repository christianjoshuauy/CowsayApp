using System.Diagnostics;

namespace CowsayApp
{
    abstract class App
    {
        protected abstract string FileName { get; }

        protected abstract string GetArgument();

        public void RunProgram()
        {
            try
            {
                string argument = GetArgument();
                ExecuteProcess(FileName, argument);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running cowsay: {ex.Message}");
            }
        }

        private void ExecuteProcess(string program, string argument)
        {
            try
            {
                using (Process process = new()
                {
                    StartInfo = new()
                    {
                        FileName = program,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true
                    }
                })
                {
                    process.Start();

                    try
                    {
                        using (StreamWriter writer = process.StandardInput)
                        {
                            writer.WriteLine(argument);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error writing to process stdin: {ex.Message}");
                    }

                    try
                    {
                        using (StreamReader reader = process.StandardOutput)
                        {
                            Console.WriteLine(reader.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading from process stdout: {ex.Message}");
                    }

                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting process: {ex.Message}");
            }
        }
    }

    class Cowsay : App
    {
        protected override string FileName => "cowsay";

        protected override string GetArgument()
        {
            Console.Write("Enter what cow says: ");
            return Console.ReadLine() ?? "Moo";
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Cowsay cowsay = new();
            cowsay.RunProgram();
        }
    }
}