using System.Runtime.InteropServices;

class Program
{
    // Imports
    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo); // mouse click

    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(int vKey);

    // Class variables
    const uint LEFTDOWN = 0x02;
    const uint LEFTUP = 0x04;
    const int HOTKEY = 0x5A; // 'Z' key
    const int EXITKEY = 0x51; // 'Q' key
    private const int OPTIONKEY = 0x4F;

    bool enableClicker = false;
    int clickInterval = 10;
    bool isHotkeyPressed = false;

    // Mouse click event
    void MouseClick()
    {
        while (true)
        {
            if (enableClicker)
            {
                mouse_event(LEFTDOWN, 0, 0, 0, IntPtr.Zero);
                mouse_event(LEFTUP, 0, 0, 0, IntPtr.Zero);
            }
            Thread.Sleep(clickInterval);
        }
    }

    // Main loop
    void RunClicker()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Clicker Ready, Z to Start/Stop, Q to Quit.");
        Console.WriteLine($"Current Click Interval is {clickInterval} ms. Press O to Change Interval.");
        Console.ResetColor();
        
        while (true)
        {
            if (GetAsyncKeyState(OPTIONKEY) < 0)
            {
                enableClicker = false;
                isHotkeyPressed = false;
                AdjustClickInterval();
                break;
            }
            if (GetAsyncKeyState(EXITKEY) < 0)
            {
                Environment.Exit(0); // Exit the application
            }
            if (GetAsyncKeyState(HOTKEY) < 0)
            {
                if (!isHotkeyPressed)
                {
                    // Key was just pressed down
                    enableClicker = !enableClicker;
                    isHotkeyPressed = true;
                    Console.WriteLine(enableClicker ? "Clicker Started" : "Clicker Stopped");
                }
            }
            else
            {
                // Key was released
                isHotkeyPressed = false;
            }
            Thread.Sleep(5);
        }
    }

    void AdjustClickInterval()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Enter new click interval in milliseconds:");
        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        // Clear the input buffer
        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }
        
        // Add a short delay to ensure the buffer is cleared
        Thread.Sleep(50);
        
        if (int.TryParse(Console.ReadLine(), out int newInterval) && newInterval > 0)
        {
            clickInterval = newInterval;
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Console.WriteLine($"Click interval updated to {clickInterval} ms.");
            Console.ResetColor();
            RunClicker();
        }
        else
        {
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Console.WriteLine("Invalid input. Please enter a positive integer.");
            Console.ResetColor();
            RunClicker();
        }
    }

    static void Main(string[] args)
    {
        Program program = new Program();
        
        Thread clickerThread = new Thread(new ThreadStart(program.MouseClick));
        clickerThread.Start();
        
        program.RunClicker();

    }
}