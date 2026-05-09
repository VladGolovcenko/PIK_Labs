var questions = [
  "Як вас звати?",
  "Скільки вам років?",
  "Яка ваша спеціальність або сфера діяльності?",
  "Що вам подобається найбільше у вашій роботі або навчанні?",
  "Які ваші плани на найближчий рік?"
];

var state = {
  currentIndex: 0,
  answers: []
};

var app = document.getElementById("app");

function render(html) {
  app.innerHTML = html;
}

function showStart() {
  state.currentIndex = 0;
  state.answers = [];

  render(
    '<div class="view card">' +
      '<p class="start-label">Лабораторна робота</p>' +
      '<h1 class="start-title">Форма<br>опитування</h1>' +
      '<p class="start-desc">Анонімне опитування з ' + questions.length + ' питань.<br>Результати зберігаються локально.</p>' +
      '<div class="start-footer">' +
        '<span class="start-meta">' + questions.length + ' питань</span>' +
        '<button class="btn btn-primary" id="btn-start">Розпочати</button>' +
      '</div>' +
    '</div>'
  );

  document.getElementById("btn-start").onclick = showSurvey;
}

function showSurvey() {
  var index = state.currentIndex;
  var progress = ((index + 1) / questions.length) * 100;
  var isLast = index === questions.length - 1;

  render(
    '<div class="view card">' +
      '<div class="progress-track">' +
        '<div class="progress-fill" style="width:' + progress + '%"></div>' +
      '</div>' +
      '<p class="counter">' + (index + 1) + ' / ' + questions.length + '</p>' +
      '<p class="question-label">Питання ' + (index + 1) + '</p>' +
      '<h2 class="question-text">' + questions[index] + '</h2>' +
      '<textarea class="answer-input" id="answer-input" placeholder="Ваша відповідь..." rows="4"></textarea>' +
      '<div class="row">' +
        '<button class="btn btn-primary" id="btn-next">' + (isLast ? "Завершити" : "Далі") + '</button>' +
      '</div>' +
    '</div>'
  );

  var input = document.getElementById("answer-input");
  var btn = document.getElementById("btn-next");

  input.value = state.answers[index] || "";
  input.focus();

  function proceed() {
    state.answers[state.currentIndex] = input.value.trim();
    if (state.currentIndex + 1 < questions.length) {
      state.currentIndex++;
      showSurvey();
    } else {
      showResult();
    }
  }

  btn.onclick = proceed;

  input.onkeydown = function(e) {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      proceed();
    }
  };
}

function showResult() {
  var record = {
    timestamp: new Date().toISOString(),
    answers: {}
  };

  for (var i = 0; i < questions.length; i++) {
    record.answers[questions[i]] = state.answers[i] || "";
  }

  var existing = JSON.parse(localStorage.getItem("survey_results") || "[]");
  existing.push(record);
  localStorage.setItem("survey_results", JSON.stringify(existing));

  var items = "";
  for (var i = 0; i < questions.length; i++) {
    items +=
      '<div class="result-item">' +
        '<p class="result-question">' + questions[i] + '</p>' +
        '<p class="result-answer">' + (state.answers[i] || "—") + '</p>' +
      '</div>';
  }

  render(
    '<div class="view card">' +
      '<h2 class="result-title">Готово</h2>' +
      '<p class="result-subtitle">Відповіді збережено локально.</p>' +
      '<div class="result-list">' + items + '</div>' +
      '<div class="result-footer">' +
        '<button class="btn btn-outline" id="btn-download">Завантажити JSON</button>' +
        '<button class="btn btn-primary" id="btn-restart">Пройти знову</button>' +
      '</div>' +
    '</div>'
  );

  document.getElementById("btn-restart").onclick = showStart;

  document.getElementById("btn-download").onclick = function() {
    var data = localStorage.getItem("survey_results") || "[]";
    var blob = new Blob([JSON.stringify(JSON.parse(data), null, 2)], { type: "application/json" });
    var url = URL.createObjectURL(blob);
    var a = document.createElement("a");
    a.href = url;
    a.download = "survey_results.json";
    a.click();
    URL.revokeObjectURL(url);
  };
}

showStart();
