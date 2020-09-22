// Input.cs
// 사용자 입력을 받는 함수

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSBASIC {
    internal enum VarType { Int, Str, Sng, Dbl }
    internal struct VarInfo {
        internal VarType Types;
        internal int ArrayNum;
        internal List<int> ArraySize;
        internal object Value;
    }
    internal struct AutoStat {
        internal bool isOn;
        internal int jump;
        internal int now;
    }
    internal struct FilePref {
        internal string name;
        internal FileStream Free;
    }
    internal struct UDFunc {
        internal string Expr;
        internal Dictionary<string, VarType> Arg;
        internal VarType ReturnType;
    };

    class Input {
        static string str;
        static internal Dictionary<int, string> Lines = new Dictionary<int, string>(); // 줄들을 저장하는 거, 사용 시 정렬 필요.
        static internal Dictionary<string, VarInfo> Vars = new Dictionary<string, VarInfo>(); // 변수들을 저장하는 거.
        static internal Dictionary<string, UDFunc> FNs = new Dictionary<string, UDFunc>(); // DEF FN 함수들.
        static internal Dictionary<int, FilePref> Files = new Dictionary<int, FilePref>(); // 현재 사용 중인 파일들.
        static internal AutoStat ActiveAuto;

        static public void GetInput(string input) {
            str = "";
            // 문자열을 제외한 모든 문자 대문자화.
            if (input.Contains("\""))
                for (int i = 0; i <= input.Length - input.Replace("\"", "").Length; i++)
                    if (i % 2 == 0)
                        str += input.Split('\"')[i].ToUpper();
                    else
                        str += "\"" + input.Split('\"')[i] + "\"";
            else
                str = input.ToUpper();

            // AUTO산 줄번호 추가
            if (ActiveAuto.isOn)
                str = ActiveAuto.now.ToString() + " " + str;

            // 앞 공백 제거
            Utility.RemoveBlank(ref str);

            // 줄 저장 or 다이렉트 모드 진입
            if (str == "")
                return;
            if (char.IsDigit(str.ElementAt(0)))
                ToLine();
            else
                ToDirect();
        }

        static void ToDirect() => Process.BeginProcess(str, -1, IsInLoof.None);

        static internal int lastEdited = 0; // 마지막 편집 변수
        static void ToLine() {
            // 줄 번호 찾아서 넣기
            int dig = 0;
            foreach (char c in str)
                if (char.IsDigit(c))
                    dig++;
                else
                    break;

            // 줄은 0~65529
            if (int.Parse(str.Substring(0, dig)) < 0 || int.Parse(str.Substring(0, dig)) > 65529) {
                Console.WriteLine("Syntax Error");
                return;
            }
            int num = int.Parse(str.Substring(0, dig));
            // 마지막 편집 저장
            lastEdited = num; ActiveAuto.now = lastEdited + ActiveAuto.jump;
            if (ActiveAuto.now > 65529)
                ActiveAuto.now -= ActiveAuto.jump;

            // 공백 삭제
            str = str.Substring(dig);
            Utility.RemoveBlank(ref str);

            // 새로 줄 저장
            if (Lines.Keys.Contains(num))
                Lines[num] = str;
            else
                Lines.Add(num, str);
        }
    }
}
