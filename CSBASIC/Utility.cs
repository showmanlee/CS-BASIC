// Utility.cs
// 코딩 중 자주 쓰는 걸 여기다 넣자

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSBASIC {
    class Utility {
        public static void RemoveBlank(ref string s)//공백 지우기
        {
            if (s == "")
                return;
            // 맨 앞 공백 지우기
            int blanks = 0;
            foreach (char c in s)
                if (c == ' ')
                    blanks++;
                else
                    break;
            s = s.Remove(0, blanks);

            // 숫자간 공백 지우기
            string str = "";
            for (int i = 0; i <= s.Length - s.Replace("\"", "").Length; i++) {
                string temp = s.Split('\"')[i];
                int first = -1, second = -1;
                if (i % 2 == 0) {
                    for (int j = 0; j < temp.Length - 1; j++) {
                        if (Char.IsDigit(temp[j]) && Char.IsWhiteSpace(temp[j + 1]))
                            first = j + 1;
                        if (first != -1 && !Char.IsWhiteSpace(temp[j]) && !Char.IsDigit(temp[j]))
                            break;
                        if (first != -1 && Char.IsWhiteSpace(temp[j]) && Char.IsDigit(temp[j + 1])) {
                            second = j;
                            break;
                        }
                    }
                    if (first != -1 && second != -1)
                        temp = temp.Remove(first, second - first + 1);
                    str += temp;
                }
                else
                    str += "\"" + temp + "\"";
            }
            // 맨 마지막 공백 지우기
            blanks = 1;
            for (; ; blanks++)
                if (str[str.Length - blanks] != ' ')
                    break;
            str = str.Substring(0, str.Length - --blanks);
            s = str;
        }
        public static string RemoveBlank(string s)//반환형 버전
        {
            if (s == "")
                return s;
            // 맨 앞 공백 지우기
            int blanks = 0;
            foreach (char c in s)
                if (c == ' ')
                    blanks++;
                else
                    break;
            s = s.Remove(0, blanks);

            // 숫자간 공백 지우기
            string str = "";
            for (int i = 0; i <= s.Length - s.Replace("\"", "").Length; i++) {
                string temp = s.Split('\"')[i];
                int first = -1, second = -1;
                if (i % 2 == 0) {
                    for (int j = 0; j < temp.Length - 1; j++) {
                        if (Char.IsDigit(temp[j]) && Char.IsWhiteSpace(temp[j + 1]))
                            first = j + 1;
                        if (first != -1 && !Char.IsWhiteSpace(temp[j]) && !Char.IsDigit(temp[j]))
                            break;
                        if (first != -1 && Char.IsWhiteSpace(temp[j]) && Char.IsDigit(temp[j + 1])) {
                            second = j;
                            break;
                        }
                    }
                    if (first != -1 && second != -1)
                        temp = temp.Remove(first, second - first + 1);
                    str += temp;
                }
                else
                    str += "\"" + temp + "\"";
            }
            // 맨 마지막 공백 지우기
            blanks = 1;
            for (; ; blanks++)
                if (str[str.Length - blanks] != ' ')
                    break;
            str = str.Substring(0, str.Length - --blanks);
            return str;
        }
        public static string RemoveMultipleBlank(string s)//연속 공백 지우기
        {
            foreach (char c in s) {
                s = s.Replace("  ", " ");
                if (!s.Contains("  "))
                    break;
            }
            return s;
        }
        public static bool IsValidVarRule(string s)//변수 규칙에 맞는가?
        {
            if (!char.IsUpper(s[0]))
                return false;
            if (s.Length == 1)
                return true;
            foreach (char c in s.Substring(1, s.Length - 2))
                if (!char.IsLetterOrDigit(c) && c != '.')
                    return false;
            if (!char.IsLetterOrDigit(s.Last()) && s.Last() != '.' && s.Last() != '$' && s.Last() != '#' && s.Last() != '!' && s.Last() != '%')
                return false;
            return true;
        }
        public static object GetValue(string s, string varname, VarType type)//변수 출력값 받기
        {
            RemoveBlank(ref s);
            s += " ";
            object value = null;
            // 사용자 지정 함수에서 받기
            if (s.Substring(0, 2) == "FN")
                value = Functions.FN(s, type);
            // 함수에서 받기
            else if (Process.FuncList.Contains(s.Substring(0, s.IndexOfAny(" (".ToCharArray()))) || Process.TempList.Contains(s.Substring(0, s.IndexOfAny(" (".ToCharArray()))))
                value = Process.FindFunc(s, type);
            // 연산자 연산
            else if (IsOpsIn(s))
                return Operator.GetOps(s, type);
            if (value != null)
                return value;
            try {
                string var = RemoveBlank(s.Split(new[] { '(', '[' })[0]);
                // 변수에서 받기
                if (Input.Vars.Keys.Contains(var)) {
                    if (((type == VarType.Str) && (Input.Vars[var].Types != VarType.Str)) || ((type != VarType.Str) && (Input.Vars[var].Types == VarType.Str)))
                        throw new TypeMismatchException();
                    if (Input.Vars[var].ArrayNum != 0) {
                        List<int> target = new List<int>();
                        string arg = s.Split(new[] { '(', '[' })[1];
                        if (!arg.Contains(')') && !arg.Contains(']'))
                            throw new SystemException();
                        if (CharAmount(arg, ',') + 1 != Input.Vars[var].ArrayNum)
                            throw new SystemException();
                        arg = arg.Split(new[] { ')', ']' })[0];
                        for (int i = 0; i <= CharAmount(arg, ','); i++) {
                            target.Add(Convert.ToInt32(GetValue(arg.Split(',')[i], "", type)));
                            if (target[i] > Input.Vars[var].ArraySize[i])
                                throw new SystemException();
                        }
                        switch (Input.Vars[var].ArrayNum) {
                            case 1:
                                object[] no1 = (object[])Input.Vars[var].Value;
                                switch (type) {
                                    case VarType.Int:
                                        value = Convert.ToInt32(no1[target[0]]);
                                        break;
                                    case VarType.Sng:
                                        value = Convert.ToSingle(no1[target[0]]);
                                        break;
                                    case VarType.Dbl:
                                        value = Convert.ToDouble(no1[target[0]]);
                                        break;
                                    case VarType.Str:
                                        value = (no1[target[0]]).ToString();
                                        break;
                                }
                                break;
                            case 2:
                                object[,] no2 = (object[,])Input.Vars[var].Value;
                                switch (type) {
                                    case VarType.Int:
                                        value = Convert.ToInt32(no2[target[0], target[1]]);
                                        break;
                                    case VarType.Sng:
                                        value = Convert.ToSingle(no2[target[0], target[1]]);
                                        break;
                                    case VarType.Dbl:
                                        value = Convert.ToDouble(no2[target[0], target[1]]);
                                        break;
                                    case VarType.Str:
                                        value = (no2[target[0], target[1]]).ToString();
                                        break;
                                }
                                break;
                            case 3:
                                object[,,] no3 = (object[,,])Input.Vars[var].Value;
                                switch (type) {
                                    case VarType.Int:
                                        value = Convert.ToInt32(no3[target[0], target[1], target[2]]);
                                        break;
                                    case VarType.Sng:
                                        value = Convert.ToSingle(no3[target[0], target[1], target[2]]);
                                        break;
                                    case VarType.Dbl:
                                        value = Convert.ToDouble(no3[target[0], target[1], target[2]]);
                                        break;
                                    case VarType.Str:
                                        value = (no3[target[0], target[1], target[2]]).ToString();
                                        break;
                                }
                                break;
                            case 4:
                                object[,,,] no4 = (object[,,,])Input.Vars[var].Value;
                                switch (type) {
                                    case VarType.Int:
                                        value = Convert.ToInt32(no4[target[0], target[1], target[2], target[3]]);
                                        break;
                                    case VarType.Sng:
                                        value = Convert.ToSingle(no4[target[0], target[1], target[2], target[3]]);
                                        break;
                                    case VarType.Dbl:
                                        value = Convert.ToDouble(no4[target[0], target[1], target[2], target[3]]);
                                        break;
                                    case VarType.Str:
                                        value = (no4[target[0], target[1], target[2], target[3]]).ToString();
                                        break;
                                }
                                break;
                            case 5:
                                object[,,,,] no5 = (object[,,,,])Input.Vars[var].Value;
                                switch (type) {
                                    case VarType.Int:
                                        value = Convert.ToInt32(no5[target[0], target[1], target[2], target[3], target[4]]);
                                        break;
                                    case VarType.Sng:
                                        value = Convert.ToSingle(no5[target[0], target[1], target[2], target[3], target[4]]);
                                        break;
                                    case VarType.Dbl:
                                        value = Convert.ToDouble(no5[target[0], target[1], target[2], target[3], target[4]]);
                                        break;
                                    case VarType.Str:
                                        value = (no5[target[0], target[1], target[2], target[3], target[4]]).ToString();
                                        break;
                                }
                                break;
                            default:
                                throw new SystemException();
                        }
                    }
                    else
                        switch (type) {
                            case VarType.Int:
                                value = Convert.ToInt32(Input.Vars[s.Substring(0, s.Length - 1)].Value);
                                break;
                            case VarType.Sng:
                                value = Convert.ToSingle(Input.Vars[s.Substring(0, s.Length - 1)].Value);
                                break;
                            case VarType.Dbl:
                                value = Convert.ToDouble(Input.Vars[s.Substring(0, s.Length - 1)].Value);
                                break;
                            case VarType.Str:
                                value = Input.Vars[s.Substring(0, s.Length - 1)].Value.ToString();
                                break;
                        }
                }
                // 8진수/16진수 받기
                else if (s[0] == '&') {
                    if (s[1] == 'H')
                        value = (Convert.ToInt32(s.Substring(2), 1));
                    else if (s[1] == 'O')
                        value = (Convert.ToInt32(s.Substring(2), 8));
                    else if (char.IsDigit(s[1]))
                        value = (Convert.ToInt32(s.Substring(1), 8));
                    else
                        throw new TypeMismatchException();
                }
                // 텍스트에서 새로 받기
                else {
                    s = s.Substring(0, s.Length - 1);
                    switch ((Input.Vars.ContainsKey(varname)) ? Input.Vars[varname].Types : type) {
                        case (VarType.Str):
                            if (s[0] == '\"')
                                value = RemoveQuotemark(s);
                            else
                                throw new TypeMismatchException();
                            break;
                        case VarType.Int:
                            value = int.Parse(s);
                            break;
                        case VarType.Sng:
                            value = float.Parse(s);
                            break;
                        case VarType.Dbl:
                            value = double.Parse(s);
                            break;
                    }
                }
                return value;
            }
            catch { return null; }
        }
        public static object ArrayInitialize(int dim, VarType type)//배열 초기화
        {
            object init = new object();
            switch (type) {
                case VarType.Str:
                    init = "";
                    break;
                case VarType.Int:
                    init = 0;
                    break;
                case VarType.Sng:
                    init = 0.0f;
                    break;
                case VarType.Dbl:
                    init = 0.0d;
                    break;
            }
            switch (dim) {
                case 1: {
                        object[] array = new object[11];
                        for (int i = 0; i < 11; i++)
                            array[i] = init;
                        return array;
                    }
                case 2: {
                        object[,] array = new object[11, 11];
                        for (int i = 0; i < 11; i++)
                            for (int j = 0; j < 11; j++)
                                array[i, j] = init;
                        return array;
                    }
                case 3: {
                        object[,,] array = new object[11, 11, 11];
                        for (int i = 0; i < 11; i++)
                            for (int j = 0; j < 11; j++)
                                for (int k = 0; k < 11; k++)
                                    array[i, j, k] = init;
                        return array;
                    }
                case 4: {
                        object[,,,] array = new object[11, 11, 11, 11];
                        for (int i = 0; i < 11; i++)
                            for (int j = 0; j < 11; j++)
                                for (int k = 0; k < 11; k++)
                                    for (int l = 0; l < 11; l++)
                                        array[i, j, k, l] = init;
                        return array;
                    }
                case 5: {
                        object[,,,,] array = new object[11, 11, 11, 11, 11];
                        for (int i = 0; i < 11; i++)
                            for (int j = 0; j < 11; j++)
                                for (int k = 0; k < 11; k++)
                                    for (int l = 0; l < 11; l++)
                                        for (int m = 0; m < 11; m++)
                                            array[i, j, k, l, m] = init;
                        return array;
                    }
                default:
                    return null;
            }
        }
        public static object ArrayInitialize(int dim, VarType type, int[] size)//크기 받는 버전
        {
            object init = new object();
            switch (type) {
                case VarType.Str:
                    init = "";
                    break;
                case VarType.Int:
                    init = 0;
                    break;
                case VarType.Sng:
                    init = 0.0f;
                    break;
                case VarType.Dbl:
                    init = 0.0d;
                    break;
            }
            switch (dim) {
                case 1: {
                        object[] array = new object[size[0]];
                        for (int i = 0; i < size[0]; i++)
                            array[i] = init;
                        return array;
                    }
                case 2: {
                        object[,] array = new object[size[0], size[1]];
                        for (int i = 0; i < size[0]; i++)
                            for (int j = 0; j < size[1]; j++)
                                array[i, j] = init;
                        return array;
                    }
                case 3: {
                        object[,,] array = new object[size[0], size[1], size[2]];
                        for (int i = 0; i < size[0]; i++)
                            for (int j = 0; j < size[1]; j++)
                                for (int k = 0; k < size[2]; k++)
                                    array[i, j, k] = init;
                        return array;
                    }
                case 4: {
                        object[,,,] array = new object[size[0], size[1], size[2], size[3]];
                        for (int i = 0; i < size[0]; i++)
                            for (int j = 0; j < size[1]; j++)
                                for (int k = 0; k < size[2]; k++)
                                    for (int l = 0; l < size[3]; l++)
                                        array[i, j, k, l] = init;
                        return array;
                    }
                case 5: {
                        object[,,,,] array = new object[size[0], size[1], size[2], size[3], size[4]];
                        for (int i = 0; i < 11; i++)
                            for (int j = 0; j < 11; j++)
                                for (int k = 0; k < 11; k++)
                                    for (int l = 0; l < 11; l++)
                                        for (int m = 0; m < 11; m++)
                                            array[i, j, k, l, m] = init;
                        return array;
                    }
                default:
                    return null;
            }
        }
        public static VarType WhatType(object obj)//타입 알아내기
        {
            if (obj.GetType() == 0.GetType())
                return VarType.Int;
            else if (obj.GetType() == 0.0f.GetType())
                return VarType.Sng;
            else if (obj.GetType() == 0.0d.GetType())
                return VarType.Dbl;
            else
                return VarType.Str;
        }
        public static string RemoveQuotemark(string s)//따옴표 제거
        {
            s = s.Remove(s.IndexOf('\"'), 1);
            s = s.Remove(s.IndexOf('\"'));
            return s;
        }
        public static bool IsOpsIn(string s) //연산자 있나 확인
        {
            if (s[0] == '-')
                return IsOpsIn(s.Substring(1));
            foreach (string o in Process.OperList)
                for (int i = 0; i <= s.Length - s.Replace("\"", "").Length; i += 2)
                    if (s.Split('\"')[i].Contains(o))
                        return true;
            return false;
        }
        public static bool IsInQuote(string s, char c, out int n) // 해당 문자가 문자열 안에 있나 확인
        {
            n = -1;
            if (c == '\"')
                return false;
            bool quote = false;
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == '\"')
                    quote = !quote;
                if (s[i] == c) {
                    n = i;
                    return quote;
                }
            }
            return false; // 효과없음
        }
        public static Dictionary<int, int> BracketIndex(string s) // 중첩 괄호 체크, 그 과정에서 문자열 내부/함수/문/배열에서 쓰는 괄호 걷어내기
        {
            Dictionary<int, int> index = new Dictionary<int, int>();
            int k = 0;
            int level = 0;
            for (int i = 0; i < s.Length; i++) {
                string c;
                if (!IsInQuote(s, s[i], out int x)) {
                    if (i == 0)
                        c = "";
                    else
                        c = s.Substring(0, i);
                    // 글자 뒤 오는 괄호는 쓸모가 없다. 오히려 바라는 건 연산자 다음 괄호.
                    if (s[i] == '(') {
                        if (i == 0)
                            index.Add(i, ++level);
                        else
                            for (int j = 1; j <= 5; j++)
                                if (Process.OperList.Contains(s.Substring(c.Length - j, j))) {
                                    index.Add(i, ++level); break;
                                }
                        if (index.Keys.Contains(i))
                            k++;
                    }
                    else if (s[i] == ')') {
                        if (k == 1 || index.ContainsKey(0))
                            index.Add(i, level--);
                        else
                            for (int j = 1; j <= 5; j++)
                                if (Process.OperList.Contains(s.Substring(i + 1, j))) {
                                    index.Add(i, level--); break;
                                }
                        if (index.Keys.Contains(i))
                            k--;
                    }
                }
            }
            if (k == 0)
                return index;
            else
                throw new Exception();
        }
        public static int CharAmount(string s, char c) // 문자열의 문자 개수 체크
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }
    }
}