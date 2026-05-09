using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace SurveyWPF;

public partial class MainWindow : Window
{
    private readonly string[] _questions = [
        "Як вас звати?",
        "Скільки вам років?",
        "Яка ваша спеціальність або сфера діяльності?",
        "Що вам подобається найбільше у вашій роботі або навчанні?",
        "Які ваші плани на найближчий рік?"
    ];

    private readonly string[] _answers;
    private int _currentIndex = 0;

    public MainWindow()
    {
        InitializeComponent();
        _answers = new string[_questions.Length];
        ProgressBar.Maximum = _questions.Length;
        ShowSurvey();
    }

    private void ShowSurvey()
    {
        ResultPanel.Visibility = Visibility.Collapsed;
        SurveyPanel.Visibility = Visibility.Visible;
        UpdateSurveyUI();
        AnswerInput.Focus();
    }

    private void UpdateSurveyUI()
    {
        CounterLabel.Text = $"{_currentIndex + 1} / {_questions.Length}";
        QuestionLabelSmall.Text = $"ПИТАННЯ {_currentIndex + 1}";
        QuestionLabel.Text = _questions[_currentIndex];
        AnswerInput.Text = _answers[_currentIndex] ?? "";
        ProgressBar.Value = _currentIndex + 1;
        NextButton.Content = _currentIndex == _questions.Length - 1 ? "Завершити" : "Далі";
    }

    private void Proceed()
    {
        _answers[_currentIndex] = AnswerInput.Text.Trim();

        if (_currentIndex + 1 < _questions.Length)
        {
            _currentIndex++;
            UpdateSurveyUI();
            AnswerInput.Focus();
        }
        else
        {
            SaveResults();
            ShowResult();
        }
    }

    private void NextButton_Click(object sender, RoutedEventArgs e) => Proceed();

    private void AnswerInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && Keyboard.Modifiers != ModifierKeys.Shift)
        {
            e.Handled = true;
            Proceed();
        }
    }

    private void ShowResult()
    {
        ResultList.Children.Clear();

        for (int i = 0; i < _questions.Length; i++)
        {
            var border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(220, 218, 213)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(0, 12, 0, 12)
            };

            var stack = new StackPanel();

            stack.Children.Add(new TextBlock
            {
                Text = _questions[i],
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 10,
                Foreground = new SolidColorBrush(Color.FromRgb(138, 125, 106)),
                Margin = new Thickness(0, 0, 0, 4)
            });

            stack.Children.Add(new TextBlock
            {
                Text = string.IsNullOrEmpty(_answers[i]) ? "—" : _answers[i],
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13,
                Foreground = new SolidColorBrush(Color.FromRgb(20, 20, 20)),
                TextWrapping = TextWrapping.Wrap
            });

            border.Child = stack;
            ResultList.Children.Add(border);
        }

        SurveyPanel.Visibility = Visibility.Collapsed;
        ResultPanel.Visibility = Visibility.Visible;
    }

    private void SaveResults()
    {
        var path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "survey_results.txt"
        );

        var lines = new List<string>
        {
            $"Дата: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
            new string('-', 40)
        };

        for (int i = 0; i < _questions.Length; i++)
        {
            lines.Add(_questions[i]);
            lines.Add(_answers[i] ?? "");
            lines.Add("");
        }

        lines.Add(new string('=', 40));
        lines.Add("");

        File.AppendAllLines(path, lines);
    }

    private void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        var source = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "survey_results.txt"
        );

        var dialog = new SaveFileDialog
        {
            FileName = "survey_results.txt",
            Filter = "Текстові файли (*.txt)|*.txt"
        };

        if (dialog.ShowDialog() == true && File.Exists(source))
            File.Copy(source, dialog.FileName, overwrite: true);
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        _currentIndex = 0;
        Array.Clear(_answers);
        ShowSurvey();
    }
}
