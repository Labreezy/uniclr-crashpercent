using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using vJoyInterfaceWrap;
using System.Timers;
using WindowsInput;
using WindowsInput.Native;
namespace KBPS4
{
    class Program {

        [DllImport("C:\\Users\\yourt\\source\\repos\\KBPS4\\SDK\\lib\\amd64\\vJoyInterface.dll")]
        static extern bool vJoyEnabled();
        [DllImport("C:\\Users\\yourt\\source\\repos\\KBPS4\\SDK\\lib\\amd64\\vJoyInterface.dll")]
        static extern bool isVJDExists(uint rid);
        [DllImport("C:\\Users\\yourt\\source\\repos\\KBPS4\\SDK\\lib\\amd64\\vJoyInterface.dll")]
        static extern ushort GetvJoyVersion();
        [DllImport("C:\\Users\\yourt\\source\\repos\\KBPS4\\SDK\\lib\\amd64\\vJoyInterface.dll")]
        static extern bool DriverMatch(IntPtr DllVer, IntPtr DrvVer);
        [DllImport("C:\\Users\\yourt\\source\\repos\\KBPS4\\SDK\\lib\\amd64\\vJoyInterface.dll")]
        static extern bool AcquireVJD(uint rid);
        [DllImport("C:\\Users\\yourt\\source\\repos\\KBPS4\\SDK\\lib\\amd64\\vJoyInterface.dll")]
        static extern bool ResetAll();
        [DllImport("C:\\Users\\yourt\\source\\repos\\KBPS4\\SDK\\lib\\amd64\\vJoyInterface.dll")]
        static extern bool SetBtn(bool value, uint rid, byte nBtn);
        [DllImport("C:\\Users\\yourt\\source\\repos\\KBPS4\\SDK\\lib\\amd64\\vJoyInterface.dll")]
        static extern bool SetContPov(int value, uint rid, byte nPov);
        string dlldir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        
        Keys[] directions = { Keys.D, Keys.Space, Keys.A, Keys.S };
        Keys[] buttons = { Keys.NumPad1, Keys.NumPad2, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad3, Keys.OemOpenBrackets, Keys.OemSemicolon, Keys.OemQuotes };
        int MAX_DIR = 36000;
        double[] dir_active = { 0, 0, 0, 0};
        static Process PS_REMOTE = Process.GetProcessesByName("RemotePlay").ToList().FirstOrDefault();
        uint WM_KEYDOWN = 0x100;
        uint WM_KEYUP = 0x101;
        public struct MSG
        {
            IntPtr hWnd;
            public uint message;
            public UIntPtr wParam;
            public UIntPtr lParam;
            int time;

        }
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        static int[] directions_pov = { -2, 23500, 18000, 13500, 27000, -1, 9000, 31500, 0, 4500 };
        static void waitFrames(int frames)
        {
            Task t = new Task(() =>
            {
                Thread.Sleep((int)(frames * 1000.0 / 60.0));
            });
            t.Start();
            t.Wait();
        }
        static void Main(string[] args)
        {
            if(PS_REMOTE == null)
            {
                return;
            }
            int dllVer = 0;
            int drvver = 0;
            IntPtr dllptr = new IntPtr(dllVer);
            IntPtr drvptr = new IntPtr(drvver);

            /*Program p = new Program();
            Thread t = new Thread(p.ProcessInputs);
            t.Start();
            t.Join();*/
            if (!isVJDExists(1))
            {
                Console.WriteLine("TURN VJOY ON");
                Console.ReadKey();
                return;
            }
            if (!AcquireVJD(1))
            {
                Console.WriteLine("SOMETHING ELSE IS USING THE VJOY");
                Console.ReadKey();
                return;
            }
            ResetAll();
            int frames = 1;
            Task t = new Task(() =>
            {
                Thread.Sleep((int)(frames * 1000.0 / 60.0));
            });
            //Start = 8, X=0,
            IInputSimulator isim = new InputSimulator();
            KeyboardSimulator ksim = new KeyboardSimulator(isim);
            Thread.Sleep(1000);
            SetForegroundWindow(PS_REMOTE.MainWindowHandle);
            Thread.Sleep(250);
            SetContPov(-1, 1, 1);
            /*ksim.KeyDown(VirtualKeyCode.RETURN);
            waitFrames(1);
            ksim.KeyUp(VirtualKeyCode.RETURN); //skip to title from OP/demo*/
            //Thread.Sleep(1000);
            //ksim.KeyPress(VirtualKeyCode.F8); Start and Split
            //TAS Starts Here
            ksim.KeyDown(VirtualKeyCode.RETURN);
            waitFrames(1);
            ksim.KeyUp(VirtualKeyCode.RETURN);
            waitFrames(61);
            for (var i = 0; i < 7; i++)
            {
                SetContPov(directions_pov[2], 1, 1);
                waitFrames(1);
                SetContPov(-1, 1, 1);
                waitFrames(3);
            }
            waitFrames(2);
            ksim.KeyDown(VirtualKeyCode.RETURN);
            waitFrames(1);
            ksim.KeyUp(VirtualKeyCode.RETURN);
            waitFrames(15);
            SetContPov(directions_pov[4], 1, 1);
            waitFrames(1);
            SetContPov(-1, 1, 1);
            waitFrames(15);
            SetBtn(true, 1, 1);
            waitFrames(1);
            SetBtn(false, 1, 1);
            waitFrames(215); 
            SetContPov(-1, 1, 1);
            SetBtn(true, 1, 1);
            waitFrames(1);
            SetBtn(false, 1, 1);
            waitFrames(20);
            SetBtn(true, 1, 1);
            waitFrames(1);
            SetBtn(false, 1, 1);
            waitFrames(25);
            SetBtn(true, 1, 1);
            waitFrames(1);
            SetBtn(false, 1, 1);
            waitFrames(243);
            SetBtn(true, 1, 1);
            waitFrames(1);
            SetBtn(false, 1, 1);
            waitFrames(32);
            SetContPov(directions_pov[6], 1, 1);
            waitFrames(1);
            SetBtn(true, 1, 7);
            waitFrames(2);
            SetBtn(false, 1, 7);
            SetContPov(-1, 1, 1);
            waitFrames(1);
            SetContPov(directions_pov[2], 1, 1);
            waitFrames(1);
            SetContPov(directions_pov[3], 1, 1);
            waitFrames(1);
            SetContPov(directions_pov[6], 1, 1);
            SetBtn(true, 1, 4);
            waitFrames(2);
            SetContPov(-1, 1, 1);
            SetBtn(false, 1, 4);
 


        }
    }
}
