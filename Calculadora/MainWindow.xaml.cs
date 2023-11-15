using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
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
        string[] operadores = { "+", "-", "/", "X", "÷", "Xⁿ" , ".", "√" };
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
                return true;
            }

            char lastChar = tbDisplay.Text.Last();
            return !(lastChar == '.' && operadores.Contains(simbol) ||
                     (lastChar == '.' && simbol == "Calc") ||
                     (new[] { '-', '+', 'X', ' ', '÷', 'ⁿ' }.Contains(lastChar) && (simbol == "Calc" || lastChar == 'ⁿ')) ||
                     (lastChar == ' ' && simbol == "-"));
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
                    firstNumber = double.Parse(tbDisplay.Text, CultureInfo.InvariantCulture);
                    operando = content;
                    tbDisplay.Text += $" {content} ";
                    nextNumberIndex = tbDisplay.Text.Length - 1;
                    break;

                case "Calc":
                    secondNumber = double.Parse(tbDisplay.Text.Substring(nextNumberIndex).Replace(" ", ""), CultureInfo.InvariantCulture);
                    ChooseOperand();
                    tbDisplay.Text = result.ToString("F2", CultureInfo.InvariantCulture);
                    break;

                case "Sair":
                    Application.Current.Shutdown();
                    break;

                case "DEL":
                    tbDisplay.Text = tbDisplay.Text.Length > 0 ? tbDisplay.Text.Substring(0, tbDisplay.Text.Length - 1) : "";
                    break;

                default:
                    tbDisplay.Text += content;
                    break;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

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
                case Key.Back:
                    return "DEL";
                case Key.Enter:
                    return "Calc";
                case Key.Escape:
                    return "Sair";

                default:
                    return string.Empty;
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

            }

        }
    }
}
