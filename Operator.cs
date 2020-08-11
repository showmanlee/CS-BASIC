// Operator.cs
// 연산자

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSBASIC {
    class Operator {
        public static object GetOps(string str, VarType type) {
            try {
                Utility.RemoveBlank(ref str);
                str = Utility.RemoveMultipleBlank(str);
                Dictionary<int, int> bracketIndex = Utility.BracketIndex(str);
                if (bracketIndex.Count != 0)
                    for (int i = bracketIndex.Values.Max(); i > 0; i--) {
                        int getset = -1;
                        Dictionary<string, string> change = new Dictionary<string, string>();
                        foreach (int j in bracketIndex.Keys) {
                            if (bracketIndex[j] == i) {
                                if (getset != -1) {
                                    if (!change.Keys.Contains(str.Substring(getset, j - getset + 1)))
                                        change.Add(str.Substring(getset, j - getset + 1), StartOps(str.Substring(getset + 1, j - getset - 1), type).ToString());
                                    getset = -1;
                                }
                                else
                                    getset = j;
                            }
                        }
                        foreach (string s in change.Keys)
                            str = str.Replace(s, change[s]);
                        bracketIndex = Utility.BracketIndex(str);
                    }
                return StartOps(str, type);
            }
            catch { return null; }
        }
        static object StartOps(string str, VarType type) {
            object a = new object(), b = new object();
            int pos;
            try {
                if (str.Contains(" IMP ")) {
                    pos = str.Split(new[] { " IMP " }, StringSplitOptions.RemoveEmptyEntries)[0].Length;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    b = Utility.GetValue(str.Substring(pos + 5), "", VarType.Dbl);
                    if (((double)a != -1 || (double)b == -1))
                        return -1;
                    else
                        return 0;
                }
                else if (str.Contains(" EQV ")) {
                    pos = str.Split(new[] { " EQV " }, StringSplitOptions.RemoveEmptyEntries)[0].Length;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    b = Utility.GetValue(str.Substring(pos + 5), "", VarType.Dbl);
                    if (((double)a == -1 && (double)b == -1) || ((double)a != -1 && (double)b != -1))
                        return -1;
                    else
                        return 0;
                }
                else if (str.Contains(" XOR ")) {
                    pos = str.Split(new[] { " XOR " }, StringSplitOptions.RemoveEmptyEntries)[0].Length;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    b = Utility.GetValue(str.Substring(pos + 5), "", VarType.Dbl);
                    if (((double)a != -1 && (double)b == -1) || ((double)a == -1 && (double)b != -1))
                        return -1;
                    else
                        return 0;
                }
                else if (str.Contains(" OR ")) {
                    pos = str.Split(new[] { " OR " }, StringSplitOptions.RemoveEmptyEntries)[0].Length;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    b = Utility.GetValue(str.Substring(pos + 4), "", VarType.Dbl);
                    if ((double)a == -1 || (double)b == -1)
                        return -1;
                    else
                        return 0;
                }
                else if (str.Contains(" AND ")) {
                    pos = str.Split(new[] { " AND " }, StringSplitOptions.RemoveEmptyEntries)[0].Length;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    b = Utility.GetValue(str.Substring(pos + 5), "", VarType.Dbl);
                    if ((double)a == -1 && (double)b == -1)
                        return -1;
                    else
                        return 0;
                }
                else if (str.Contains("=") || str.Contains("<>") || str.Contains("><") || str.Contains("<=") || str.Contains("=<") || str.Contains(">=") || str.Contains("=>") || str.Contains("<") || str.Contains(">")) {
                    // 두개 사이 띄어써도 적용됨
                    pos = str.Split(new[] { '=', '<', '>' }, StringSplitOptions.RemoveEmptyEntries)[0].Length;
                    string stra = Utility.RemoveBlank(Utility.RemoveBlank(str.Substring(pos)).Substring(1));
                    VarType now = VarType.Dbl;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    if (a == null) {
                        a = Utility.GetValue(str.Substring(0, pos), "", VarType.Str);
                        now = VarType.Str;
                    }
                    switch (str[pos]) {
                        case '=':
                            switch (stra[0]) {
                                case '<':
                                    if (now == VarType.Str) {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Str);
                                        if (string.Compare((string)a, (string)b) <= 0)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                    else {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Dbl);
                                        if ((double)a <= (double)b)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                case '>':
                                    if (now == VarType.Str) {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Str);
                                        if (string.Compare((string)a, (string)b) >= 0)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                    else {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Dbl);
                                        if ((double)a >= (double)b)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                default:
                                    if (now == VarType.Str) {
                                        b = Utility.GetValue(stra.Substring(0), "", VarType.Str);
                                        if (string.Compare((string)a, (string)b) == 0)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                    else {
                                        b = Utility.GetValue(stra.Substring(0), "", VarType.Dbl);
                                        if ((double)a == (double)b)
                                            return -1;
                                        else
                                            return 0;
                                    }
                            }
                        case '>':
                            switch (stra[0]) {
                                case '=':
                                    if (now == VarType.Str) {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Str);
                                        if (string.Compare((string)a, (string)b) >= 0)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                    else {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Dbl);
                                        if ((double)a >= (double)b)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                case '<':
                                    if (now == VarType.Str) {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Str);
                                        if (string.Compare((string)a, (string)b) != 0)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                    else {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Dbl);
                                        if ((double)a != (double)b)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                default:
                                    if (now == VarType.Str) {
                                        b = Utility.GetValue(stra.Substring(0), "", VarType.Str);
                                        if (string.Compare((string)a, (string)b) > 0)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                    else {
                                        b = Utility.GetValue(stra.Substring(0), "", VarType.Dbl);
                                        if ((double)a > (double)b)
                                            return -1;
                                        else
                                            return 0;
                                    }
                            }
                        case '<':
                            switch (stra[0]) {
                                case '=':
                                    if (now == VarType.Str) {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Str);
                                        if (string.Compare((string)a, (string)b) <= 0)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                    else {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Dbl);
                                        if ((double)a <= (double)b)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                case '>':
                                    if (now == VarType.Str) {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Str);
                                        if (string.Compare((string)a, (string)b) != 0)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                    else {
                                        b = Utility.GetValue(stra.Substring(1), "", VarType.Dbl);
                                        if ((double)a != (double)b)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                default:
                                    if (now == VarType.Str) {
                                        b = Utility.GetValue(stra.Substring(0), "", VarType.Str);
                                        if (string.Compare((string)a, (string)b) < 0)
                                            return -1;
                                        else
                                            return 0;
                                    }
                                    else {
                                        b = Utility.GetValue(stra.Substring(0), "", VarType.Dbl);
                                        if ((double)a < (double)b)
                                            return -1;
                                        else
                                            return 0;
                                    }
                            }
                    }
                }
                else if (str.Contains("+") || str.Contains("-")) {
                    if (type == VarType.Str) {
                        pos = str.Split('+')[0].Length;
                        a = Utility.GetValue(str.Substring(0, pos), "", VarType.Str);
                        b = Utility.GetValue(str.Substring(pos + 1), "", VarType.Str);
                        if (a == null || b == null)
                            throw new Exception();
                        return ((string)a + (string)b);
                    }
                    else {
                        if (str[0] == '-') // 처음이 음수
                        {
                            char[] bb = str.ToCharArray();
                            bb[0] = '§';
                            str = new String(bb);
                        }
                        pos = str.Split(new[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries)[0].Length;
                        if (pos == str.Length) // 아까 그 처리로 +-로 온 이유가 없어졌다면
                        {
                            a = Utility.GetValue(str, "", VarType.Dbl);
                            switch (type) {
                                case VarType.Int:
                                    return (Convert.ToInt32(a));
                                case VarType.Sng:
                                    return (Convert.ToSingle(a));
                                case VarType.Dbl:
                                    return (Convert.ToDouble(a));
                            }
                        }
                        foreach (string s in Process.OperList) // 다른 연산자 다음 음수 = 여기로 온 이유가 없어짐
                        {
                            if (pos - s.Length >= 0 && str.Substring(pos - s.Length, s.Length) == s) {
                                char[] bb = str.ToCharArray();
                                bb[pos] = '§';
                                str = new String(bb);
                                a = Utility.GetValue(str, "", VarType.Dbl);
                                switch (type) {
                                    case VarType.Int:
                                        return (Convert.ToInt32(a));
                                    case VarType.Sng:
                                        return (Convert.ToSingle(a));
                                    case VarType.Dbl:
                                        return (Convert.ToDouble(a));
                                }
                            }
                        }
                        if (str[pos + 1] == '-') // --, +-꼴로 겹침
                        {
                            char[] bb = str.ToCharArray();
                            bb[pos + 1] = '§';
                            str = new String(bb);
                        }
                        a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                        b = Utility.GetValue(str.Substring(pos + 1), "", VarType.Dbl);
                    }
                    switch (str[pos]) {
                        case '+':
                            switch (type) {
                                case VarType.Int:
                                    return (Convert.ToInt32(a) + Convert.ToInt32(b));
                                case VarType.Sng:
                                    return (Convert.ToSingle(a) + Convert.ToSingle(b));
                                case VarType.Dbl:
                                    return (Convert.ToDouble(a) + Convert.ToDouble(b));
                            }
                            break;
                        case '-':
                            switch (type) {
                                case VarType.Int:
                                    return (Convert.ToInt32(a) - Convert.ToInt32(b));
                                case VarType.Sng:
                                    return (Convert.ToSingle(a) - Convert.ToSingle(b));
                                case VarType.Dbl:
                                    return (Convert.ToDouble(a) - Convert.ToDouble(b));
                            }
                            break;
                    }
                }
                else if (str.Contains(" MOD ")) {
                    pos = str.Split(new[] { " MOD " }, StringSplitOptions.RemoveEmptyEntries)[0].Length;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    b = Utility.GetValue(str.Substring(pos + 5), "", VarType.Dbl);
                    if (type == VarType.Str)
                        throw new TypeMismatchException();
                    else
                        return Convert.ToDouble(Convert.ToInt32((Convert.ToDouble(a) % Convert.ToDouble(b))));
                }
                else if (str.Contains("\\")) {
                    pos = str.Split('\\')[0].Length;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    b = Utility.GetValue(str.Substring(pos + 1), "", VarType.Dbl);
                    if (type == VarType.Str)
                        throw new TypeMismatchException();
                    else
                        return Convert.ToInt32((Convert.ToDouble(a) / Convert.ToDouble(b)));
                }
                else if (str.Contains("/") || str.Contains("*")) {
                    pos = str.Split('/', '*')[0].Length;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    b = Utility.GetValue(str.Substring(pos + 1), "", VarType.Dbl);
                    switch (str[str.Split('/', '*')[0].Length]) {
                        case '/':
                            switch (type) {
                                case VarType.Int:
                                    return (Convert.ToInt32(a) / Convert.ToInt32(b));
                                case VarType.Sng:
                                    return (Convert.ToSingle(a) / Convert.ToSingle(b));
                                case VarType.Dbl:
                                    return (Convert.ToDouble(a) / Convert.ToDouble(b));
                                case VarType.Str:
                                    throw new TypeMismatchException();
                            }
                            break;
                        case '*':
                            switch (type) {
                                case VarType.Int:
                                    return (Convert.ToInt32(a) * Convert.ToInt32(b));
                                case VarType.Sng:
                                    return (Convert.ToSingle(a) * Convert.ToSingle(b));
                                case VarType.Dbl:
                                    return (Convert.ToDouble(a) * Convert.ToDouble(b));
                                case VarType.Str:
                                    throw new TypeMismatchException();
                            }
                            break;
                    }
                }
                else if (str.Contains("^")) {
                    pos = str.Split('^')[0].Length;
                    a = Utility.GetValue(str.Substring(0, pos), "", VarType.Dbl);
                    b = Utility.GetValue(str.Substring(pos + 1), "", VarType.Dbl);
                    if (a == null || b == null)
                        throw new TypeMismatchException();
                    switch (type) {
                        case VarType.Int:
                            return Convert.ToInt32(Math.Pow((double)a, (double)b));
                        case VarType.Sng:
                            return Convert.ToSingle(Math.Pow((double)a, (double)b));
                        case VarType.Dbl:
                            return Convert.ToDouble(Math.Pow((double)a, (double)b));
                        case VarType.Str:
                            throw new TypeMismatchException();
                    }
                }
                else if (str.Contains("§")) {
                    pos = str.Split('§')[0].Length;
                    a = Utility.GetValue(str.Substring(pos + 1), "", VarType.Dbl);
                    switch (type) {
                        case VarType.Int:
                            return Convert.ToInt32((double)a * -1);
                        case VarType.Sng:
                            return Convert.ToSingle((double)a * -1);
                        case VarType.Dbl:
                            return Convert.ToDouble((double)a * -1);
                        case VarType.Str:
                            throw new TypeMismatchException();
                    }
                }
                else if (str.Contains("NOT ")) {
                    pos = str.Split(new[] { "NOT " }, StringSplitOptions.None)[0].Length;
                    a = Utility.GetValue(str.Substring(pos + 4), "", VarType.Dbl);
                    return Convert.ToInt32(((double)a + 1) * -1);
                }
                else
                    switch (type) {
                        case VarType.Int:
                            return Convert.ToInt32(str);
                        case VarType.Sng:
                            return Convert.ToSingle(str);
                        case VarType.Dbl:
                            return Convert.ToDouble(str);
                        case VarType.Str:
                            return str;
                    }
            }
            catch (TypeMismatchException e) { return null; }
            catch (Exception e) { return null; }
            return null;
        }
    }
}
