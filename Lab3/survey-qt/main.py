import sys
from PyQt5.QtWidgets import (
    QApplication, QWidget, QLabel, QTextEdit,
    QPushButton, QVBoxLayout, QHBoxLayout,
    QFileDialog, QMessageBox, QFrame
)
from PyQt5.QtCore import Qt
from PyQt5.QtGui import QFont


QUESTIONS = [
    "Як вас звати?",
    "Скільки вам років?",
    "Яка ваша спеціальність?",
    "Яке ваше хобі?",
    "Що вам найбільше подобається у навчанні?"
]


class SurveyApp(QWidget):
    def __init__(self):
        super().__init__()
        self.current = 0
        self.answers = [""] * len(QUESTIONS)
        self.init_ui()
        self.update_view()

    def init_ui(self):
        self.setWindowTitle("Форма опитування")
        self.setFixedSize(520, 320)
        self.setStyleSheet("""
            QWidget {
                background-color: #ffffff;
                font-family: Segoe UI;
            }
            QLabel#title {
                font-size: 16px;
                font-weight: bold;
                color: #111111;
            }
            QLabel#progress {
                font-size: 11px;
                color: #999999;
            }
            QLabel#question {
                font-size: 13px;
                color: #222222;
            }
            QTextEdit {
                border: 1px solid #cccccc;
                font-size: 13px;
                padding: 6px;
                background-color: #fafafa;
            }
            QTextEdit:focus {
                border: 1px solid #555555;
            }
            QPushButton {
                font-size: 12px;
                padding: 7px 20px;
                border: 1px solid #cccccc;
                background-color: #ffffff;
            }
            QPushButton:hover {
                background-color: #f0f0f0;
            }
            QPushButton:disabled {
                color: #cccccc;
                border-color: #e0e0e0;
            }
            QPushButton#btnNext {
                background-color: #222222;
                color: #ffffff;
                border-color: #222222;
            }
            QPushButton#btnNext:hover {
                background-color: #444444;
            }
            QFrame#separator {
                color: #eeeeee;
            }
        """)

        main_layout = QVBoxLayout()
        main_layout.setContentsMargins(36, 28, 36, 24)
        main_layout.setSpacing(0)

        self.label_title = QLabel("Форма опитування")
        self.label_title.setObjectName("title")
        main_layout.addWidget(self.label_title)

        separator = QFrame()
        separator.setObjectName("separator")
        separator.setFrameShape(QFrame.HLine)
        separator.setFixedHeight(1)
        separator.setStyleSheet("background-color: #eeeeee; margin-top: 8px; margin-bottom: 12px;")
        main_layout.addWidget(separator)

        self.label_progress = QLabel()
        self.label_progress.setObjectName("progress")
        self.label_progress.setAlignment(Qt.AlignRight)
        main_layout.addWidget(self.label_progress)

        main_layout.addSpacing(6)

        self.label_question = QLabel()
        self.label_question.setObjectName("question")
        self.label_question.setWordWrap(True)
        main_layout.addWidget(self.label_question)

        main_layout.addSpacing(10)

        self.text_answer = QTextEdit()
        self.text_answer.setFixedHeight(90)
        main_layout.addWidget(self.text_answer)

        main_layout.addSpacing(14)

        btn_layout = QHBoxLayout()
        btn_layout.setSpacing(10)

        self.btn_prev = QPushButton("Назад")
        self.btn_prev.setObjectName("btnPrev")
        self.btn_prev.clicked.connect(self.go_prev)

        self.btn_next = QPushButton("Далі")
        self.btn_next.setObjectName("btnNext")
        self.btn_next.clicked.connect(self.go_next)

        btn_layout.addWidget(self.btn_prev)
        btn_layout.addStretch()
        btn_layout.addWidget(self.btn_next)

        main_layout.addLayout(btn_layout)
        self.setLayout(main_layout)

    def update_view(self):
        self.label_progress.setText(f"Питання {self.current + 1} з {len(QUESTIONS)}")
        self.label_question.setText(QUESTIONS[self.current])
        self.text_answer.setPlainText(self.answers[self.current])
        self.text_answer.setFocus()
        self.btn_prev.setEnabled(self.current > 0)
        self.btn_next.setText("Завершити" if self.current == len(QUESTIONS) - 1 else "Далі")

    def go_next(self):
        answer = self.text_answer.toPlainText().strip()
        if not answer:
            self.text_answer.setStyleSheet("border: 1px solid #cc0000; font-size: 13px; padding: 6px;")
            return
        self.text_answer.setStyleSheet("")
        self.answers[self.current] = answer

        if self.current == len(QUESTIONS) - 1:
            self.save_results()
        else:
            self.current += 1
            self.update_view()

    def go_prev(self):
        self.answers[self.current] = self.text_answer.toPlainText().strip()
        self.current -= 1
        self.update_view()

    def save_results(self):
        path, _ = QFileDialog.getSaveFileName(
            self, "Зберегти результати", "results.txt", "Text Files (*.txt)"
        )
        if not path:
            return

        with open(path, "w", encoding="utf-8") as f:
            for i, question in enumerate(QUESTIONS):
                f.write(f"Питання {i + 1}: {question}\n")
                f.write(f"Відповідь: {self.answers[i]}\n\n")

        QMessageBox.information(self, "Збережено", f"Результати збережено у файл:\n{path}")
        QApplication.quit()


def main():
    app = QApplication(sys.argv)
    window = SurveyApp()
    window.show()
    sys.exit(app.exec_())


if __name__ == "__main__":
    main()
