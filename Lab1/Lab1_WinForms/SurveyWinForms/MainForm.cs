namespace SurveyWinForms;

public class MainForm : Form
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

    private Panel _surveyPanel = null!;
    private Panel _resultPanel = null!;

    private Label _counterLabel = null!;
    private ProgressBar _progressBar = null!;
    private Label _questionLabel = null!;
    private TextBox _answerInput = null!;
    private Button _nextButton = null!;

    private FlowLayoutPanel _resultList = null!;

    public MainForm()
    {
        _answers = new string[_questions.Length];
        InitializeComponent();
        ShowSurvey();
    }

    private void InitializeComponent()
    {
        Text = "Опитування";
        Size = new Size(560, 520);
        MinimumSize = new Size(560, 520);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(245, 244, 241);
        Font = new Font("Segoe UI", 10f);

        _surveyPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40) };
        _resultPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40), Visible = false };

        BuildSurveyPanel();
        BuildResultPanel();

        Controls.Add(_resultPanel);
        Controls.Add(_surveyPanel);
    }

    private void BuildSurveyPanel()
    {
        _progressBar = new ProgressBar
        {
            Maximum = _questions.Length,
            Value = 1,
            Height = 4,
            Dock = DockStyle.Top,
            Style = ProgressBarStyle.Continuous,
            ForeColor = Color.FromArgb(30, 30, 30),
            BackColor = Color.FromArgb(220, 218, 213)
        };

        _counterLabel = new Label
        {
            Text = $"1 / {_questions.Length}",
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(140, 135, 128),
            Font = new Font("Segoe UI", 9f),
            Height = 28
        };

        _questionLabel = new Label
        {
            Text = _questions[0],
            Dock = DockStyle.Top,
            ForeColor = Color.FromArgb(20, 20, 20),
            Font = new Font("Georgia", 16f, FontStyle.Regular),
            AutoSize = false,
            Height = 80,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _answerInput = new TextBox
        {
            Multiline = true,
            Height = 110,
            Dock = DockStyle.Top,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(235, 233, 228),
            ForeColor = Color.FromArgb(20, 20, 20),
            Font = new Font("Segoe UI", 10f),
            ScrollBars = ScrollBars.Vertical,
            Padding = new Padding(8)
        };

        _nextButton = new Button
        {
            Text = "Далі",
            Width = 120,
            Height = 40,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f),
            Cursor = Cursors.Hand
        };
        _nextButton.FlatAppearance.BorderSize = 0;
        _nextButton.Click += NextButton_Click;

        var spacer = new Panel { Dock = DockStyle.Top, Height = 16 };
        var bottomPanel = new Panel { Dock = DockStyle.Bottom, Height = 52 };
        bottomPanel.Controls.Add(_nextButton);
        _nextButton.Location = new Point(bottomPanel.Width - 128, 6);
        bottomPanel.Resize += (_, _) => _nextButton.Location = new Point(bottomPanel.Width - 128, 6);

        _surveyPanel.Controls.Add(bottomPanel);
        _surveyPanel.Controls.Add(spacer);
        _surveyPanel.Controls.Add(_answerInput);
        _surveyPanel.Controls.Add(_questionLabel);
        _surveyPanel.Controls.Add(_counterLabel);
        _surveyPanel.Controls.Add(_progressBar);
    }

    private void BuildResultPanel()
    {
        var titleLabel = new Label
        {
            Text = "Опитування завершено",
            Dock = DockStyle.Top,
            Font = new Font("Georgia", 18f),
            ForeColor = Color.FromArgb(20, 20, 20),
            Height = 48,
            TextAlign = ContentAlignment.MiddleLeft
        };

        var subtitleLabel = new Label
        {
            Text = "Відповіді збережено у файл.",
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 9f),
            ForeColor = Color.FromArgb(140, 135, 128),
            Height = 24,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _resultList = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(0, 8, 0, 0)
        };

        var bottomPanel = new Panel { Dock = DockStyle.Bottom, Height = 52 };

        var downloadButton = new Button
        {
            Text = "Завантажити TXT",
            Width = 160,
            Height = 40,
            BackColor = Color.FromArgb(245, 244, 241),
            ForeColor = Color.FromArgb(20, 20, 20),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f),
            Cursor = Cursors.Hand
        };
        downloadButton.FlatAppearance.BorderColor = Color.FromArgb(200, 198, 193);
        downloadButton.Click += DownloadButton_Click;

        var restartButton = new Button
        {
            Text = "Пройти знову",
            Width = 140,
            Height = 40,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f),
            Cursor = Cursors.Hand
        };
        restartButton.FlatAppearance.BorderSize = 0;
        restartButton.Click += (_, _) =>
        {
            _currentIndex = 0;
            Array.Clear(_answers);
            ShowSurvey();
        };

        bottomPanel.Controls.Add(restartButton);
        bottomPanel.Controls.Add(downloadButton);

        void LayoutBottom(object? s, EventArgs e)
        {
            restartButton.Location = new Point(bottomPanel.Width - restartButton.Width, 6);
            downloadButton.Location = new Point(bottomPanel.Width - restartButton.Width - downloadButton.Width - 8, 6);
        }
        bottomPanel.Resize += LayoutBottom;

        _resultPanel.Controls.Add(bottomPanel);
        _resultPanel.Controls.Add(_resultList);
        _resultPanel.Controls.Add(subtitleLabel);
        _resultPanel.Controls.Add(titleLabel);
    }

    private void ShowSurvey()
    {
        _resultPanel.Visible = false;
        _surveyPanel.Visible = true;
        UpdateSurveyUI();
        _answerInput.Focus();
    }

    private void UpdateSurveyUI()
    {
        _counterLabel.Text = $"{_currentIndex + 1} / {_questions.Length}";
        _questionLabel.Text = _questions[_currentIndex];
        _answerInput.Text = _answers[_currentIndex] ?? "";
        _progressBar.Value = _currentIndex + 1;
        _nextButton.Text = _currentIndex == _questions.Length - 1 ? "Завершити" : "Далі";
    }

    private void NextButton_Click(object? sender, EventArgs e)
    {
        _answers[_currentIndex] = _answerInput.Text.Trim();

        if (_currentIndex + 1 < _questions.Length)
        {
            _currentIndex++;
            UpdateSurveyUI();
            _answerInput.Focus();
        }
        else
        {
            SaveResults();
            ShowResult();
        }
    }

    private void ShowResult()
    {
        _resultList.Controls.Clear();

        for (int i = 0; i < _questions.Length; i++)
        {
            var item = new Panel
            {
                Width = _resultList.ClientSize.Width - 4,
                AutoSize = true,
                Padding = new Padding(0, 8, 0, 8)
            };

            var q = new Label
            {
                Text = _questions[i],
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(140, 135, 128),
                AutoSize = false,
                Width = item.Width,
                Height = 20,
                Location = new Point(0, 8)
            };

            var a = new Label
            {
                Text = string.IsNullOrEmpty(_answers[i]) ? "—" : _answers[i],
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(20, 20, 20),
                AutoSize = true,
                MaximumSize = new Size(item.Width, 0),
                Location = new Point(0, 30)
            };

            item.Height = 38 + a.PreferredHeight + 8;
            item.Controls.Add(q);
            item.Controls.Add(a);
            _resultList.Controls.Add(item);
        }

        _surveyPanel.Visible = false;
        _resultPanel.Visible = true;
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

    private void DownloadButton_Click(object? sender, EventArgs e)
    {
        var source = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "survey_results.txt"
        );

        using var dialog = new SaveFileDialog
        {
            FileName = "survey_results.txt",
            Filter = "Текстові файли (*.txt)|*.txt"
        };

        if (dialog.ShowDialog() == DialogResult.OK && File.Exists(source))
            File.Copy(source, dialog.FileName, overwrite: true);
    }
}
