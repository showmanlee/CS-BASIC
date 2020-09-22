//Functions.cs
//값을 반환하는 함수의 집합
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSBASIC {
    class Functions {
        static double lastRND = -1;
        internal static int seed = 0;

        public static object ABS(string str, VarType vartype) {
            //ABS(값)
            //값이 수치형일 경우 절댓값, 문자열일 경우 그 문자열을 출력한다.
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    result = Math.Abs((float)result);
                    if ((float)result % 1.0f >= 0.5)
                        result = (int)result + 1;
                    else
                        result = (int)result;
                    break;
                case VarType.Sng:
                    result = Math.Abs((float)result);
                    break;
                case VarType.Dbl:
                    result = Math.Abs((double)result);
                    break;
            }
            return result;
        }
        public static object ASC(string str, VarType vartype) {
            //ASC(문자)
            //해당 문자의 아스키 코드 값을 뱉는다.
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", VarType.Str);
            if (vartype == VarType.Str || result.GetType() != "".GetType())
                throw new TypeMismatchException();
            else if ((string)result == "")
                throw new ArgumentException();
            else
                return Convert.ToInt32(Utility.RemoveQuotemark(str)[0]);
        }
        public static object ATN(string str, VarType vartype) {
            // ATN(값)
            // 역탄젠트 값을 뱉는다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Math.Atan((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Math.Atan((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Math.Atan((double)result));
            }
            return null;
        }
        public static object CDBL(string str, VarType vartype) {
            // CDBL(값)
            // 배정밀도 실수형으로 변환한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Convert.ToDouble((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Convert.ToDouble((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Convert.ToDouble((double)result));
            }
            return null;
        }
        public static object CHR(string str, VarType vartype) {
            // CHR$(값)
            // 해당 값의 아스키 코드에 해당하는 문자를 반환한다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", VarType.Int);
            try { return Convert.ToChar((int)result).ToString(); }
            catch { Console.WriteLine("Overflow"); return null; }
        }
        public static object CINT(string str, VarType vartype) {
            // CINT(값)
            // 정수형으로 변환한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Convert.ToInt32((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Convert.ToInt32((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Convert.ToInt32((double)result));
            }
            return null;
        }
        public static object COS(string str, VarType vartype) {
            // COS(값)
            // 코사인 값을 뱉는다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Math.Cos((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Math.Cos((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Math.Cos((double)result));
            }
            return null;
        }
        public static object CSNG(string str, VarType vartype) {
            // CSNG(값)
            // 단정밀도 실수형으로 변환한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Convert.ToSingle((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Convert.ToSingle((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Convert.ToSingle((double)result));
            }
            return null;
        }
        public static object CSRLIN(string str, VarType vartype) {
            // CSRLIN
            // 현재 화면 커서가 위치한 줄을 출력한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Console.CursorTop);
                case VarType.Sng:
                    return Convert.ToSingle(Console.CursorTop);
                case VarType.Dbl:
                    return Convert.ToDouble(Console.CursorTop);
            }
            return null;
        }
        public static object CVI(string str, VarType vartype) {
            // CVI(문자열)
            // 문자열의 최초 문자 2개를 정수형으로 반환한다. 마치 C에서 마우스 갖다 댇 것처럼.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", VarType.Str);
            string foo = result as string;
            short newresult = BitConverter.ToInt16(Encoding.UTF8.GetBytes((foo.Substring(0, 2))), 0);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(newresult);
                case VarType.Sng:
                    return Convert.ToSingle(newresult);
                case VarType.Dbl:
                    return Convert.ToDouble(newresult);
            }
            return null;
        }
        public static object CVS(string str, VarType vartype) {
            // CVS(문자열)
            // 문자열의 최초 문자 4개를 단정밀도 실수형으로 반환한다. 마치 C에서 마우스 갖다 댇 것처럼.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", VarType.Str);
            string foo = result as string;
            float newresult = BitConverter.ToSingle(Encoding.UTF8.GetBytes((foo.Substring(0, 4))), 0);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(newresult);
                case VarType.Sng:
                    return Convert.ToSingle(newresult);
                case VarType.Dbl:
                    return Convert.ToDouble(newresult);
            }
            return null;
        }
        public static object CVD(string str, VarType vartype) {
            // CVD(문자열)
            // 문자열의 최초 문자 8개를 배정밀도 실수형으로 반환한다. 마치 C에서 마우스 갖다 댇 것처럼.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", VarType.Str);
            string foo = result as string;
            double newresult = BitConverter.ToDouble(Encoding.UTF8.GetBytes((foo.Substring(0, 8))), 0);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(newresult);
                case VarType.Sng:
                    return Convert.ToSingle(newresult);
                case VarType.Dbl:
                    return Convert.ToDouble(newresult);
            }
            return null;
        }
        public static object DATE(string str, VarType vartype) {
            // DATE$
            // 현재 날짜를 출력한다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            string result = DateTime.Today.Month.ToString("D2") + "-" + DateTime.Today.Day.ToString("D2") + "-" + DateTime.Today.Year.ToString("D4");
            return result;
        }
        public static object ENVIRON(string str, VarType vartype) // 미완성
        {
            // ENVIRON$(매변)
            // 매개 변수가 문자열이면 매개 변수 이름의 환경 변수를 출력한다.
            // 매개 변수가 정수형이면 해당 번호의 환경 변수를 출력한다.
            return null;
        }
        public static object EOF(string str, VarType vartype) // 미완성
        {
            // EOF(X)
            // X번 파일이 파일에 끝에 도달하면 -1을 출력한다.
            return null;
        }
        public static object ERDEV(string str, VarType vartype) {
            // ERDEV
            // 기기 오류 코드를 밷는데, 지원하지 않을 예정이다.
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object ERR(string str, VarType vartype) {
            // ERR
            // 에러가 일어난 줄을 뱉는다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Process.Geterrln);
                case VarType.Sng:
                    return Convert.ToSingle(Process.Geterrln);
                case VarType.Dbl:
                    return Convert.ToDouble(Process.Geterrln);
            }
            return null;
        }
        public static object EXP(string str, VarType vartype) {
            // EXP(값)
            // e^x를 구한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Math.Exp((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Math.Exp((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Math.Exp((double)result));
            }
            return null;
        }
        public static object EXTERR(string str, VarType vartype) {
            // EXTERR
            // MS-DOS 상에서 뜬 오류를 보여주는데, 어차피 도스에서 안 돌아가잖아 이거.
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object FIX(string str, VarType vartype) {
            // FIX(X)
            // 소수를 정수로 만드는데, 음수는 0에 가깝게 한다(c# 디폴트).
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Convert.ToInt32((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Convert.ToInt32((double)result));
            }
            return null;
        }
        public static object FN(string str, VarType vartype) {
            // FN
            // 사용자 지정 함수를 사용한다.
            str = str.Remove(0, 2);
            str = Utility.RemoveBlank(str);
            // 함수 이름
            string name = Utility.RemoveBlank(str.Split('(', ' ')[0]);
            if (!Input.FNs.Keys.Contains(name))
                return null;
            if (!(vartype == VarType.Str ^ Input.FNs[name].ReturnType != VarType.Str))
                throw new TypeMismatchException();

            object result = new object();
            str = str.Replace(name, "");
            str = str.Remove(str.LastIndexOf(')'));
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            // 만약 인수가 없다면
            if (Input.FNs[name].Arg.Count == 0)
                if (Input.FNs[name].ReturnType == VarType.Str)
                    return Utility.GetValue(Input.FNs[name].Expr, "", VarType.Str);
                else
                    return Utility.GetValue(Input.FNs[name].Expr, "", VarType.Sng);

            // 주어진 인수 확인
            List<string> exp = new List<string>();
            List<string> inQuote = new List<string>();
            for (int i = 0; i < Utility.CharAmount(str, '\"'); i += 2) {
                inQuote.Add("\"" + str.Split('\"')[0] + "\"");
                str = str.Replace(inQuote[i / 2], "\"");
            }
            for (int i = 0, j = 0; i <= Utility.CharAmount(str, ','); i++) {
                if (str.Split(',')[i].Contains("\""))
                    exp.Add(str.Split(',')[i].Replace("\"", inQuote[j++]));
                else
                    exp.Add(str.Split(',')[i]);
            }
            if (exp.Count() != Input.FNs[name].Arg.Count())
                throw new ArgumentException();
            // 인수 대응하기
            int k = 0; string expr = Input.FNs[name].Expr;
            foreach (string s in Input.FNs[name].Arg.Keys) {
                if (Utility.GetValue(exp[k], "", Input.FNs[name].Arg[s]) == null)
                    throw new TypeMismatchException();
                expr = expr.Replace(s, exp[k]);
                k++;
            }
            if (Input.FNs[name].ReturnType == VarType.Str)
                return Utility.GetValue(expr, "", VarType.Str).ToString();
            else
                switch (vartype) {
                    case VarType.Int:
                        return Convert.ToInt32(Utility.GetValue(expr, "", Input.FNs[name].ReturnType));
                    case VarType.Sng:
                        return Convert.ToSingle(Utility.GetValue(expr, "", Input.FNs[name].ReturnType));
                    case VarType.Dbl:
                        return Convert.ToDouble(Utility.GetValue(expr, "", Input.FNs[name].ReturnType));
                }
            return null;
        }
        public static object FRE(string str, VarType vartype) {
            // FRE(값)
            // 값이 문자열이면 메모리를 정리하고 나온 값, 아니면 그냥 남은 메모리를 출력한다.
            Console.WriteLine("I said Many of bytes are FREE");
            if (vartype == VarType.Str)
                return "";
            else
                return 0;
        }
        public static object HEX(string str, VarType vartype) {
            // HEX$(값)
            // 해당 값의 16진수형을 문자열로 출력한다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            int result = Convert.ToInt32(Utility.GetValue(str, "", VarType.Dbl));
            return result.ToString("X");
        }
        public static object INKEY(string str, VarType vartype) // 미완성
        {
            // INKEY$
            // 현재 키보드 버퍼의 문자를 출력한다.
            return null;
        }
        public static object INP(string str, VarType vartype) {
            // INP(X)
            // 기계 포트에서의 값을 반환하지만 지금은 지원하지 않을 예정.
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object INPUT(string str, VarType vartype) // 미완성
        {
            // INPUT$(X, #N)
            // #N이 정의되지 않으면 N번 파일에서, 그렇지 않으면 키보드에서 X글자를 받는다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 6);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            string result = "";
            int x, n = -1;
            if (str.Contains(',')) {
                x = Convert.ToInt32(Utility.GetValue(str.Split(',')[0], "", VarType.Int));
                n = Convert.ToInt32(Utility.GetValue(str.Contains('#') ? str.Split(',')[1].Split('#')[1] : str.Split(',')[1], "", VarType.Int));
            }
            else {
                x = Convert.ToInt32(Utility.GetValue(str.Split(',')[0], "", VarType.Int));
                result = Console.ReadLine();
            }
            result = result.Substring(0, x);
            return result;
        }
        public static object INSTR(string str, VarType vartype) {
            // INSTR(X, A, B)
            // 문자열 A의 (X번 자리부터) 문자열 B가 등장하는 자리를 반환한다. 인덱스는 1부터.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 5);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);

            List<string> inQuote = new List<string>();
            for (int i = 0; i < Utility.CharAmount(str, '\"'); i += 2) {
                inQuote.Add(str.Split('\"')[i + 1]);
                str = str.Replace(inQuote[i / 2], "");
            }
            string a, b; int x;
            switch (Utility.CharAmount(str, ',')) {
                case 1:
                    if (str.Split(',')[0].Contains('\"'))
                        if (str.Split(',')[1].Contains('\"')) {
                            a = inQuote[0];
                            b = inQuote[1];
                        }
                        else {
                            a = inQuote[0];
                            b = Utility.GetValue(str.Split(',')[1], "", VarType.Str).ToString();
                        }
                    else if (str.Split(',')[1].Contains('\"')) {
                        a = Utility.GetValue(str.Split(',')[0], "", VarType.Str).ToString();
                        b = inQuote[0];
                    }
                    else {
                        a = Utility.GetValue(str.Split(',')[0], "", VarType.Str).ToString();
                        b = Utility.GetValue(str.Split(',')[1], "", VarType.Str).ToString();
                    }
                    return Convert.ToInt32(a.IndexOf(b) + 1);
                case 2:
                    x = Convert.ToInt32(Utility.GetValue(str.Split(',')[0], "", VarType.Int));
                    if (str.Split(',')[1].Contains('\"'))
                        if (str.Split(',')[2].Contains('\"')) {
                            a = inQuote[0];
                            b = inQuote[1];
                        }
                        else {
                            a = inQuote[0];
                            b = Utility.GetValue(str.Split(',')[2], "", VarType.Str).ToString();
                        }
                    else if (str.Split(',')[2].Contains('\"')) {
                        a = Utility.GetValue(str.Split(',')[1], "", VarType.Str).ToString();
                        b = inQuote[0];
                    }
                    else {
                        a = Utility.GetValue(str.Split(',')[1], "", VarType.Str).ToString();
                        b = Utility.GetValue(str.Split(',')[2], "", VarType.Str).ToString();
                    }
                    return Convert.ToInt32(a.IndexOf(b, x - 1) + 1);
                default:
                    return null;
            }
        }
        public static object INT(string str, VarType vartype) {
            // INT(X)
            // 소수를 정수로 만드는데, 음수는 -무한대에 가깝게 한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            double result = Convert.ToDouble(Utility.GetValue(str, "", VarType.Dbl));
            if (result < 0)
                result -= 1;
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(result);
                case VarType.Sng:
                    return Convert.ToSingle(Convert.ToInt32(result));
                case VarType.Dbl:
                    return Convert.ToDouble(Convert.ToInt32(result));
            }
            return null;
        }
        public static object IOCTL(string str, VarType vartype) {
            // IOCTL
            // 기기에 명령을 넣은 걸 뭐 어쩌란 건데, 어차피 구현 안 됨.
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object LEFT(string str, VarType vartype) {
            // LEFT$(문자열, 문자개수)
            // 문자열에서 왼쪽에 있는 몇 개 문자를 가져온다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 5);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);

            string s;
            if (str.Contains('\"')) {
                s = str.Split('\"')[1];
                str = str.Replace(s, "");
            }
            else
                s = Utility.GetValue(str.Split(',')[0], "", VarType.Str).ToString();
            int x = Convert.ToInt32(Utility.GetValue(str.Split(',')[1], "", VarType.Int));
            return s.Substring(0, x);
        }
        public static object LEN(string str, VarType vartype) {
            // LEN(S)
            // 문자열의 길이를 구한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            string result = Utility.GetValue(str, "", VarType.Str).ToString();
            return result.Length;
        }
        public static object LOC(string str, VarType vartype) {
            // LOC(X)
            // X번 파일이 열린 바이트 주소를 구한다. 젠장 바이트잖아?
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object LOF(string str, VarType vartype) // 미완성
        {
            // LOF(X)
            // X번 파일의 크기를 바이트로 반환한다.
            return null;
        }
        public static object LOG(string str, VarType vartype) {
            // LOG(값)
            // 자연로그 값을 구한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Math.Log((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Math.Log((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Math.Log((double)result));
            }
            return null;
        }
        public static object LPOS(string str, VarType vartype) {
            // LPOS(X)
            // 연결된 X번 프린터의 열 위치를 구한다. 그런데 지금 이렇게 열 위치를 구하는 프린터가 없지 아마?
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object MID(string str, VarType vartype) {
            // MID$(문자열, 시작, 길이)
            // 문자열에서 길이가 주어졌으면 시작 인덱스에서 끝까지, 그렇지 않으면 시작 인덱스에서 길이만큼의 서브스트링을 내놓는다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);

            string s;
            if (str[0] == '\"') {
                s = str.Split('\"')[1];
                str = str.Replace(s, "");
            }
            else
                s = Utility.GetValue(str.Split(',')[0], "", VarType.Str).ToString();
            int a, b;
            a = Convert.ToInt32(Utility.GetValue(str.Split(',')[1], "", VarType.Int));
            if (Utility.CharAmount(str, ',') == 2) {
                b = Convert.ToInt32(Utility.GetValue(str.Split(',')[2], "", VarType.Int));
                return s.Substring(a - 1, b);
            }
            else if (Utility.CharAmount(str, ',') == 1)
                return s.Substring(a - 1);
            else
                return null;
        }
        public static object MKD(string str, VarType vartype) {
            // MKD$(값)
            // CVD의 역함수.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            double t = Convert.ToDouble(Utility.GetValue(str, "", VarType.Dbl));
            byte[] res = BitConverter.GetBytes(t);
            string result = Encoding.Default.GetString(res);
            return result;
        }
        public static object MKI(string str, VarType vartype) {
            // MKI$(값)
            // CVI의 역함수.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            short t = Convert.ToInt16(Utility.GetValue(str, "", VarType.Int));
            byte[] res = BitConverter.GetBytes(t);
            string result = Encoding.Default.GetString(res);
            return result;
        }
        public static object MKS(string str, VarType vartype) {
            // MKS$(값)
            // CVS의 역함수.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            float t = Convert.ToSingle(Utility.GetValue(str, "", VarType.Sng));
            byte[] res = BitConverter.GetBytes(t);
            string result = Encoding.Default.GetString(res);
            return result;
        }
        public static object OCT(string str, VarType vartype) {
            // OCT$(값)
            // 해당 값의 8진수형을 문자열로 출력한다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            int result = Convert.ToInt32(Utility.GetValue(str, "", VarType.Dbl));
            return result.ToString("D8");
        }
        public static object PEEK(string str, VarType vartype) // 미완성
        {
            // PEEK(주소)
            // DEF SEG로 지정한 메모리 세그먼트 * 16 + X에 있는 값을 출력한다... 뭐야?
            return null;
        }
        public static object PEN(string str, VarType vartype) {
            // PEN
            // 라이트 펜의 입력을 받는다. 우리 펜 없음 ㅅㄱ
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object PLAY(string str, VarType vartype) {
            // PLAY
            // 배경 음악을 재생한다. 현재는 일단 무시하자.
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object PMAP(string str, VarType vartype) {
            // PMAP
            // 시야와 WINDOW로 지정한 영역의 좌표를 변환한다. 근데 그래픽 한정임.
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object POINT(string str, VarType vartype) {
            // POINT
            // 현재 그래픽의 커서 혹은 그 좌표의 상태를 표시한다. 그래픽 한정이겠지?
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object POS(string str, VarType vartype) {
            // POS(x)
            // 현재 화면 커서가 위치한 칸을 출력한다. 단 안에 인수가 들어가야 함.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            if (str == "")
                throw new Exception();
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Console.CursorLeft);
                case VarType.Sng:
                    return Convert.ToSingle(Console.CursorLeft);
                case VarType.Dbl:
                    return Convert.ToDouble(Console.CursorLeft);
            }
            return null;
        }
        public static object RIGHT(string str, VarType vartype) {
            // RIGHT$(문자열, 개수)
            // 해당 문자열 가장 끝에 있는 문자 몇 개를 반환한다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 6);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);

            string s;
            if (str.Contains('\"'))
                s = str.Split('\"')[1];
            else
                s = Utility.GetValue(str.Split(',')[0], "", VarType.Str).ToString();
            int a = Convert.ToInt32(Utility.GetValue(str.Split(',')[1], "", VarType.Int));
            if (a > s.Length)
                return s;
            else
                return s.Substring(s.Length - a, a);
        }
        public static object RND(string str, VarType vartype) {
            // RND(x)
            // 0~1까지의 단정밀도 실수형으로 표시되는 난수를 생성한다. 인수 없거나 0 이상이면 새롭게, 0이면 아까 거, 없으면 새로운 시드 지정.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            Random r = new Random(seed);
            double result = 0.0;
            if (!str.Contains('('))
                result = r.NextDouble();
            else {
                str = str.Remove(str.LastIndexOf(')'));
                str = str.Remove(0, 3);
                str = Utility.RemoveBlank(str);
                str = str.Remove(0, 1);
                int c = Convert.ToInt32(Utility.GetValue(str, "", VarType.Int));
                if (c > 0)
                    result = r.NextDouble();
                else if (c == 0)
                    result = lastRND;
                else {
                    r = new Random(c);
                    result = r.NextDouble();
                }
            }
            lastRND = result;
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(result);
                case VarType.Sng:
                    return Convert.ToSingle(result);
                case VarType.Dbl:
                    return result;
            }
            return null;
        }
        public static object SCREEN(string str, VarType vartype) {
            // SCREEN(y, x, f)
            // 해당 칸에 있는 문자의 코드를 출력한다. 만약 f가 정의되고 0이 아니라면 해당 칸의 상태를 출력한다.
            // 그런데 시도하려고 했으나 지금은 안 된다. 미안!
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object SGN(string str, VarType vartype) {
            // SGN(n)
            // 해당 값의 부호를 반환한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            double result = Convert.ToDouble(Utility.GetValue(str, "", VarType.Dbl));
            if (result == 0)
                return 0;
            else if (result > 0)
                return 1;
            else
                return -1;
        }
        public static object SIN(string str, VarType vartype) {
            // SIN(값)
            // 사인 값을 뱉는다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Math.Sin((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Math.Sin((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Math.Sin((double)result));
            }
            return null;
        }
        public static object SPACE(string str, VarType vartype) {
            // SPACE$(x)
            // x칸의 공백으로 구성된 문자열을 출력한다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 6);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            string result = "";
            int a = Convert.ToInt32(Utility.GetValue(str, "", VarType.Int));
            for (int i = 0; i < a; i++)
                result += " ";
            return result;
        }
        public static object SQR(string str, VarType vartype) {
            // SQR(값)
            // 제곱근 값을 뱉는다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Math.Sqrt((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Math.Sqrt((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Math.Sqrt((double)result));
            }
            return null;
        }
        public static object STICK(string str, VarType vartype) {
            // STICK
            // 조이스틱이 물려 있어야지 원.
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object STR(string str, VarType vartype) {
            // STR$(x)
            // 수치형 x의 tostring!
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 4);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            double a = Convert.ToDouble(Utility.GetValue(str, "", VarType.Dbl));
            return a.ToString();
        }
        public static object STRIG(string str, VarType vartype) {
            // STRIG
            // 조이스틱이 물려 있어야지 원.
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object STRING(string str, VarType vartype) {
            // STRING$(x, c)
            // 문자 C가 x개 있는 문자열을 출력한다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 7);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            int a = Convert.ToInt32(Utility.GetValue(str.Split(',')[0], "", VarType.Int));
            string result = "";
            try {
                char s = Convert.ToChar(Convert.ToInt32(Utility.GetValue(str.Split(',')[1], "", VarType.Int)));
                for (int i = 0; i < a; i++)
                    result += s;
                return result;
            }
            catch {
                string s = Utility.GetValue(str.Split(',')[1], "", VarType.Str).ToString();
                for (int i = 0; i < a; i++)
                    result += s[0];
                return result;
            }
        }
        public static object TAN(string str, VarType vartype) {
            // TAN(값)
            // 탄젠트 값을 뱉는다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            object result = Utility.GetValue(str, "", vartype);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Math.Tan((int)result));
                case VarType.Sng:
                    return Convert.ToSingle(Math.Tan((float)result));
                case VarType.Dbl:
                    return Convert.ToDouble(Math.Tan((double)result));
            }
            return null;
        }
        public static object TIME(string str, VarType vartype) {
            // TIME$
            // 현재 시간을 출력한다.
            if (vartype != VarType.Str)
                throw new TypeMismatchException();
            string result = DateTime.Now.Hour.ToString("D2") + ":" + DateTime.Now.Minute.ToString("D2") + ":" + DateTime.Now.Second.ToString("D2");
            return result;
        }
        public static object TIMER(string str, VarType vartype) {
            // TIMER
            // 자정에서 지난 시간을 초 단위로 출력한다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            long temp = DateTime.Now.Ticks % 864000000000 / 500000 * 5;
            double result = temp / 100.0;
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(result);
                case VarType.Sng:
                    return Convert.ToSingle(result);
                case VarType.Dbl:
                    return result;
            }
            return null;
        }
        public static object USR(string str, VarType vartype) {
            // USR
            // 사용자 지정 기계어 코드? 기계어까지 구현하고 싶지는 않아
            Console.WriteLine("Not support on this version");
            return null;
        }
        public static object VAL(string str, VarType vartype) {
            // VAL(문자열)
            // 해당 문자열에서 수치형을 가져온다.
            if (vartype == VarType.Str)
                throw new TypeMismatchException();
            str = str.Remove(str.LastIndexOf(')'));
            str = str.Remove(0, 3);
            str = Utility.RemoveBlank(str);
            str = str.Remove(0, 1);
            switch (vartype) {
                case VarType.Int:
                    return Convert.ToInt32(Utility.GetValue(str.Replace(" ", ""), "", VarType.Int));
                case VarType.Sng:
                    return Convert.ToSingle(Utility.GetValue(str.Replace(" ", ""), "", VarType.Sng));
                case VarType.Dbl:
                    return Convert.ToDouble(Utility.GetValue(str.Replace(" ", ""), "", VarType.Dbl));
            }
            return null;
        }
        public static object VARPTR(string str, VarType vartype) {
            // VARPTR(X)
            // 해당 변수의 주소를 출력한다. 만약 $이라면 그 주소를 문자열로 출력한다.
            // 하지만 클래스 멤버는 주소를 만들 수 없다고 하네.
            Console.WriteLine("Not support on this version");
            return null;
        }
    }
}