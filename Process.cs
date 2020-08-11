// Process.cs
// Input에서 받아 여기서 문장 인식 후 처리

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSBASIC {
    internal enum IsInLoof { None, For, While }
    class Process {
        internal static string[] FuncList = { "ABS", "ASC", "ATN", "CDBL", "CHR$", "CINT", "COS", "CSNG", "CSRLIN", "CVI", "CVS", "CVD", "ENVIRON$", "EOF", "ERDEV", "ERL", "ERR", "EXP", "EXTERR", "FIX", "FN", "FRE", "HEX$", "INKEY$", "INP", "INPUT$", "INSTR", "INT", "IOCTL$", "LEFT$", "LEN", "LOC", "LOF", "LOG", "LOPS", "MKD$", "MKI$", "MKS$", "OCT$", "PEEK", "PMAP", "POS", "RIGHT$", "RND", "SGN", "SIN", "SPACE$", "SQR", "STICK", "STR$", "STRING$", "TAN", "USR", "VAL", "VARPTR", "VARPTR$" };
        internal static string[] StatList = { "AUTO", "BEEP", "BLOAD", "BSAVE", "CALL", "CALLS", "CHAIN", "CHDIR", "CIRCLE", "CLEAR", "CLOSE", "CLS", "COLOR", "COM", "COMMON", "CONT", "DATA", "DEF", "DELETE", "DIM", "DRAW", "EDIT", "ELSE", "END", "ENVIRON", "ERASE", "ERROR", "FIELD", "FILES", "FOR", "GET", "GOSUB", "GOTO", "IF", "INPUT", "IOCTL", "KEY", "KILL", "LCOPY", "LET", "LINE", "LIST", "LLIST", "LOAD", "LOCATE", "LOCK", "LPRINT", "LSET", "MERGE", "MKDIR", "MOTOR", "NAME", "NEW", "NEXT", "NOISE", "ON", "OPEN", "OPTION BASE", "OUT", "PAINT", "PALETTE", "PCOPY", "PEN", "PLAY", "POKE", "PSET", "PRESET", "PRINT", "LPRINT", "PUT", "RANDOMIZE", "READ", "REM", "RENUM", "RESET", "RESTORE", "RESUME", "RETURN", "RMDIR", "RSET", "RUN", "SAVE", "SCREEN", "SHELL", "SOUND", "STOP", "STRIG", "SWAP", "SYSTEM", "TERM", "TRON", "TROFF", "UNLOCK", "VIEW", "WAIT", "WEND", "WHILE", "WIDTH", "WINDOW", "WRITE" };
        internal static string[] TempList = { "DATE$", "MID$", "PEN", "PLAY", "SCREEN", "STRIG", "TIME$", "TIMER" };
        internal static string[] OperList = { "+", "-", "*", "/", "\\", "^", " MOD ", "=", "<>", "><", "<", ">", "<=", ">=", "NOT ", " AND ", " OR ", " XOR ", " EQV ", " IMP ", "§" };
        // 이상 줄 함수, 문, 둘 다, 연산자 리스트
        static string str = "";
        internal static int nowln;
        static int errln = -1;
        static public int Geterrln => errln;

        static public void BeginProcess(string s, int line, IsInLoof loof) // 프로세스를 시작하며 저장.
        {
            nowln = line; errln = 0; str = s;
            // 앞 공백 제거
            Utility.RemoveBlank(ref str);
            // 콜론이 붙으면 중간에 갈라져요
            List<string> sepbyColon = new List<string>(); bool inq = false;
            for (int i = 0; i <= Utility.CharAmount(str, ':'); i++) {
                if (!inq)
                    sepbyColon.Add(Utility.RemoveBlank(s.Split(':')[i]));
                else
                    sepbyColon[sepbyColon.Count - 1] += ":" + Utility.RemoveBlank(s.Split(':')[i]);
                if (Utility.CharAmount(s.Split(':')[i], '\"') % 2 == 1)
                    inq = !inq;
            }
            try {
                if (sepbyColon.Count != 0)
                    foreach (string c in sepbyColon) {
                        str = c;
                        if ((loof == IsInLoof.For && c.Substring(0, 3) == "FOR") || (loof == IsInLoof.While && c.Substring(0, 5) == "WHILE"))
                            continue;
                        if (!FindStat())
                            FindVar(false, str);
                    }
                else {
                    if ((loof == IsInLoof.For && str.Substring(0, 3) == "FOR") || (loof == IsInLoof.While && str.Substring(0, 5) == "WHILE"))
                        return;
                    if (!FindStat())
                        FindVar(false, str);
                }
                if (line == -1) {
                    if (Statements.printnotlined) {
                        Console.WriteLine();
                        Statements.printnotlined = !Statements.printnotlined;
                    }
                    Console.WriteLine("Ok");
                }
            }
            catch { Console.WriteLine("Error occurred"); }
        }
        static bool FindStat() // 먼저 처음 나올 수밖에 없는 문을 찾는다.
        {
            // ? 즉각싷행 -> PRINT로
            if (str[0] == '?') {
                Statements.PRINT(str);
                return true;
            }
            string statement;
            if (str.Contains(' '))
                statement = str.Split(' ')[0];
            else
                statement = str;

            // 이제 그 문을 처리하는 곳으로 넘어간다.
            MethodInfo destination;
            Statements a = new Statements(); Type type = a.GetType();
            if (StatList.Contains(statement) || TempList.Contains(statement)) {
                if (statement.Contains('$'))
                    statement = statement.Split('$')[0];
                destination = type.GetMethod(statement);
                if (destination != null) {
                    object[] arr = { str };
                    destination.Invoke(statement, arr);
                    return true;
                }
            }
            return false;
        }
        internal static bool FindVar(bool isinWork, string str) // 변수를 찾는다. 변수 리스트에 없다면 변수를 만든다.
        {
            // 함수에서 변수를 찾는 것이 아니라면
            if (!isinWork)
                str = Process.str;
            // 변수, 배열 모두 체크할 것.
            int variable = 0;
            foreach (char c in str)
                if (char.IsUpper(c) || char.IsDigit(c) || c == '.')
                    variable++;
                else
                    break;
            if (variable == 0 || !char.IsUpper(str[0]))
                return false;
            if (FuncList.Contains(str.Substring(0, variable)) || StatList.Contains(str.Substring(0, variable)) || TempList.Contains(str.Substring(0, variable)) || OperList.Contains(str.Substring(0, variable)))
            // 변수의 이름이 예약자이면
            {
                Console.WriteLine("Syntax Error");
                return false;
            }
            if (str.Length == variable)
            // 변수가 그냥 단독으로만 적혔다면
            {
                Console.WriteLine("Syntax Error");
                return false;
            }

            // 변수 이름 검색
            int arrnum = 0;
            int[] dig = new int[5] { 0, 0, 0, 0, 0 };
            try {
                // 자료형 문자 및 배열 여부 체크
                switch (str[variable]) {
                    case '$'://str
                    case '%'://int
                    case '!'://sng
                    case '#'://dbl
                        variable++;
                        switch (str[variable]) {
                            case '(':
                            case '[':
                                if (char.IsDigit(Utility.RemoveBlank(str.Substring(variable + 1))[0]) || Input.Vars.ContainsKey(str.Substring(variable + 1).Split(',')[0])) {
                                    arrnum = str.Substring(variable + 1).Length - str.Substring(variable + 1).Replace(",", "").Length + 1;
                                    if (arrnum > 6)
                                        throw new IndexOutOfRangeException();
                                    for (int i = 0; i < arrnum - 1; i++) {
                                        if (!(int.TryParse(str.Substring(variable + 1).Split(',')[i], out dig[i]) || int.TryParse(Utility.GetValue(str.Substring(variable + 1).Split(',')[i].Split(')', ']')[0], "", VarType.Int).ToString(), out dig[i])))
                                            throw new TypeMismatchException();
                                        if ((!Input.Vars.ContainsKey(str.Substring(0, variable)) && dig[i] > 10) || (Input.Vars[str.Substring(0, variable)].ArrayNum != 0 && dig[i] > Input.Vars[str.Substring(0, variable)].ArraySize[i]))
                                            throw new IndexOutOfRangeException();
                                    }
                                    if (!int.TryParse(str.Substring(variable + 1).Split(',')[arrnum - 1].Split(')', ']')[0], out dig[arrnum - 1]) && !int.TryParse(Utility.GetValue(str.Substring(variable + 1).Split(',')[arrnum - 1].Split(')', ']')[0], "", VarType.Int).ToString(), out dig[arrnum - 1]))
                                        throw new Exception();
                                }
                                break;
                        }
                        break;
                    case '(':
                    case '[':
                        if (char.IsDigit(Utility.RemoveBlank(str.Substring(variable + 1))[0]) || Input.Vars.ContainsKey(str.Substring(variable + 1).Split(new[] { ')', ']' })[0].Split(',')[0])) {
                            arrnum = str.Substring(variable + 1).Length - str.Substring(variable + 1).Replace(",", "").Length + 1;
                            if (arrnum > 6)
                                throw new IndexOutOfRangeException();
                            for (int i = 0; i < arrnum - 1; i++) {
                                if (!(int.TryParse(str.Substring(variable + 1).Split(',')[i], out dig[i]) || int.TryParse(Utility.GetValue(str.Substring(variable + 1).Split(',')[i].Split(')', ']')[0], "", VarType.Int).ToString(), out dig[i])))
                                    throw new TypeMismatchException();

                                if ((!Input.Vars.ContainsKey(str.Substring(0, variable)) && dig[i] > 10) || (Input.Vars[str.Substring(0, variable)].ArrayNum != 0 && dig[i] > Input.Vars[str.Substring(0, variable)].ArraySize[i]))
                                    throw new IndexOutOfRangeException();
                            }
                            if (!int.TryParse(str.Substring(variable + 1).Split(',')[arrnum - 1].Split(')', ']')[0], out dig[arrnum - 1]) && !int.TryParse(Utility.GetValue(str.Substring(variable + 1).Split(',')[arrnum - 1].Split(')', ']')[0], "", VarType.Int).ToString(), out dig[arrnum - 1]))
                                throw new Exception();
                        }
                        break;
                }
            }
            catch (IndexOutOfRangeException e) {
                Console.WriteLine("Subscript out of range");
                return false;
            }
            catch (TypeMismatchException e) {
                Console.WriteLine("Type mismatch");
                return false;
            }
            catch {
                Console.WriteLine("Syntax Error");
                return false;
            }

            // 변수 이름 및 자료형 체크
            string varname = str.Substring(0, variable);
            VarType type;
            char typeword = str[variable - 1];
            switch (typeword) {
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
                default:
                    type = VarType.Sng;
                    break;
            }

            // 새로운 변수 발견
            string stra = "";
            object value = new object();
            int arrdig = 0;
            char[] a = new char[2] { ')', ']' };
            if (arrnum != 0)
                stra = str.Substring(str.IndexOfAny(a) + 1);
            else
                stra = str.Substring(variable);
            Utility.RemoveBlank(ref stra);

            // 할당
            try {
                if (stra[0] == '=') {
                    stra = stra.Substring(1);
                    value = Utility.GetValue(stra, varname, type);
                    if (value == null)
                        throw new TypeMismatchException();
                    if (arrnum != 0) {
                        switch (arrnum) {
                            case 1: {
                                    object[] array;
                                    if (Input.Vars.Keys.Contains(varname))
                                        array = (object[])Input.Vars[varname].Value;
                                    else
                                        array = (object[])Utility.ArrayInitialize(arrnum, type);
                                    array[dig[0]] = value;
                                    value = array;
                                    break;
                                }
                            case 2: {
                                    object[,] array;
                                    if (Input.Vars.Keys.Contains(varname))
                                        array = (object[,])Input.Vars[varname].Value;
                                    else
                                        array = (object[,])Utility.ArrayInitialize(arrnum, type);
                                    array[dig[0], dig[1]] = value;
                                    value = array;
                                    break;
                                }
                            case 3: {
                                    object[,,] array;
                                    if (Input.Vars.Keys.Contains(varname))
                                        array = (object[,,])Input.Vars[varname].Value;
                                    else
                                        array = (object[,,])Utility.ArrayInitialize(arrnum, type);
                                    array[dig[0], dig[1], dig[2]] = value;
                                    value = array;
                                    break;
                                }
                            case 4: {
                                    object[,,,] array;
                                    if (Input.Vars.Keys.Contains(varname))
                                        array = (object[,,,])Input.Vars[varname].Value;
                                    else
                                        array = (object[,,,])Utility.ArrayInitialize(arrnum, type);
                                    array[dig[0], dig[1], dig[2], dig[3]] = value;
                                    value = array;
                                    break;
                                }
                            case 5: {
                                    object[,,,,] array;
                                    if (Input.Vars.Keys.Contains(varname))
                                        array = (object[,,,,])Input.Vars[varname].Value;
                                    else
                                        array = (object[,,,,])Utility.ArrayInitialize(arrnum, type);
                                    array[dig[0], dig[1], dig[2], dig[3], dig[4]] = value;
                                    value = array;
                                    break;
                                }
                        }
                    }
                    VarInfo done; done.Types = type; done.Value = value; done.ArrayNum = arrnum; done.ArraySize = new List<int>();
                    for (int i = 0; i < arrnum; i++)
                        done.ArraySize.Add(10);
                    if (Input.Vars.Keys.Contains(varname))
                        if (done.Types == Input.Vars[varname].Types)
                            Input.Vars[varname] = done;
                        else {
                            Console.WriteLine("Type mismatch");
                            return false;
                        }
                    else
                        Input.Vars.Add(varname, done);
                    return true;
                }
            }
            catch (IndexOutOfRangeException e) {
                Console.WriteLine("Subscript out of range");
                return false;
            }
            catch (TypeMismatchException e) {
                Console.WriteLine("Type mismatch");
                return false;
            }
            catch {
                Console.WriteLine("Syntax error");
                return false;
            }
            return false;

        }
        internal static object FindFunc(string str, VarType vartype) {
            string function;
            if (str.Contains(' ') || str.Contains('('))
                function = str.Split(' ', '(')[0];
            else
                function = str;
            object result = new object();

            // 이제 그 문을 처리하는 곳으로 넘어간다.
            MethodInfo destination;
            Functions a = new Functions(); Type type = a.GetType();
            if (FuncList.Contains(function) || TempList.Contains(function)) {
                if (function.Contains('$'))
                    function = function.Split('$')[0];
                destination = type.GetMethod(function);
                if (destination != null) {
                    object[] arr = { str, vartype };
                    try {
                        result = destination.Invoke(function, arr);
                    }
                    catch { result = null; }
                    if (result == null)
                        return null;
                    return result;
                }
            }
            return null;
        }
    }
}