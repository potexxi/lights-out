using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LightsOut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<List<Button>> buttons;
        int moves = 0;
        int wins = 0;
        int allTimeMoves = 0;
        int size = 5;
        Brush ButtonColor = Brushes.Gray;
        public MainWindow()
        {
            InitializeComponent();
            ColorButtons();
        }

        private void CheckForWin()
        {
            int counter = 0;
            foreach (List<Button> buttonRow in buttons)
            {
                foreach(Button button in buttonRow)
                {
                    if (button.Background == ButtonColor)
                    {
                        counter += 1;
                    }
                }
            }
            if (counter == (buttons.Count * buttons[0].Count))
            {
                LabelWin.Content = "Gewonnen!";
                LabelWin.Background = Brushes.LightBlue;
                wins += 1;
                LabelWins.Content = $"Wins: {wins}";
            }
            else
            {
                LabelWin.Content = "";
            }
        }

        private void AddMoreButtons()
        {
            for (int i = 0; i < size; i++)
            {
                for (int t = 0; t < size; t++)
                {
                    Button button = new Button
                    {
                        Name = $"Button{i + 1}{t + 1}",
                        //Content = $"{i + 1}{t + 1}",
                        Background = Brushes.Red,
                        Width = 50,
                        Height = 50
                    };
                    CanvasPlay.Children.Add(button);
                    buttons[i].Add(button);
                    Canvas.SetLeft(button, 200 + (50 * t));
                    Canvas.SetTop(button, (50 * (i + 1)));
                    button.Click += Button_Click;
                }
            }
        }

        private void ColorButtons()
        {
            if (size > 4)
            {
                this.Width = 300 + size * 50;
                this.Height = 200 + size * 50;
            }
            else
            {
                this.Height = 400;
                this.Width = 550;
            }
            buttons = new List<List<Button>>();
            for (int i = 0; i < size; i++)
            {
                buttons.Add(new List<Button>());
            }
            AddMoreButtons();
            Random random = new Random();
            int randomNumber;
            foreach (List<Button> buttonRow in buttons)
            {
                foreach(Button button in buttonRow)
                {
                    randomNumber = random.Next(2);
                    if (randomNumber == 0)
                    {
                        button.Background = ButtonColor;
                    }
                    else
                    {
                        button.Background = Brushes.White;
                    }
                }
            }
            CheckForWin();
        }

        private void CheckForButtonColor()
        {
            if (buttons != null)
            {
                if (RadioButtonGray.IsChecked == true)
                    ButtonColor = Brushes.Gray;
                else if (RadioButtonRed.IsChecked == true)
                    ButtonColor = Brushes.Red;
                else if (RadioButtonGreen.IsChecked == true)
                    ButtonColor = Brushes.Green;
                else if (RadioButtonBlue.IsChecked == true)
                    ButtonColor = Brushes.Blue;
                foreach (List<Button> buttonRow in buttons)
                {
                    foreach (Button button in buttonRow)
                    {
                        if (button.Background != Brushes.White)
                        {
                            button.Background = ButtonColor;
                        }
                    }
                }
            }
            
        }

        private void ChangeButtonSelf(Button button)
        {
            if (LabelWin.Content == "")
            {
                if (button.Background == ButtonColor)
                {
                    button.Background = Brushes.White;
                }
                else
                {
                    button.Background = ButtonColor;
                }
            }
        }

        private void ChangeBorderButton(int row, int col)
        {
            moves += 1;
            allTimeMoves += 1;
            LabelMoves.Content = $"Moves: {moves}";
            LabelAllMoves.Content = $"All-Time-Moves: {allTimeMoves}";
            List<Button> borderButtons = new List<Button>();
            borderButtons.Add(buttons[row][col]);
            if (row > 0)
            {
                borderButtons.Add(buttons[row - 1][col]);
            }
            if (row < (size - 1))
            {
                borderButtons.Add(buttons[row + 1][col]);
            }
            if (col > 0)
            {
                borderButtons.Add(buttons[row][col - 1]);
            }
            if (col < (size - 1))
            {
                borderButtons.Add(buttons[row][col + 1]);
            }
            
            foreach (Button button in borderButtons)
            {
                ChangeButtonSelf(button);
            }
            CheckForWin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (LabelWin.Content == "")
                {
                    int lenght = button.Name.Length;
                    string col_string = button.Name.Substring(lenght - 1, 1);
                    string row_string = button.Name.Substring(lenght - 2, 1);

                    ChangeBorderButton(Convert.ToInt32(row_string) - 1, Convert.ToInt32(col_string) - 1);
                }
            }
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            LabelWin.Content = "";
            moves = 0;
            LabelMoves.Content = "Moves: 0";
            LabelWin.Background = null;
            ColorButtons();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            CheckForButtonColor();
        }

        private void RemoveButtons()
        {
            foreach(List<Button> buttonRow in buttons)
            {
                foreach(Button button in buttonRow)
                {
                    CanvasPlay.Children.Remove(button);
                }
            }
            buttons.Clear();
            TextBoxSize.Text = "";
            TextBoxSize.Background = Brushes.White;
            LabelWin.Content = "";
            moves = 0;
            LabelMoves.Content = "Moves: 0";
            LabelWin.Background = null;
            ColorButtons();
        }

        private void ButtonApplySize_Click(object sender, RoutedEventArgs e)
        {
            string entryString = TextBoxSize.Text;
            try
            {
                int entry = Convert.ToInt32(entryString);
                if ((entry > 0) && (entry < 10))
                {
                    size = entry;
                    RemoveButtons();
                    return;
                }
                MessageBox.Show("Gebe eine Zahl zwischen 1 und 9 ein!");
            }
            catch
            {
                MessageBox.Show("Gebe eine Zahl zwischen 1 und 9 ein!");
            }
        }

        private void TextBoxSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            string entryString = TextBoxSize.Text;
            try
            {
                int entry = Convert.ToInt32(entryString);
                if ((entry > 0) && (entry < 10))
                {
                    TextBoxSize.Background = Brushes.White;
                    return;
                }
                TextBoxSize.Background = Brushes.LightCoral;
            }
            catch
            {
                TextBoxSize.Background = Brushes.LightCoral;
            }
        }
    }
}