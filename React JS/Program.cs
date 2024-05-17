using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the App Creator!");
        Console.WriteLine("Enter 'new react appname' to create a new React app.");
        Console.WriteLine("Enter 'new reactnative appname' to create a new React Native app.");
        Console.WriteLine("Enter 'exit' to quit the application.");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (input == "exit")
            {
                break;
            }

            string[] commandParts = input.Split(' ');
            if (commandParts.Length == 3 && commandParts[0] == "new")
            {
                string framework = commandParts[1];
                string appName = commandParts[2];

                if (framework == "react")
                {
                    CreateReactApp(appName);
                }
                else if (framework == "reactnative")
                {
                    CreateReactNativeApp(appName);
                }
                else
                {
                    Console.WriteLine("Invalid framework. Please use 'react' or 'reactnative'.");
                }
            }
            else
            {
                Console.WriteLine("Invalid command. Please use 'new framework appname' or type 'exit' to quit.");
            }
        }
    }

    static void CreateReactApp(string appName)
    {
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string desktopPath = Path.Combine(userProfile, "Desktop");
        string appPath = Path.Combine(desktopPath, appName);

        try
        {
            // Run 'npx create-react-app my-app'
            RunCommand("npx create-react-app", appName, desktopPath);

            // Open the app folder with Visual Studio Code
            RunCommand("code", appPath, desktopPath);

            // Start the app with 'npm start'
            RunCommand("npm start", "", appPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void CreateReactNativeApp(string appName)
    {
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string desktopPath = Path.Combine(userProfile, "Desktop");
        string appPath = Path.Combine(desktopPath, appName);

        try
        {
            // Stop all running servers
            StopAllServers();

            // Run 'npx create-expo-app@latest my-app'
            RunCommand("npx create-expo-app@latest", appName, desktopPath);

            // Open the app folder with Visual Studio Code
            RunCommand("code", appPath, desktopPath);

            // Start the app with 'npm start'
            RunCommand("npm start", "", appPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void StopAllServers()
    {
        try
        {
            RunCommand("taskkill /F /IM node.exe", "", "");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while stopping servers: {ex.Message}");
        }
    }

    static void RunCommand(string command, string arguments, string workingDirectory = "")
    {
        string fullCommand = $"{command} {arguments}";
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/C {fullCommand}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory
        };

        using (Process process = new Process { StartInfo = processStartInfo })
        {
            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
    }
}
