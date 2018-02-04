using System;
using System.Threading;

namespace IPLEmu
{
    static class Program
    {
        static TelnetConnection tc = new TelnetConnection("192.168.254.254", 23);
        static bool stopped;
        static void Main(string[] args)
        {
            Thread t1 = new Thread(Loop);
            t1.Start();
            while(!stopped)
            {
                if (Console.ReadKey().KeyChar == 'q')
                    stopped = true;
            }
            t1.Join();
        }

        static void Loop()
        {
            tc.Read();
            Read inputRead = new Read(PopUpInput,tc,"!");
            Read powerRead = new Read(PopUpPower, tc, "p");
            Read volumeRead = new Read(PopUpVolume, tc, "v");
            while(!stopped)
            {
                Console.SetCursorPosition(0, 0);
                inputRead.Update();
                Console.SetCursorPosition(0, 2);
                powerRead.Update();
                Console.SetCursorPosition(0, 3);
                volumeRead.Update();
                Thread.Sleep(500);
            }
        }

        static void PopUpVolume(string s)
        {
            int i;
            int.TryParse(s, out i);
            Console.WriteLine($"Volume is now {i}/100");
        }

        static void PopUpPower(string s)
        {
            s = s == "1" ? "on" : "off";
            Console.WriteLine($"Power is now {s}");
        }

        static void PopUpInput(string s)
        {
            Console.WriteLine($"Input was changed to {s}");
        }
    }

    class Read
    {
        public string LastRead { get; private set; }
        string _currentRead, query;
        Action<string> onChanged;
        TelnetConnection tc;
        public string CurrentRead
        {
            get
            {
                return _currentRead;
            }
            set
            {
                
                LastRead = _currentRead;
                _currentRead = value.TrimEnd();
            }
        }

        public void Update()
        {
            tc.Write(query);
            CurrentRead = tc.Read();
            if (HasChanged)
                onChanged(_currentRead);
        }

        public bool HasChanged
        {
            get
            {
                return _currentRead != LastRead;
            }
        }


        public Read(Action<string> onChanged, TelnetConnection tc, string query)
        {
            LastRead = "";
            _currentRead = "";
            this.onChanged = onChanged;
            this.tc = tc;
            this.query = query;
        }

        public static implicit operator string(Read r)
        {
            return r._currentRead;
        }

        public override string ToString()
        {
            return _currentRead;
        }
    }
}
