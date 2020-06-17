using System;
using System.Linq;
using System.Threading;
using Gtk;
using System.Text;
using HidSharp;

namespace UbuconQuiz
{
    public class BuzzBuzzer
    {
        object _obj = new System.Object ();
        CancellationTokenSource cts;
        bool isWaiting;

        public BuzzBuzzer ()
        {
            isWaiting = false;
        }

        public void WaitForPress ()
        {
            if (isWaiting) {
                throw new InvalidOperationException ("Can't wait for two Events!");
            }
            isWaiting = true;
            cts = new CancellationTokenSource ();
            ThreadPool.QueueUserWorkItem (new WaitCallback (WaitReadFromBuzzer), cts.Token);
        }

        private void WaitReadFromBuzzer (System.Object obj)
        {
            var token = (CancellationToken)obj;
            byte[] bytes = new byte[0];
            HidStream stream;
            lock (_obj) {
                if (!Device.TryOpen (out stream)) {
                    Console.WriteLine ("Failed to open device.");
                    Environment.Exit (2);
                }

                using (stream) {
                    bool isPress = false;
                    while (!isPress) {

                        if (token.IsCancellationRequested) {
                            isPress = true;
                            bytes = new byte[Device.MaxInputReportLength];
                        } else {
                            bytes = new byte[Device.MaxInputReportLength];
                            int count;

                            try {
                                count = stream.Read (bytes, 0, bytes.Length);
                            } catch (TimeoutException) {
                                continue;
                            }
                            if (token.IsCancellationRequested) {
                                isPress = true;
                                bytes = new byte[Device.MaxInputReportLength];
                            } else if (count > 0) {
                                isPress = true;
                            }
                        }
                    }
                    Application.Invoke (delegate {
                        isWaiting = false;
                        if (PressBuzz != null && bytes.Length > 0)
                            PressBuzz (this, TransformBufferToEventArg (bytes));
                    });
                }
            }
        }

        public void CancelWait ()
        {
            cts.Cancel ();
        }


        public void LoadDevice ()
        {
            var loader = new HidDeviceLoader ();
            Thread.Sleep (2000); // Give a bit of time so our timing below is more valid as a benchmark.
            Device = loader.GetDevices (1356, 2).FirstOrDefault ();
            if (Device == null) {
                throw new ApplicationException ("Failed to open device.");
            }
        }

        public HidDevice Device {
            get;
            set;
        }

        public event EventHandler<PressBuzzBuzzerEventArgs> PressBuzz;

        private PressBuzzBuzzerEventArgs TransformBufferToEventArg (Byte[] buffer)
        {
            var button = BuzzButton.None;
            byte[] buttonBuffer = null;
            //Button 1: 00 7F 7F 01 00 F0
            buttonBuffer = new byte[]{ 0x00, 0x7F, 0x7F, 0x01, 0x00, 0xF0 };
            if (buffer.SequenceEqual (buttonBuffer)) {
                button = BuzzButton.Main1;
            }
            //Button 2: 00 7F 7F 20 00 F0
            buttonBuffer = new byte[]{ 0x00, 0x7F, 0x7F, 0x20, 0x00, 0xF0 };
            if (buffer.SequenceEqual (buttonBuffer)) {
                button = BuzzButton.Main2;
            }
            //Button 3: 00 7F 7F 00 04 F0
            buttonBuffer = new byte[]{ 0x00, 0x7F, 0x7F, 0x00, 0x04, 0xF0 };
            if (buffer.SequenceEqual (buttonBuffer)) {
                button = BuzzButton.Main3;
            }
            //Button 3: 00 7F 7F 00 80 F0
            buttonBuffer = new byte[]{ 0x00, 0x7F, 0x7F, 0x01, 0x80, 0xF0 };
            if (buffer.SequenceEqual (buttonBuffer)) {
                button = BuzzButton.Main4;
            }

            return new PressBuzzBuzzerEventArgs (buffer, button);
        }

    }

    public class PressBuzzBuzzerEventArgs : EventArgs
    {
        public PressBuzzBuzzerEventArgs (Byte[] returnBuffern, BuzzButton button)
        {
            ReturnBuffer = returnBuffern;
            PressButton = button;
        }

        public Byte[] ReturnBuffer {
            get;
        }

        public string ReturnStringBuffer {
            get {
                var sb = new StringBuilder ();
                foreach (var seq in ReturnBuffer) {
                    sb.Append (String.Format ("{0:X} ", seq));
                }
                return sb.ToString ().TrimEnd ();
            }
        }

        public BuzzButton PressButton {
            get;
        }

    }


    public enum BuzzButton
    {
        None,
        Main1,
        Main2,
        Main3,
        Main4
    }
}

