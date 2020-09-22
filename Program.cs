// Program.cs
// 기본적인 프로그램의 진입점 제공
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSBASIC {
    class Program {
        static void Main(string[] args) {
            Console.SetWindowSize(80, 25);
            Console.SetBufferSize(80, 25);
            Console.WriteLine("CS-BASIC 0.95");
            Console.WriteLine("Copyright (c) 2017-2018 showmanlee");
            Console.WriteLine("Many of Bytes Free");
            Console.WriteLine("Ok");
            while (true) {
                Console.CancelKeyPress += Console_CancelKeyPress;
                // 저것도 지울 수 있게 하기
                if (Input.ActiveAuto.isOn)
                    Console.Write("{0} ", Input.ActiveAuto.now);
                string input = Console.ReadLine();
                if (input.Replace(" ", "").Length != 0)
                    Input.GetInput(input);
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e) {
            if (Input.ActiveAuto.isOn) {
                Input.ActiveAuto.isOn = false;
                Console.WriteLine("Ok");
            }
            e.Cancel = true;
        }
    }
}
