using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculadora
{

    public partial class MainWindow : Window
    {
        Stack<char> userActions = new Stack<char>();
        private double firstNumber { get; set; }
        private double secondNumber { get; set; }
        private string operando { get; set; }
        private int nextNumberIndex { get; set; }
        string[] operadores = { "+", "-", "/", "X", "÷", "Xⁿ" , ".", "√", "MOD"};
        public double result;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button button)
            {
                
                string conteudoDoBotao = button.Content?.ToString();
                DigitarNoDisplay(conteudoDoBotao);

            }

        }

        public bool VerifyContent(string simbol)
        {
            if (string.IsNullOrEmpty(tbDisplay.Text))
            {
                if (simbol == "-")
                {
                    tbDisplay.Text += "-";
                    return false;
                }
                else if(string.IsNullOrEmpty(tbDisplay.Text) && operadores.Contains(simbol))
                {
                    return false;
                }
                return true;
            }

            char lastChar = tbDisplay.Text.Last();
            return !(lastChar == '.' && operadores.Contains(simbol) ||
                     (lastChar == '.' && simbol == "Calc") ||
                     (new[] { '-', '+', 'X', ' ', '÷', 'ⁿ' }.Contains(lastChar) && (simbol == "Calc" || lastChar == 'ⁿ')) ||
                     (lastChar == ' ' && simbol == "-") || (lastChar == ' ' && simbol == "√"));
        }

        public void DigitarNoDisplay(string content)
        {
            if (!VerifyContent(content)) return;

            switch (content)
            {
                case "+":
                case "-":
                case "X":
                case "÷":
                case "Xⁿ":
                case "MOD":
                    createFirstNumber(content);
                    break;

                case "Calc":
                    
                    if(nextNumberIndex < tbDisplay.Text.Length)
                    {
                        
                        secondNumber = double.Parse(tbDisplay.Text.Substring(nextNumberIndex).Replace(" ", ""), CultureInfo.InvariantCulture);
                        ChooseOperand();
                        showResult();
                    }
                    break;

                case "Sair":
                    Application.Current.Shutdown();
                    break;

                case "DEL":
                    del();
                    break;

                case "C":
                    Erase();
                    break;

                case "√":
                    sqrt();
                    break;
                case "Abs":
                    abs();
                    break;

                default:
                    tbDisplay.Text += content;
                    break;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            string digito = ExtractDigitFromKey(e.Key);

            if (!string.IsNullOrEmpty(digito))
            {
                DigitarNoDisplay(digito);
            }
        }

        private string ExtractDigitFromKey(Key key)
        {
            switch (key)
            {


                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    return (key - Key.NumPad0).ToString();

                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                    return (key - Key.D0).ToString();

                // Adicionando suporte para sinais
                case Key.Divide:
                    return "÷";
                case Key.Multiply:
                    return "X";
                case Key.Add:
                    return "+";
                case Key.Subtract:
                    return "-";
                case Key.X:
                    return "X";
                case Key.P:
                    return "Xⁿ";
                case Key.OemPeriod:
                    return ".";
                case Key.M:
                    return "MOD";
                case Key.C:
                    return "C";
                case Key.A:
                    return "Abs";
                case Key.Back:
                    return "DEL";
                case Key.Enter:
                    return "Calc";
                case Key.Escape:
                    return "Sair";
                case Key.Space:
                    return string.Empty;

                default:
                    return string.Empty;
            }
        }

        private void createFirstNumber(string content)
        {
            
            string input = tbDisplay.Text;
            if (double.TryParse(input, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out double parsedNumber))
            {
                firstNumber = parsedNumber;
                operando = content;
                tbDisplay.Text += $" {content} ";
                nextNumberIndex = tbDisplay.Text.Length - 1;
            }
        }

        private void Add()
        {
            result = firstNumber + secondNumber;
        }

        private void Subtract()
        {
            result = firstNumber - secondNumber;
        }

        private void Multiply()
        {
            result = firstNumber * secondNumber;
        }

        private void Divide()
        {
            result = firstNumber / secondNumber;
        }

        private void Pow()
        {
            result = Math.Pow(firstNumber, secondNumber);
        }

        private void Mod()
        {
            result = firstNumber % secondNumber;
        }

        private void sqrt()
        {
            string input = tbDisplay.Text.Trim();
            if (double.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double parsedNumber))
            {
                result = Math.Sqrt(parsedNumber); showResult();
            }
            else
            {
                tbDisplay.Text = "";
                MessageBox.Show("Entrada invalida");
            }
        }

        private void abs()
        {
            string input = tbDisplay.Text.Trim();
            if (double.TryParse(input, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out double parsedNumber))
            {
                result = Math.Abs(parsedNumber); showResult();
            }
            else
            {
                tbDisplay.Text = "";
                MessageBox.Show("Entrada invalida");
            }
        }

        private void Erase()
        {
            tbDisplay.Text = "";
            firstNumber = 0;
            secondNumber = 0;
            operando = "";
            nextNumberIndex = 0;
        }

        private void del()
        {
            tbDisplay.Text = tbDisplay.Text.Length > 0 ? tbDisplay.Text.Substring(0, tbDisplay.Text.Length - 1) : "";
        }

        private void showResult()
        {
            tbDisplay.Text = result.ToString("F2", CultureInfo.InvariantCulture).Trim();
        }

        private void ChooseOperand()
        {

            switch (operando)
            {
                case "+":
                    Add();
                    break;
                case "-":
                    Subtract();
                    break;
                case "÷":
                    Divide();
                    break;
                case "X":
                    Multiply();
                    break;
                case "Xⁿ":
                    Pow();
                    break;
                case "MOD":
                    Mod();
                    break;
                case "Abs":
                    abs();
                    break;

            }

        }
    }
}
