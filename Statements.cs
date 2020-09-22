//Statements.cs
//값을 반환하는 함수의 집합
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace CSBASIC {
    class Statements {
        internal static bool printnotlined = false;
        static int subroutineLine = -10;
        static int breakLine = -10;
        static bool trace = false;
        static int soundfreq = 0;
        static int sounddur = 0;
        public static void AUTO(string str) {
            // AUTO [줄번호|.][, [증가폭]]
            // . == 마지막으로 편집한 줄 사용
            // 증가폭 없으면 짜잔

            str = str.Remove(0, 4);
            Utility.RemoveBlank(ref str);
            int target = 0, inc = 0; string temp;
            if (str.Replace(" ", "") == "") {
                target = 10; inc = 10;
            }
            else {
                try {
                    if (str.Contains(',')) {
                        if (int.Parse(str.Split(',')[0]) < 0 || int.Parse(str.Split(',')[0]) > 65529) {
                            Console.WriteLine("Syntax Error"); return;
                        }
                        else if (str.Split(',')[0].Contains(".")) {
                            bool isAppeared = false;
                            foreach (char c in str.Split(',')[0]) {
                                if (c != ' ' && c != '.') {
                                    Console.WriteLine("Syntax Error"); return;
                                }
                                else if (c == '.')
                                    if (isAppeared) {
                                        Console.WriteLine("Syntax Error"); return;
                                    }
                                    else
                                        isAppeared = true;
                            }
                            target = Input.lastEdited;
                        }
                        else {
                            target = int.Parse(str.Split(',')[0]);
                        }
                        inc = int.Parse(str.Split(',')[1]);
                    }
                    else {
                        if (int.Parse(str) < 0 || int.Parse(str) > 65529) {
                            Console.WriteLine("Syntax Error"); return;
                        }
                        else if (str.Contains(".")) {
                            bool isAppeared = false;
                            foreach (char c in str) {
                                if (c != ' ' && c != '.') {
                                    Console.WriteLine("Syntax Error"); return;
                                }
                                else if (c == '.')
                                    if (isAppeared) {
                                        Console.WriteLine("Syntax Error"); return;
                                    }
                                    else
                                        isAppeared = true;
                            }
                            target = Input.lastEdited;
                        }
                        else {
                            target = int.Parse(str);
                        }
                        inc = 10;
                    }
                }
                catch { Console.WriteLine("Type mismatch"); return; }
            }

            Input.ActiveAuto.isOn = true; Input.ActiveAuto.now = target; Input.ActiveAuto.jump = inc;
        }
        public static void BEEP(string str) {
            // BEEP
            str = str.Remove(0, 4);
            if (str.Replace(" ", "") != "") {
                Console.WriteLine("Syntax Error"); return;
            }
            Console.Beep(800, 250);
        }
        public static void BLOAD(string str) {
            // BLOAD
            // 메모리 이미지를 불러온다. 지원하지 않을 예정.
            Console.WriteLine("Not support on this version");
            return;
        }
        public static void BSAVE(string str) {
            // BSAVE
            // 메모리에 이미지 파일을 넣는다. 지원하지 않을 예정.
            Console.WriteLine("Not support on this version");
            return;
        }
        public static void CALL(string str) {
            // CALL 기계어주소[(변수들)]
            // 기계어를 다루며, 지금은 지원하지 않는다.
            Console.WriteLine("Not Support this Version");
        }
        public static void CALLS(string str) {
            // CALLS 포트란주소[(변수들)]
            // 포트란을 다루며, 지금은 지원하지 않는다.
            Console.WriteLine("Not Support on this Version");
        }
        public static void CIRCLE(string str) {
            // CIRCLE (x, y), 지름[, [색상][, 시작][, 종료][, 반경]]
            // 그래픽 관련 명령어로, 지금은 지원하지 않는다.
            Console.WriteLine("Not Support on this Version");
        }
        public static void CLEAR(string str) // 미완성
        {
            // CLEAR [, 메모리제한[, 스택제한]]
            // 메모리나 스택은 그냥 껍질만 구현해두자.
            str = str.Remove(0, 5);
            Utility.RemoveBlank(ref str);
            if (str.Replace(" ", "") != "") {
                if (str[0] == ',') {
                    try { int.Parse(str.Split(',')[0]); }
                    catch { Console.WriteLine("Syntax Error"); return; }
                    Console.WriteLine("Syntax Error"); return;
                }
            }
            Input.Vars.Clear();
            Input.FNs.Clear();
            Console.Beep(100, 1);

        }
        public static void CLS(string str) {
            // CLS
            // 콘솔 창을 지운다.
            str = str.Remove(0, 3);
            if (str.Replace(" ", "") != "") {
                Console.WriteLine("Illegal function call"); return;
            }
            Console.Clear();
        }
        public static void COLOR(string str) {
            // COLOR 글자,배경,테두리
            str = str.Remove(0, 5);
            int fore = Convert.ToInt32(Utility.GetValue(str.Split(',')[0], "", VarType.Int));
            if (fore > 31 || fore < 0)
                throw new Exception();
            Console.ForegroundColor = (ConsoleColor)(fore % 16);
            if (str.Contains(',')) {
                str = str.Split(',')[1];
                int back = Convert.ToInt32(Utility.GetValue(str.Split(',')[0], "", VarType.Int));
                if (back > 15 || back < 0)
                    throw new Exception();
                Console.BackgroundColor = (ConsoleColor)(back % 8);
                if (str.Contains(',')) {
                    str = str.Split(',')[1];
                    int border = Convert.ToInt32(Utility.GetValue(str.Split(',')[0], "", VarType.Int));
                    if (border > 15 || border < 0)
                        throw new Exception();
                }
            }
        }
        public static void CONT(string str) {
            // CONT
            // 작동 다시 시작하기(다이렉트에서만 실행 가능)
            if (breakLine == -10 || Process.nowln != -1) {
                Console.WriteLine("Can't continue");
                throw new Exception();
            }
            List<int> order = Input.Lines.Keys.OrderBy(num => num).ToList();
            for (int i = 0; i < order.Count; i++)
                if (order[i] == breakLine) {
                    Process.nowln = order[++i];
                    break;
                }
            breakLine = -10;
            RUN("RUN " + Process.nowln);
        }
        public static void DEF(string str) {
            if (Process.nowln == -1)
                throw new Exception();
            // DEF 시리즈
            str = str.Remove(0, 3);
            if (str[0] != ' ') {
                // DEFINT, SNG, DBL, STR
                VarType type;
                switch (str.Substring(0, 3)) {
                    case "INT":
                        type = VarType.Int;
                        break;
                    case "SNG":
                        type = VarType.Sng;
                        break;
                    case "DBL":
                        type = VarType.Dbl;
                        break;
                    case "STR":
                        type = VarType.Str;
                        break;
                    default:
                        throw new Exception();
                }
                str = str.Remove(0, 4);
                for (int i = 0; i < Utility.CharAmount(str, ','); i++) {
                    string stra = Utility.RemoveBlank(str.Split(',')[i]);
                    // A~Z까지 전체할당 - 공간 지원 X
                    if (stra.Contains('-'))
                        if (stra.Split('-')[0].Length == 1 && stra.Split('-')[1].Length == 1 && char.IsUpper(stra.Split('-')[0][0]) && char.IsUpper(stra.Split('-')[1][0])) {
                            for (char a = stra.Split('-')[0][0]; a <= stra.Split('-')[1][0]; a++) {
                                if (Input.Vars.ContainsKey(a.ToString()))
                                    Input.Vars.Remove(a.ToString());
                                VarInfo t; t.ArrayNum = 0; t.Types = type; t.Value = null; t.ArraySize = new List<int>();
                                switch (type) {
                                    case VarType.Int:
                                        t.Value = 0;
                                        break;
                                    case VarType.Sng:
                                        t.Value = 0.0f;
                                        break;
                                    case VarType.Dbl:
                                        t.Value = 0.0;
                                        break;
                                    case VarType.Str:
                                        t.Value = "";
                                        break;
                                }
                                Input.Vars.Add(a.ToString(), t);
                            }
                        }
                        else
                            throw new Exception();
                    // 그런 거 없는 단독 할당
                    else {
                        if (!char.IsUpper(stra[0])) {
                            foreach (char c in stra)
                                if (!char.IsDigit(c) && !char.IsUpper(c) && c == '.')
                                    throw new Exception();
                            VarInfo t; t.ArrayNum = 0; t.Types = type; t.Value = null; t.ArraySize = new List<int>();
                            switch (type) {
                                case VarType.Int:
                                    t.Value = 0;
                                    break;
                                case VarType.Sng:
                                    t.Value = 0.0f;
                                    break;
                                case VarType.Dbl:
                                    t.Value = 0.0;
                                    break;
                                case VarType.Str:
                                    t.Value = "";
                                    break;
                            }
                            Input.Vars.Add(stra, t);
                        }
                        else
                            throw new Exception();
                    }
                }
            }
            else {
                Utility.RemoveBlank(ref str);
                if (str.Substring(0, 3) == "USR") {
                    // DEF USR - 어셈블리어 함수를 지정한다. 지원하지 않을 예정.
                    Console.WriteLine("Not support on this version");
                    return;
                }
                else if (str.Substring(0, 3) == "SEG") {
                    // DEF SEG - BLOAD, BSAVE, CALL, PEEK, POKE, and USR에서 읽을 수 있는 메모리 세그먼트를 할당한다는데, 어차피 저거 태반 여기서 지원 안 할 예정.
                    Console.WriteLine("Not support on this version");
                    return;
                }
                else if (str.Substring(0, 2) == "FN") {
                    // DEF FN[ ]이름 (인수들) = 식
                    // 사용자 지정 함수. FN 함수로 불러들인다.
                    str = Utility.RemoveBlank(str.Remove(0, 2));
                    // 이름
                    UDFunc u;
                    string name = str.Split('(', ')')[0];
                    if (!char.IsUpper(name[0]))
                        throw new Exception();
                    foreach (char c in name.Substring(1, name.Length - 1))
                        if (!char.IsDigit(c) && !char.IsUpper(c) && c == '.')
                            throw new Exception();
                    // 출력형 지정
                    switch (name.Last()) {
                        case '$':
                            u.ReturnType = VarType.Str;
                            break;
                        case '%':
                            u.ReturnType = VarType.Int;
                            break;
                        case '#':
                            u.ReturnType = VarType.Dbl;
                            break;
                        case '!':
                            u.ReturnType = VarType.Sng;
                            break;
                        default:
                            if (!char.IsDigit(name.Last()) && !char.IsUpper(name.Last()) && name.Last() == '.')
                                throw new Exception();
                            u.ReturnType = VarType.Sng;
                            break;
                    }
                    // 인수 설정
                    string arg = str.Split('(', ')')[1];
                    u.Arg = new Dictionary<string, VarType>();
                    for (int i = 0; i <= Utility.CharAmount(arg, ','); i++) {
                        string s = Utility.RemoveBlank(arg.Split(',')[i]);
                        if (!char.IsUpper(s[0]))
                            throw new Exception();
                        foreach (char c in s.Substring(1, s.Length - 1))
                            if (!char.IsDigit(c) && !char.IsUpper(c) && c == '.')
                                throw new Exception();
                        VarType type;
                        switch (s.Last()) {
                            case '$':
                                type = VarType.Str;
                                break;
                            case '%':
                                type = VarType.Int;
                                break;
                            case '#':
                                type = VarType.Dbl;
                                break;
                            case '!':
                                type = VarType.Sng;
                                break;
                            default:
                                if (!char.IsDigit(s.Last()) && !char.IsUpper(s.Last()) && s.Last() == '.')
                                    throw new Exception();
                                type = VarType.Sng;
                                break;
                        }
                        u.Arg.Add(s, type);
                    }
                    u.Expr = str.Split('=')[1];
                    if (Input.FNs.ContainsKey(name))
                        Input.FNs[name] = u;
                    else
                        Input.FNs.Add(name, u);
                    // 식 설정

                }
                else
                    throw new Exception();
            }
        }
        public static void DELETE(string str) {
            str = Utility.RemoveBlank(str.Remove(0, 6));
            int start = 0, end = 65530;
            if (str.Replace(" ", "") == "") {
                Input.Lines.Clear();
                return;
            }

            if (str[0] == '.') {
                start = Input.lastEdited;
                str = Utility.RemoveBlank(str.Remove(0, 1));
            }
            else if (Char.IsDigit(str[0])) {
                start = Convert.ToInt32(str.Split('-')[0]);
                if (!Input.Lines.Keys.Contains(start))
                    throw new Exception();
                str = Utility.RemoveBlank(str.Remove(0, str.Split('-')[0].Length));
            }
            if (str.Replace(" ", "") != "") {
                if (str[0] == '-') {
                    str = Utility.RemoveBlank(str.Remove(0, 1));
                    if (str == ".")
                        end = Input.lastEdited;
                    else if (Char.IsDigit(str[0])) {
                        end = Convert.ToInt32(str);
                        if (!Input.Lines.Keys.Contains(end))
                            throw new Exception();
                    }
                    else
                        throw new Exception();
                }
                else
                    throw new Exception();
            }
            List<int> order = Input.Lines.Keys.OrderBy(num => num).ToList();
            foreach (int i in order)
                if (i >= start && i <= end)
                    Input.Lines.Remove(i);
        }
        public static void DIM(string str) {
            str = Utility.RemoveBlank(str.Remove(0, 3));
            string name = Utility.RemoveBlank(str.Split(new[] { '(', '[' })[0]);
            string sizes = Utility.RemoveBlank(str.Split(new[] { '(', '[' })[1]);
            if (sizes.Last() == ')' || sizes.Last() == ']')
                sizes = sizes.Substring(0, sizes.Length - 1);
            else
                throw new Exception();
            if (!Utility.IsValidVarRule(name))
                throw new Exception();

            if (Input.Vars.ContainsKey(name)) {
                Console.WriteLine("Duplicate definition");
                return;
            }
            VarInfo done;
            done.ArrayNum = Utility.CharAmount(sizes, ',') + 1; done.ArraySize = new List<int>(); done.Types = VarType.Sng;
            if (done.ArrayNum > 5)
                throw new Exception();
            for (int i = 0; i <= Utility.CharAmount(sizes, ','); i++) {
                done.ArraySize.Add(Convert.ToInt32(sizes.Split(',')[i]) + 1);
                if (done.ArraySize[i] > 256 || done.ArraySize[i] < 0)
                    throw new Exception();
            }
            switch (name.Last()) {
                case '$':
                    done.Types = VarType.Str;
                    break;
                case '%':
                    done.Types = VarType.Int;
                    break;
                case '#':
                    done.Types = VarType.Dbl;
                    break;
                case '!':
                default:
                    done.Types = VarType.Sng;
                    break;
            }
            done.Value = Utility.ArrayInitialize(done.ArrayNum, done.Types, done.ArraySize.ToArray());
            Input.Vars.Add(name, done);
        }
        public static void EDIT(string str) {
            // EDIT
            // 지금 수정은 불가함. 대신 한 줄만 출력하게 하는 걸로.
            str = Utility.RemoveBlank(str.Remove(0, 4));
            if (str == ".")
                Console.WriteLine(Input.lastEdited + " " + Input.Lines[Input.lastEdited]);
            else
                Console.WriteLine(int.Parse(str) + " " + Input.Lines[int.Parse(str)]);
        }
        public static void END(string str) {
            // END
            // 프로그램 중단 - 파일 닫음
            str = str.Remove(0, 3);
            Utility.RemoveBlank(ref str);
            if (str == "") {
                breakLine = Process.nowln;
                Process.nowln = -10;
            }
        }
        public static void ERASE(string str) {
            // ERASE
            // 배열을 삭제한다.
            str = str.Remove(0, 5);
            for (int i = 0; i <= Utility.CharAmount(str, ','); i++) {
                string stra = Utility.RemoveBlank(str.Split(',')[i]);
                if (Input.Vars.ContainsKey(stra)) {
                    if (Input.Vars[stra].ArrayNum != 0)
                        Input.Vars.Remove(stra);
                    else
                        throw new Exception();
                }
                else
                    throw new Exception();
            }
        }
        public static void FOR(string str) {
            // FOR 선언식 TO 목표 STEP 추가폭
            // :== for (선언식; i < 목표; 추가폭)
            // 끝은 NEXT 변수
            str = str.Remove(0, 3);
            string stra = Utility.RemoveBlank(str.Split(new[] { "TO" }, StringSplitOptions.None)[0]);
            // 선언식
            string varname = stra.Split('=')[0];
            if (!Process.FindVar(true, stra))
                throw new Exception();
            stra = str.Split(new[] { "TO" }, StringSplitOptions.None)[1];
            // 목표
            double target = Convert.ToDouble(Utility.GetValue(stra.Split(new[] { "STEP" }, StringSplitOptions.None)[0], "", VarType.Dbl));
            // 추가폭
            double step = 1;
            if (stra.Contains("STEP"))
                step = Convert.ToDouble(Utility.GetValue(stra.Split(new[] { "STEP" }, StringSplitOptions.None)[1], "", VarType.Dbl));

            // NEXT 찾기
            List<int> order = Input.Lines.Keys.OrderBy(num => num).ToList();
            int end = -1;
            foreach (int s in order) {
                if (s < Process.nowln)
                    continue;
                if (Input.Lines[s].Contains("NEXT"))
                    if (Utility.RemoveBlank(Input.Lines[s].Split(new[] { "NEXT" }, StringSplitOptions.None)[1]) == varname) {
                        end = order.IndexOf(s);
                        break;
                    }
            }
            if (end == -1) {
                Console.WriteLine("FOR without NEXT" + (Process.nowln == -1 ? "" : ("in " + Process.nowln)));
                return;
            }
            // 다이렉트 체크
            int start = Process.nowln;
            if (start == -1)
                throw new Exception();
            int cursor = order.IndexOf(start);
            // for 반복 시작
            for (; Convert.ToDouble(Input.Vars[varname].Value) <= target; Process.FindVar(true, varname + "=" + Input.Vars[varname].Value.ToString() + "+" + step.ToString()))
                for (cursor = order.IndexOf(start); cursor < end; cursor++) {
                    if (trace)
                        Console.Write("[" + order[cursor] + "]");
                    if (cursor == order.IndexOf(start))
                        Process.BeginProcess(Input.Lines[order[cursor]], order[cursor], IsInLoof.For);
                    else
                        Process.BeginProcess(Input.Lines[order[cursor]], order[cursor], IsInLoof.None);
                    if (Input.Lines[order[cursor]].Substring(0, 3) == "FOR" && cursor != order.IndexOf(start))
                        for (int i = end - 1; i > cursor; i--) {
                            try {
                                if (Utility.RemoveBlank(Input.Lines[order[i]].Split(new[] { "NEXT" }, StringSplitOptions.None)[1]) == Utility.RemoveBlank(Input.Lines[order[cursor]].Remove(0, 3).Split('=')[0])) {
                                    cursor = i;
                                    break;
                                }
                            }
                            catch { continue; }
                        }
                }
            Process.nowln = order[end];
        }
        public static void GOSUB(string str) {
            // GOSUB 번호
            // 해당 줄에 있는 서브루틴으로 이동한다.
            str = str.Remove(0, 5);
            Utility.RemoveBlank(ref str);
            int target = Convert.ToInt32(Convert.ToInt32(str.Split(' ')[0]));
            List<int> order = Input.Lines.Keys.OrderBy(num => num).ToList();
            if (Input.Lines.ContainsKey(target)) {
                for (int i = 0; i < order.Count; i++)
                    if (order[i] == Process.nowln) {
                        subroutineLine = order[++i];
                        break;
                    }
                Process.nowln = target;
            }
            else
                throw new Exception();
        }
        public static void GOTO(string str) {
            str = str.Remove(0, 4);
            Utility.RemoveBlank(ref str);
            int target = Convert.ToInt32(Convert.ToInt32(str.Split(' ')[0]));
            if (Input.Lines.ContainsKey(target))
                Process.nowln = target;
            else
                throw new Exception();
        }
        public static void IF(string str) {
            // IF 조건식 THEN(GOTO) 참 ELSE 거짓
            str = str.Remove(0, 2);
            Utility.RemoveBlank(ref str);
            string afterelse = "", afterthen = "";
            if (str.Contains("ELSE")) {
                afterelse = Utility.RemoveBlank(str.Split(new[] { "ELSE" }, StringSplitOptions.None)[1]);
                str = str.Split(new[] { "ELSE" }, StringSplitOptions.None)[0];
            }
            if (str.Contains("THEN")) {
                afterthen = Utility.RemoveBlank(str.Split(new[] { "THEN" }, StringSplitOptions.None)[1]);
                str = str.Split(new[] { "THEN" }, StringSplitOptions.None)[0];
            }
            else if (str.Contains("ELSE")) {
                afterthen = str.Split(new[] { "GOTO" }, StringSplitOptions.None)[1];
                str = str.Split(new[] { "GOTO" }, StringSplitOptions.None)[0];
            }
            else
                throw new Exception();
            // 그럼 조건식만 남겠지
            if (Convert.ToInt32(Utility.GetValue(str, "", VarType.Int)) == 0) {
                if (afterelse.Length != 0) {
                    // ELSE
                    if (char.IsDigit(afterelse[0]))
                        Process.nowln = Convert.ToInt32(Utility.GetValue(afterelse, "", VarType.Int));
                    if (trace)
                        Console.Write("[" + Process.nowln + "]");
                    Process.BeginProcess(afterelse, Process.nowln, IsInLoof.None);
                }
            }
            else {
                // THEN
                if (char.IsDigit(afterthen[0]))
                    Process.nowln = Convert.ToInt32(Utility.GetValue(afterthen, "", VarType.Int));
                if (trace)
                    Console.Write("[" + Process.nowln + "]");
                Process.BeginProcess(afterthen, Process.nowln, IsInLoof.None);
            }
        }
        public static void INPUT(string str) {
            str = Utility.RemoveBlank(str.Remove(0, 5));
            bool notnewline = false;
            if (str[0] == ';') {
                notnewline = true;
                str = Utility.RemoveBlank(str.Remove(0, 1));
            }

            // 문자열
            string Keep = str;
            REDOFROMSTART:
            if (str[0] == '\"') {
                if (!notnewline)
                    Console.WriteLine();
                Console.Write(str.Split('\"')[1]);
                str = Utility.RemoveBlank(str.Split('\"')[2]);
                if (str[0] == ';')
                    Console.Write("?");
                else if (str[0] != ',')
                    throw new Exception();
                str = Utility.RemoveBlank(str.Remove(0, 1));
            }
            else
                Console.Write("?");

            // 할당할 변수 이름들 지정 
            List<string> vars = new List<string>();
            for (int i = 0; i <= Utility.CharAmount(str, ','); i++) {
                string name = Utility.RemoveBlank(str.Split(',')[i]);
                if (!Utility.IsValidVarRule(name.Split(new[] { '(', '[' })[0]))
                    throw new Exception();
                vars.Add(name);
            }

            string input = Console.ReadLine(); int quote = 0;
            List<string> inputs = new List<string>();
            for (int i = 0; i <= Utility.CharAmount(input, ','); i++) {
                if (quote % 2 == 1)
                    if (!input.Split(',')[i].Contains('\"'))
                        continue;
                    else {
                        inputs.Add(input.Split(',')[quote++]);
                        continue;
                    }
                else if (input.Split(',')[i].Contains('\"')) {
                    quote++; continue;
                }
                inputs.Add(Utility.RemoveBlank(input.Split(',')[i]));
            }
            if (inputs.Count != vars.Count)
                throw new Exception();
            for (int i = 0; i < vars.Count; i++) {
                VarInfo done; List<int> dig = new List<int>();
                // 변수 만들기
                if (!Input.Vars.ContainsKey(vars[i].Split(new[] { '(', '[' })[0])) {
                    // 배열 변수 만들기
                    if (vars[i].Last() == ')' || vars[i].Last() == ']') {
                        string sendtoDIM = "DIM " + vars[i].Split(new[] { '(', '[' })[0] + "(";
                        for (int j = 0; j <= Utility.CharAmount(vars[i].Split(new[] { '(', '[' })[1], ','); j++) {
                            dig.Add(Convert.ToInt32(vars[i].Split(new[] { '(', '[' })[1].Substring(0, vars[i].Split(new[] { '(', '[' })[1].Length - 1).Split(',')[j]));
                            if (dig[j] > 10 || dig[j] < 0)
                                throw new Exception();
                            sendtoDIM += j == 0 ? "10" : ",10";
                        }
                        DIM(sendtoDIM + ")");
                    }
                    // 일반 변수 만들기
                    else {
                        if (!Utility.IsValidVarRule(vars[i]))
                            throw new Exception();
                        done.ArrayNum = 0; done.ArraySize = new List<int>(); done.Types = VarType.Sng; done.Value = null;
                        switch (vars[i].Last()) {
                            case '$':
                                done.Types = VarType.Str;
                                break;
                            case '%':
                                done.Types = VarType.Int;
                                break;
                            case '#':
                                done.Types = VarType.Dbl;
                                break;
                            case '!':
                            default:
                                done.Types = VarType.Sng;
                                break;
                        }
                        Input.Vars.Add(vars[i], done);
                    }
                }
                // 할당
                done = Input.Vars[vars[i].Split(new[] { '(', '[' })[0]];
                object value = null;
                try {
                    switch (done.Types) {
                        case VarType.Str:
                            value = inputs[i];
                            break;
                        case VarType.Int:
                            value = Convert.ToInt32(inputs[i]);
                            break;
                        case VarType.Sng:
                            value = Convert.ToSingle(inputs[i]);
                            break;
                        case VarType.Dbl:
                            value = Convert.ToDouble(inputs[i]);
                            break;
                    }
                }
                catch {
                    Console.WriteLine("?Redo from start");
                    str = Keep;
                    goto REDOFROMSTART;
                }
                switch (done.ArrayNum) {
                    case 0:
                        done.Value = value;
                        break;
                    case 1: {
                            object[] array = (object[])done.Value;
                            array[dig[0]] = value;
                            done.Value = array;
                            break;
                        }
                    case 2: {
                            object[,] array = (object[,])done.Value;
                            array[dig[0], dig[1]] = value;
                            done.Value = array;
                            break;
                        }
                    case 3: {
                            object[,,] array = (object[,,])done.Value;
                            array[dig[0], dig[1], dig[2]] = value;
                            done.Value = array;
                            break;
                        }
                    case 4: {
                            object[,,,] array = (object[,,,])done.Value;
                            array[dig[0], dig[1], dig[2], dig[3]] = value;
                            done.Value = array;
                            break;
                        }
                    case 5: {
                            object[,,,,] array = (object[,,,,])done.Value;
                            array[dig[0], dig[1], dig[2], dig[3], dig[4]] = value;
                            done.Value = array;
                            break;
                        }
                }
                Input.Vars[vars[i].Split(new[] { '(', '[' })[0]] = done;
            }
        }
        public static void KILL(string str) {
            // KILL
            // 해당 파일을 삭제한다.
            str = str.Remove(0, 4);
            string pos = Utility.GetValue(str, "", VarType.Str).ToString();
            File.Delete(pos);
        }
        public static void LCOPY(string str) {
            // LCOPY
            // 공식적으로 아무 것도 안 함
        }
        public static void LET(string str) {
            // LET
            // 그저 할당
            str = Utility.RemoveBlank(str.Substring(0, 3));
            Process.FindVar(true, str);
        }
        public static void LINE(string str) {
            str = Utility.RemoveBlank(str.Remove(0, 4));
            if (str.Substring(0, 5) == "INPUT") {
                // LINE INPUT
                // 문자열 한 줄을 입력받는다.
            }
            else
                // LINE
                // 선을 그린다. 정말로?
                Console.WriteLine("Not Support on this Version");
        }
        public static void LIST(string str) // 미완성 
        {
            // LIST [줄시작][-][줄끝][, 파일번호]
            // 시작/끝 대신 .만 쓰이면 마지막으로 썼던 곳 전 후
            int start = -1, end = 65556;
            str = str.Remove(0, 4);
            Utility.RemoveBlank(ref str);
            string stra = Utility.RemoveBlank(str.Split(',')[0]);
            if (stra.Contains('-') || stra == "") {
                if (stra.Split('-')[0] != "") {
                    if (Utility.RemoveBlank(stra.Split('-')[0]) == ".")
                        start = Input.lastEdited;
                    else
                        start = Convert.ToInt32(Utility.GetValue(stra.Split('-')[0], "", VarType.Int));
                }
                if (stra.LastIndexOf('-') != stra.Length - 1 && Utility.RemoveBlank(stra.Split('-')[1]) != "") {
                    if (Utility.RemoveBlank(stra.Split('-')[1]) == ".")
                        end = Input.lastEdited;
                    else
                        end = Convert.ToInt32(Utility.GetValue(stra.Split('-')[1], "", VarType.Int));
                }
                foreach (var s in Input.Lines.OrderBy(num => num.Key))
                    if (s.Key >= start & s.Key <= end)
                        Console.WriteLine("{0} {1}", s.Key, s.Value);
            }
            else {
                if (Input.Lines.ContainsKey(Convert.ToInt32(stra)))
                    Console.WriteLine("{0} {1}", Convert.ToInt32(stra), Input.Lines[Convert.ToInt32(stra)]);
                else
                    throw new Exception();
            }
            // 파일 구현되면 파일 번호에 대응해서 만들기
        }
        public static void LOAD(string str) {
            // LOAD 경로 ,r
            // CLEAR와 함께, 프로그램을 불러온다.
            CLEAR("CLEAR");
            Input.Lines.Clear();
            str = Utility.RemoveBlank(str.Remove(0, 4));
            int comma = Utility.CharAmount(str, ','); string pos, mod = "";
            if (comma == 0)
                pos = Utility.GetValue(str, "", VarType.Str).ToString();
            else {
                pos = Utility.GetValue(str.Split(',')[comma - 1], "", VarType.Str).ToString();
                mod = Utility.RemoveBlank(str.Split(',')[comma]);
            }
            if (mod != "" && mod != "R")
                throw new Exception();
            StreamReader sw = new StreamReader(pos);
            do {
                string a = sw.ReadLine();
                Input.Lines.Add(Convert.ToInt32(a.Split(' ')[0]), a.Substring(a.Split(' ')[0].Length + 1));
            } while (!sw.EndOfStream);
            sw.Close();
        }
        public static void LPRINT(string str) {
            // LPRINT
            // 프린터에서 출력한다. 현재는 미지원.
            Console.WriteLine("Not Support on this Version");
        }
        public static void MERGE(string str) {
            // MERGE
            // 해당 프로그램의 줄들을 여기다가 덮어쓴다.
            str = Utility.RemoveBlank(str.Remove(0, 4));
            string pos = Utility.GetValue(str, "", VarType.Str).ToString();
            StreamReader sw = new StreamReader(pos);
            do {
                string a = sw.ReadLine();
                int num = Convert.ToInt32(a.Split(' ')[0]);
                string exp = a.Substring(a.Split(' ')[0].Length + 1);
                if (Input.Lines.ContainsKey(num))
                    Input.Lines[num] = exp;
                else
                    Input.Lines.Add(num, exp);
            } while (!sw.EndOfStream);
            sw.Close();
        }
        public static void MKDIR(string str) {
            // MKDIR
            // 폴더를 하나 만든다.
            str = str.Remove(0, 5);
            string dir = Utility.GetValue(str, "", VarType.Str).ToString();
            DirectoryInfo di = new DirectoryInfo(dir);
            if (!Directory.Exists(dir))
                di.Create();
        }
        public static void MOTOR(string str) {
            // MOTOR
            // 카세트 모터를 돌린다고?
            Console.WriteLine("Not Support on this Version");
        }
        public static void NEW(string str) {
            // NEW
            // 완전 초기화
            str = str.Remove(0, 3);
            if (str.Replace(" ", "") != "") {
                Console.WriteLine("Illegal function call"); return;
            }
            Input.Lines.Clear();
            CLEAR("CLEAR");
            trace = false;
        }
        public static void NEXT(string str) {
            // NEXT 변수
            // 해당 변수에 대응하는 FOR로 돌아간다. 그리고 그건 얘가 해줄 예정.
            str = str.Remove(0, 4);
            List<int> order = Input.Lines.Keys.OrderBy(num => num).ToList();
            foreach (int i in order)
                if (i < Process.nowln && Input.Lines[i].Contains("FOR") && Utility.RemoveBlank(Input.Lines[i].Split(new[] { "FOR", "=" }, StringSplitOptions.None)[1]) == Utility.RemoveBlank(str))
                    return;
            Console.WriteLine("NEXT without FOR");
            throw new Exception();
        }
        public static void ON(string str) {
            // ON 변수 GOTO|GOSUB 1, 2, 3...
            // 변수의 값에 따라 1,2,3.. 번째 케이스로 이동한다.
            str = Utility.RemoveBlank(str.Remove(0, 2));
            string var = str.Split(' ')[0];
            str = Utility.RemoveBlank(str.Remove(0, var.Length));
            bool isGosub = false;
            if (str.Split(' ')[0] == "GOTO") {
                isGosub = false;
                str = Utility.RemoveBlank(str.Remove(0, 4));
            }
            else if (str.Split(' ')[0] == "GOSUB") {
                isGosub = true;
                str = Utility.RemoveBlank(str.Remove(0, 5));
            }
            else
                throw new Exception();

            List<int> cases = new List<int>();
            for (int i = 0; i <= Utility.CharAmount(str, ','); i++) {
                if (!Input.Lines.ContainsKey(Convert.ToInt32(str.Split(',')[i])))
                    throw new Exception();
                cases.Add(Convert.ToInt32(str.Split(',')[i]));
            }

            int expr = Convert.ToInt32(Utility.GetValue(var, "", VarType.Int));
            if (expr > 0 && expr <= cases.Count) {
                if (isGosub)
                    GOSUB("GOSUB " + cases[expr - 1]);
                else
                    GOTO("GOTO " + cases[expr - 1]);
            }
        }
        public static void OPEN(string str) {
            str = Utility.RemoveBlank(str.Remove(0, 4));
            FileStream fs;

        }
        public static void PAINT(string str) {
            // PAINT
            // 색깔을 채운다. 지원하지 않는다.
            Console.WriteLine("Not Support on this Version");
        }
        public static void PALLETE(string str) {
            // PALLETE
            // 색상 팔레트를 변경한다. 당연히 지원 안 하지
            Console.WriteLine("Not Support on this Version");
        }
        public static void PEEK(string str) {
            // PEEK
            // DEF SEG로 지정된 메모리에 있는 값을 본다. 지원하지 않을 에정이다.
            Console.WriteLine("Not support on this version");
            return;
        }
        public static void POKE(string str) {
            // POKE
            // DEF SEG로 할당된 메모리에 데이터를 넣는다. 지원하지 않을 예정.
            Console.WriteLine("Not support on this version");
            return;
        }
        public static void PRINT(string str) {
            // PRINT
            // 출력한다! 출력!
            // USING은 포맷화하기.
            if (str[0] == '?')
                str = str.Remove(0, 1);
            else
                str = str.Remove(0, 5);
            Utility.RemoveBlank(ref str);
            if (str == "") {
                if (printnotlined)
                    printnotlined = !printnotlined;
                Console.WriteLine();
                return;
            }

            int file = -1;
            if (str[0] == '#') {
                // 파일 구현되면 파일에 적기 구현할 것.
            }

            Utility.RemoveBlank(ref str);
            string format = ""; int formatonstring = 0; string formatonnum = "";
            if (str.Length > 5 && str.Substring(0, 5) == "USING") {
                // 포맷화
                str = str.Remove(0, 5);
                format = Utility.RemoveBlank(str.Split('\"')[1]);
                if (format.Contains('!') || format.Contains('\\') || format.Contains('&')) {
                    if (format == "!")
                        formatonstring = 1;
                    else if (format == "&")
                        formatonstring = int.MaxValue;
                    else if (format[0] == '\\') {
                        int k = 2;
                        foreach (char c in format.Substring(1)) {
                            if (c == ' ')
                                k++;
                            else if (c == '\\')
                                break;
                            else
                                throw new Exception();
                        }
                        formatonstring = k;
                    }
                    else
                        throw new Exception();
                }
                else if (format.Contains('#')) {
                    formatonstring = -1;
                    string strb = format;
                    string charfirst = "", charlast = "";
                    int issigned = 0, integer = 0, floaat = 0; bool has2star = false, hasdollor = false, isexpo = false, hascomma = false;
                    // 선행 문자열
                    while (true) {
                        if (strb.Length >= 0 && strb[0] == '_')
                            charfirst += strb[1];
                        else
                            break;
                        strb = strb.Remove(0, 2);
                    }
                    // 선행 부호
                    if (strb.Length >= 0 && strb[0] == '+') {
                        issigned = 1;
                        strb = strb.Remove(0, 1);
                    }
                    if (strb.Length >= 2 && strb.Substring(0, 2) == "**") {
                        has2star = true;
                        if (strb.Length >= 3 && strb[2] == '$') {
                            hasdollor = true;
                            strb = strb.Remove(0, 3);
                        }
                        else
                            strb = strb.Remove(0, 2);
                    }
                    else if (strb.Length >= 2 && strb.Substring(0, 2) == "$$") {
                        hasdollor = true;
                        strb = strb.Remove(0, 2);
                    }
                    bool tictok = true;
                    // 수 표현 부분
                    foreach (char c in strb) {
                        if (c == '#')
                            if (tictok)
                                integer++;
                            else
                                floaat++;
                        else if (c == '.')
                            if (tictok)
                                tictok = false;
                            else
                                throw new Exception();
                        else if (c == ',')
                            if (strb[strb.IndexOf(',') + 1] == '.' && !hascomma)
                                hascomma = true;
                            else
                                throw new Exception();
                        else
                            break;
                    }
                    strb = strb.Remove(0, integer + floaat + (tictok ? 0 : 1) + (hascomma ? 1 : 0));
                    if (strb != "") {
                        // 후행 부호
                        if (strb.Length >= 4 && strb.Substring(0, 4) == "^^^^") {
                            isexpo = true;
                            strb = strb.Remove(0, 4);
                            hascomma = false;
                        }
                        if (strb.Length > 0 && strb[0] == '+') {
                            if (issigned != 0)
                                throw new Exception();
                            issigned = 2;
                            strb = strb.Remove(0, 1);
                        }
                        else if (strb.Length > 0 && strb[0] == '-') {
                            if (issigned != 0)
                                throw new Exception();
                            issigned = 3;
                            strb = strb.Remove(0, 1);
                        }
                        // 후행 문자열
                        while (true) {
                            if (strb.Length > 0 && strb[0] == '_')
                                charlast += strb[1];
                            else
                                break;
                            strb = strb.Remove(0, 2);
                        }
                    }
                    // 제작 중
                    string temp = "";
                    for (int i = 0; i < integer; i++)
                        temp += "#";
                    if (floaat != 0) {
                        temp += ".";
                        for (int i = 0; i < floaat; i++)
                            temp += "0";
                    }
                    if (hascomma)
                        temp = temp.Insert(0, "#,");
                    if (hasdollor)
                        temp = temp.Insert(0, "'$'");
                    if (has2star)
                        temp = temp.Insert(0, "'**'");
                    if (isexpo)
                        temp += "E+00";
                    switch (issigned) {
                        case 0:
                            formatonnum = temp;
                            break;
                        case 1:
                            formatonnum = "'+'" + temp + ";'-'" + temp;
                            break;
                        case 2:
                            formatonnum = temp + "'+';" + temp + "'-'";
                            break;
                        case 3:
                            formatonnum = temp + ";" + temp + "'-'";
                            break;
                    }
                }
                else
                    throw new Exception();
                str = Utility.RemoveBlank(str.Replace("\"" + format + "\"", ""));
                if (str[0] != ';')
                    throw new Exception();
                str = Utility.RemoveBlank(str.Substring(1));
            }

            // seps에 대한 전면 재구성 필요.
            // ;으로 끝나면 이행을 하지 않음.
            // 계산을 위한 공백(앞 or 뒤에 연산자가 있으면, 앞뒤에 수치형 리터럴이 있으면)과 그렇지 않은 공백 구분 필요.

            if (str == "") {
                if (printnotlined)
                    printnotlined = !printnotlined;
                Console.WriteLine();
                return;
            }
            string result = "", expr = "";
            string stra = str;
            while (stra.Length > 0) {
                if (stra[0] == ' ' || stra[0] == ';')
                    stra = stra.Substring(1);
                else if (stra[0] == ',') {
                    result += "\t\t";
                    stra = stra.Substring(1);
                }
                else if (stra[0] == '\"') {
                    string value = stra.Split('\"')[1];
                    if (formatonstring < 1)
                        result += value;
                    else
                        result += value.Substring(0, (formatonstring > value.Length ? value.Length : formatonstring));
                    stra = stra.Remove(0, value.Length + 1);
                    if (stra.Length != 0 && stra[0] == '\"')
                        stra = stra.Remove(0, 1);
                }
                else {
                    // 불완전. 우선은 ;이나 ,로 토큰을 분리하도록 합시다.
                    string deletefor = "";
                    List<char> seps = new List<char>();
                    foreach (char c in stra)
                        if (c == ';' || c == ',')
                            seps.Add(c);
                    if (seps.Count == 0)
                        deletefor = stra;
                    else
                        for (int i = 0; i <= seps.Count; i++) {
                            deletefor += stra.Split(new[] { ';', ',' })[i];
                            if (Utility.CharAmount(deletefor, '\"') % 2 == 1) {
                                if (i != seps.Count)
                                    deletefor += seps[i];
                                continue;
                            }
                            if (Utility.CharAmount(deletefor, '(') != Utility.CharAmount(deletefor, ')')) {
                                if (i != seps.Count)
                                    deletefor += seps[i];
                                continue;
                            }
                            break;
                        }

                    stra = stra.Remove(0, deletefor.Length);
                    object vs = Utility.GetValue(deletefor, "", VarType.Str);
                    object vn = Utility.GetValue(deletefor, "", VarType.Sng);
                    if (vs == null || vs.GetType() != "".GetType()) {
                        if (formatonnum.Length != 0)
                            result += " " + (Convert.ToInt32(vn).ToString().Length > Utility.CharAmount(formatonnum.Split('.')[0], '#') ? "%" : "") + Convert.ToSingle(vn).ToString(formatonnum) + " ";
                        else
                            result += " " + Convert.ToSingle(vn).ToString() + " ";
                    }
                    else if (vn == null || vn.GetType() == "".GetType()) {
                        if (formatonstring < 1)
                            result += vs.ToString();
                        else
                            result += vs.ToString().Substring(0, (formatonstring > vs.ToString().Length ? vs.ToString().Length : formatonstring));
                    }
                    else if (Utility.IsValidVarRule(Utility.RemoveBlank(deletefor))) {
                        if (Utility.RemoveBlank(deletefor).Last() == '$')
                            result += "";
                        else
                            result = " 0 ";
                    }
                    else
                        throw new Exception();
                }
            }
            Console.Write(result);
            if (str.Last() == ';' || str.Last() == ',')
                printnotlined = true;
            else
                Console.WriteLine();
        }
        public static void RANDOMIZE(string str) {
            // RANDOMIZE
            // RND 함수를 위한 시드를 생성한다.
            str = Utility.RemoveBlank(str.Remove(0, 9));
            if (str.Replace(" ", "") == "")
                Functions.seed = new Random().Next();
            else if (str.Length == 1)
                Functions.seed = Convert.ToInt32(str[0]);
            else
                Functions.seed = BitConverter.ToInt32(Encoding.UTF8.GetBytes((str.Substring(0, 2))), 0);
        }
        public static void REM(string str) {
            // REM
            // 주석.
        }
        public static void RETURN(string str) {
            str = str.Remove(0, 6);
            if (str.Replace(" ", "") == "")
                Process.nowln = subroutineLine;
            else
                Process.nowln = Convert.ToInt32(Utility.GetValue(str, "", VarType.Int));
            subroutineLine = -10;
        }
        public static void RSET(string str) {
            // RSET
            // 문자열 변수에 문자열을 넣는다.
            str = Utility.RemoveBlank(str.Remove(0, 4));
            string var = str.Split('=')[0];
            string exp = Utility.RemoveBlank(Utility.GetValue(str.Replace(str.Split('=')[0], "").Substring(1), "", VarType.Str).ToString());
            if (!Utility.IsValidVarRule(Utility.RemoveBlank(var.Split(new[] { '(', '[' })[0])) || !var.Contains('$'))
                throw new Exception();
            Process.FindVar(true, var + "=\"" + exp + "\"");
        }
        public static void RUN(string str) // 미완성
        {
            // RUN [줄번호][, R]
            // R == 열렸던 파일을 계속 열어둘 것인가
            str = str.Remove(0, 3);
            Utility.RemoveBlank(ref str);
            int start = -1;
            if (str != "" && Char.IsDigit(str.ElementAt(0)))
                start = Convert.ToInt32(Utility.GetValue(str.Split(',')[0], "", VarType.Int));
            List<int> order = Input.Lines.Keys.OrderBy(num => num).ToList();
            int i = 0;
            if (start != -1) {
                i = -1;
                for (int j = 0; j < order.Count; j++)
                    if (start == order[j]) {
                        i = j;
                        break;
                    }
                if (i == -1) {
                    Console.WriteLine("Undefined line number");
                    return;
                }
            }
            do {
                if (trace)
                    Console.Write("[" + order[i] + "]");
                Process.BeginProcess(Input.Lines[order[i]], order[i], IsInLoof.None);
                if (order[i] == Process.nowln)
                    i++;
                else if (Process.nowln == -10)
                    break;
                else
                    for (int j = 0; j < order.Count; j++)
                        if (Process.nowln == order[j]) {
                            i = j;
                            break;
                        }
            } while (i < order.Count);

        }
        public static void SAVE(string str) {
            // SAVE 경로, a|p
            // 해당 경로에 프로그램을 저장한다. ,a가 붙으면 일반 텍스트, ,p가 붙으면 암호화되어, 안 붙어 있으면 토큰화되어 저장되는데, 일단 지금은 편의상 모두 일반 텍스트 파일로 저장한다.
            str = Utility.RemoveBlank(str.Remove(0, 4));
            int comma = Utility.CharAmount(str, ','); string pos, mod = "";
            if (comma == 0)
                pos = Utility.GetValue(str, "", VarType.Str).ToString();
            else {
                pos = Utility.GetValue(str.Split(',')[comma - 1], "", VarType.Str).ToString();
                mod = Utility.RemoveBlank(str.Split(',')[comma]);
            }
            if (mod != "" && mod != "A" && mod != "P")
                throw new Exception();
            FileStream fs = File.Create(pos); fs.Close();
            StreamWriter sw = new StreamWriter(pos);
            List<int> order = Input.Lines.Keys.OrderBy(num => num).ToList();
            foreach (int i in order)
                sw.WriteLine("{0} {1}", i, Input.Lines[i]);
            sw.Close();
        }
        public static void SCREEN(string str) {
            // SCREEN
            // 우선은 그래픽 출력이 가능해야 화면 모드를 바꾸든지 말든지 하지
            Console.WriteLine("Not support on this version");
        }
        public static void SHELL(string str) {
            // SHELL
            // 여기 안에서 도스를 돌린다. 지원하지 않습니다.
            Console.WriteLine("Not Support on this Version");
        }
        public static void SOUND(string str) {
            // SOUND 주파수, 길이
            // 길이/18.4초 동안 주파수만큼의 소리를 낸다. 0이면 소리가 나지 않지만. 0.022초(22밀리초)를 치면 무한히 들릴 것이다.
            str = Utility.RemoveBlank(str.Remove(0, 5));
            int freq = Convert.ToInt32(Utility.GetValue(str.Split(',')[0], "", VarType.Int));
            int dur = (int)(Convert.ToInt32(Utility.GetValue(str.Split(',')[1], "", VarType.Dbl)) / 18.2 * 1000);
            if (freq < 37 || freq > 32767)
                throw new Exception();
            if (dur == 1)
                dur = int.MaxValue;
            if (dur == 0) {
                Console.Beep(100, 1);
                return;
            }
            soundfreq = freq;
            sounddur = dur;
            Console.Beep(100, 1);
            Thread t = new Thread(new ThreadStart(sound));
            t.IsBackground = true;
            t.Start();
        }
        public static void sound() {
            // 위 SOUND의 스레딩 구현을 위한 함수
            Console.Beep(soundfreq, sounddur);
        }
        public static void STOP(string str) {
            // STOP
            // 프로그램 중단 - 파일 안 닫음
            str = str.Remove(0, 4);
            Utility.RemoveBlank(ref str);
            if (str == "") {
                breakLine = Process.nowln;
                Process.nowln = -10;
                Console.WriteLine("Break");
            }
        }
        public static void STRIG(string str) {
            // STRIG
            // 게임패드 트리거 관련 설정. 지원하지 않음.
            Console.WriteLine("Not Support on this Version");
        }
        public static void SWAP(string str) {
            // SWAP
            // 두 변수의 값을 서로 바꾼다.
            str = Utility.RemoveBlank(str.Remove(0, 4));
            string var1 = Utility.RemoveBlank(str.Split(',')[0]);
            string var2 = Utility.RemoveBlank(str.Split(',')[1]);
            string realvar1 = Utility.RemoveBlank(var1.Split(new[] { '(', '[' })[0]);
            string realvar2 = Utility.RemoveBlank(var2.Split(new[] { '(', '[' })[0]);
            if (!Input.Vars.ContainsKey(realvar1) || !Input.Vars.ContainsKey(realvar2))
                throw new Exception();
            if (Input.Vars[realvar1].Types != Input.Vars[realvar1].Types)
                throw new Exception();
            object temp = Utility.GetValue(var1, "", Input.Vars[realvar1].Types);
            Process.FindVar(true, var1 + "=" + Utility.GetValue(var2, "", Input.Vars[realvar2].Types).ToString());
            Process.FindVar(true, var2 + "=" + temp.ToString());
        }
        public static void TERM(string str) {
            // TERM
            // 뭔 소리야? 다른 환경에서만 지원.
            Console.WriteLine("Not Support on this Version");
        }
        public static void TRON(string str) {
            // TRON
            // 실행한 문자열을 표시한다.
            str = str.Remove(0, 4);
            if (str.Replace(" ", "") != "") {
                Console.WriteLine("Illegal function call"); return;
            }
            trace = true;
        }
        public static void TROFF(string str) {
            // TRON
            // 실행한 문자열을 표시한다.
            str = str.Remove(0, 5);
            if (str.Replace(" ", "") != "") {
                Console.WriteLine("Illegal function call"); return;
            }
            trace = false;
        }
        public static void VIEW(string str) // 미완성
        {
            str = Utility.RemoveBlank(str.Substring(0, 4));
        }
        public static void WAIT(string str) {
            // WAIT
            // 기계 코드 관련 문. INP도 지원하지 않는 마당에, 지원 안 함.
            Console.WriteLine("Not Support on this Version");
        }
        public static void WEND(string str) {
            // WEND
            // 이거랑 매치되는 WHILE로 돌아간다. 그리고 그건 얘가 해줄 예정.
            str = str.Remove(0, 4);
            List<int> order = Input.Lines.Keys.OrderBy(num => num).ToList();
            int whilec = 0;
            foreach (int i in order)
                if (i <= Process.nowln) {
                    if (Input.Lines[i].Contains("WHILE"))
                        whilec++;
                    if (Input.Lines[i].Contains("WEND"))
                        whilec--;
                }
                else
                    break;
            if (whilec < 0) {
                Console.WriteLine("WEND without WHILE");
                throw new Exception();
            }
        }
        public static void WHILE(string str) {
            // WHILE 조건식
            // 조건식이 -1이면 WEND까지 돌린다.
            str = str.Remove(0, 5);
            List<int> order = Input.Lines.Keys.OrderBy(num => num).ToList();
            int whilec = 0, set = -1, target = -1;
            // WEND 찾기
            foreach (int i in order) {
                if (Input.Lines[i].Contains("WHILE")) {
                    whilec++;
                    if (i == Process.nowln)
                        set = whilec - 1;
                }
                if (Input.Lines[i].Contains("WEND")) {
                    whilec--;
                    if (whilec == set) {
                        target = i;
                        break;
                    }
                }
            }
            if (target == -1) {
                Console.WriteLine("WHILE without WEND");
                throw new Exception();
            }

            set = Process.nowln;
            while (Convert.ToInt32(Utility.GetValue(str, "", VarType.Int)) != 0) {
                for (int cursor = order.IndexOf(set); cursor <= order.IndexOf(target); cursor++) {
                    if (trace)
                        Console.Write("[" + order[cursor] + "]");
                    if (cursor == order.IndexOf(set))
                        Process.BeginProcess(Input.Lines[order[cursor]], order[cursor], IsInLoof.While);
                    else
                        Process.BeginProcess(Input.Lines[order[cursor]], order[cursor], IsInLoof.None);
                }
            }
            Process.nowln = target;
        }
        public static void WIDTH(string str) {
            str = Utility.RemoveBlank(str.Remove(0, 5));
            if (str == "40") {
                if (Console.WindowWidth != 40) {
                    Console.Clear();
                    Console.SetWindowSize(40, 25);
                    Console.SetBufferSize(40, 25);
                }
            }
            else if (str == "80") {
                if (Console.WindowWidth != 80) {
                    Console.Clear();
                    Console.SetBufferSize(80, 25);
                    Console.SetWindowSize(80, 25);
                }
            }
            else {
                Console.WriteLine("Illegal function call");
                return;
            }
        }
        public static void WINDOW(string str) {
            Console.WriteLine("Not Support on this Version");
        }
        public static void SYSTEM(string str) {
            // SYSTEM
            str = str.Remove(0, 6);
            if (str.Replace(" ", "") != "") {
                Console.WriteLine("Illegal function call"); return;
            }
            Environment.Exit(0);
        }
    }
}