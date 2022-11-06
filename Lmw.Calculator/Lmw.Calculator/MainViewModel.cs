using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Lmw.Calculator
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _value = "0";
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        DataTable dt = new DataTable();
        private List<string> strList = new List<string>();
        private List<string> stepList = new List<string>();
        private string exp = string.Empty;
        private StringBuilder builder = new StringBuilder();

        string reStr = "";
        private bool isFinish = false;
        private bool isEffective = true;

        public ICommand BtnCommand
        {
            get => new RelayCommand<string>((param) =>
            {

                decimal restult = 0;
                var re = decimal.TryParse(param, out restult);
                if (re == true)
                {
                    if (isEffective == false)
                        return;

                    if (isFinish == true)
                        strList.Clear();
                    var last = strList.LastOrDefault();
                    if (strList.Count == 1 || decimal.TryParse(last, out restult) == true || (last != null && last.Contains(".") && last.LastIndexOf('.') == last.Length - 1))
                    {
                        strList[strList.Count - 1] = last + param;
                    }
                    else
                    {
                        strList.Add(param);
                        isFinish = false;
                    }
                }
                else
                {
                    var last = strList.LastOrDefault();
                    re = decimal.TryParse(last, out restult);
                    if (re == false && param != "." && (last != null && !last.Contains("(")))
                        return;

                    switch (param)
                    {
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                            {
                                if (!strList.Any())
                                {
                                    strList.Add("0");
                                }
                                strList.Add(param);
                                isFinish = false;
                                isEffective = true;
                                break;
                            }
                        case "=":
                            {
                                var expression = string.Join("", strList);
                                Value = dt.Compute(expression, null).ToString();
                                strList.Clear();
                                strList.Add(Value);
                                isFinish = true;
                                isEffective = true;
                                return;
                            }
                        case "%":
                            {
                                if (!strList.Any())
                                    return;

                                if (strList.Count == 1)
                                {
                                    reStr = strList[0];
                                    if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                    {
                                        reStr += "0";
                                    }
                                    reStr = (decimal.Parse(reStr) / 100).ToString();
                                    //reStr = dt.Compute(reStr, null).ToString();
                                    Value = reStr;
                                    strList.Clear();
                                    strList.Add(Value);
                                    isFinish = true;
                                    isEffective = true;
                                    return;
                                }

                                if (isEffective == false) return;

                                reStr = strList.LastOrDefault();
                                if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                {
                                    reStr += "0";
                                }
                                reStr = (decimal.Parse(reStr) / 100).ToString();
                                strList[strList.Count - 1] = reStr;
                                isEffective = false;
                                break;
                            }
                        case "1/x":
                            {
                                if (!strList.Any())
                                {
                                    strList.Add("0");
                                }

                                if (strList.Count == 1)
                                {
                                    reStr = strList[0];
                                    if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                    {
                                        reStr += "0";
                                    }
                                    if (decimal.TryParse(reStr, out restult) && restult == 0)
                                        reStr = "0";
                                    reStr = "1/" + reStr;

                                    reStr = dt.Compute(reStr, null).ToString();
                                    Value = reStr;
                                    strList.Clear();
                                    strList.Add(Value);
                                    isFinish = true;
                                    isEffective = true;
                                    return;
                                }
                                if (isEffective == false) return;

                                reStr = strList.LastOrDefault() ;

                                if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                {
                                    reStr += "0";
                                }
                                reStr = "1/" + reStr;
                                reStr = dt.Compute(reStr, null).ToString();
                                strList[strList.Count - 1] = reStr;
                                isEffective = false;
                                break;

                            }
                        case "x²":
                            {
                                if (!strList.Any())
                                    return;

                                if (strList.Count == 1)
                                {
                                    reStr = strList[0];
                                    if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                    {
                                        reStr += "0";
                                    }
                                    reStr = reStr + "*" + reStr;
                                    reStr = dt.Compute(reStr, null).ToString();
                                    Value = reStr;
                                    strList.Clear();
                                    strList.Add(Value);
                                    isFinish = true;
                                    isEffective = true;
                                    return;
                                }
                                if (isEffective == false) return;
                                reStr = strList.LastOrDefault();
                                if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                {
                                    reStr += "0";
                                }
                                reStr = reStr + "*" + reStr;
                                reStr = dt.Compute(reStr, null).ToString();
                                strList[strList.Count - 1] = reStr;
                                isEffective = false;
                                break;

                            }
                        case "√":
                            {
                                if (!strList.Any())
                                    return;
                                if (strList.Count == 1)
                                {
                                    reStr = strList[0];
                                    if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                    {
                                        reStr += "0";
                                    }
                                    var num1 = double.Parse(reStr);
                                    reStr = Math.Sqrt(num1).ToString();
                                    Value = reStr;
                                    strList.Clear();
                                    strList.Add(Value);
                                    isFinish = true;
                                    isEffective = true;
                                    return;
                                }
                                if (isEffective == false) return;
                                reStr = strList.LastOrDefault(); 
                                if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                {
                                    reStr += "0";
                                }
                                var num = double.Parse(reStr);
                                reStr = Math.Sqrt(num).ToString();
                                strList[strList.Count - 1] = reStr;
                                isEffective = false;
                                break;
                            }
                        case "n!":
                            {

                                if (!strList.Any())
                                {
                                    strList.Add("0");
                                }

                                if (strList.Count == 1)
                                {
                                    reStr = strList[0];
                                    if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                    {
                                        reStr += "0";
                                    }
                                    int temp = 0;
                                    if (!int.TryParse(reStr, out temp))
                                        return;
                                    reStr = myFatorFun(temp).ToString();

                                  
                                    Value = reStr;
                                    strList.Clear();
                                    strList.Add(Value);
                                    isFinish = true;
                                    isEffective = true;
                                    return;
                                }
                                if (isEffective == false) return;

                                reStr = strList.LastOrDefault();

                                if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                {
                                    reStr += "0";
                                }
                                int temp2 = 0;
                                if (!int.TryParse(reStr, out temp2))
                                    return;
                                reStr = myFatorFun(temp2).ToString();
                                strList[strList.Count - 1] = reStr;
                                isEffective = false;
                                break;
                            }
                        case ".":
                            {
                                if (isFinish == true || isEffective == false)
                                {
                                    strList.Clear();
                                    Value = "0.";
                                    strList.Add(Value);
                                    isFinish = false;
                                    isEffective = true;
                                    return;
                                }
                                if (!strList.Any())
                                {
                                    strList.Add("0");
                                }
                                if (strList.LastOrDefault().Contains('.'))
                                    return;
                                if (strList.Count == 1)
                                {
                                    var num1 = double.Parse(strList[0]);
                                    reStr = num1 + ".";
                                    Value = reStr;
                                    strList.Clear();
                                    strList.Add(Value);
                                    //isFinish = true;
                                    isEffective = true;
                                    return;
                                }

                                var num = double.Parse(strList.LastOrDefault());
                                reStr = num + ".";
                                strList[strList.Count - 1] = reStr;
                                break;
                            }
                        case "+/_":
                            {
                                if (!strList.Any())
                                    return;
                                if (strList.Count == 1)
                                {
                                    reStr = strList[0];
                                    if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                    {
                                        reStr += "0";
                                    }
                                    var num1 = double.Parse(reStr);
                                    reStr = (-num1).ToString();
                                    Value = reStr;
                                    strList.Clear();
                                    strList.Add(Value);
                                    isFinish = true;
                                    isEffective = true;
                                    return;
                                }

                                reStr = strList.LastOrDefault();
                                if (reStr.Contains(".") && reStr.LastIndexOf('.') == reStr.Length - 1)
                                {
                                    reStr += "0";
                                }

                                if (reStr.Contains("("))
                                {
                                    reStr = reStr.Replace("(", "");
                                    reStr = reStr.Replace(")", "");
                                }
                                var num = double.Parse(reStr);

                                reStr = num > 0 ? $"({(-num).ToString()})" : (-num).ToString();
                                strList[strList.Count - 1] = reStr;
                                isEffective = true;
                                break;
                            }
                    }
                }

                reStr = string.Join("", strList);
                if (reStr.Contains('*'))
                    reStr = reStr.Replace('*', '×');
                if (reStr.Contains('/'))
                    reStr = reStr.Replace('/', '÷');
                Value = reStr;

            });
        }

        public ICommand ClearCommand
        {
            get => new RelayCommand<string>((param) =>
            {
                Value = "0";
                strList.Clear();
                isFinish = true;
                isEffective = true;
            });
        }

        public ICommand RecantedCommand
        {
            get => new RelayCommand<string>((param) =>
            {
                isEffective = true;

                if (isFinish)
                    return;
                strList.RemoveAt(strList.Count - 1);

                if (!strList.Any())
                    isFinish = true;

                if (strList.Any())
                {
                    reStr = string.Join("", strList);
                    if (reStr.Contains('*'))
                        reStr = reStr.Replace('*', '×');
                    if (reStr.Contains('/'))
                        reStr = reStr.Replace('/', '÷');
                }
                else
                    reStr = "0";
                Value = reStr;

            });
        }


        private Int64 myFatorFun(int n)
        {
            if (n == 0)
                return 1;
            Int64 temp = 1;
            for (Int64 i = 1; i <= n; i++)
            {
                temp = temp * i;
            }
            return temp;
        }
    }
}
