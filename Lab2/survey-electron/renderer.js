const questions = [
    'Як вас звати?',
    'Скільки вам років?',
    'Яка ваша спеціальність?',
    'Яке ваше хобі?',
    'Що вам найбільше подобається у навчанні?'
];

const answers = new Array(questions.length).fill('');
let current = 0;

const progressEl = document.getElementById('progress');
const questionEl = document.getElementById('question');
const answerEl = document.getElementById('answer');
const btnPrev = document.getElementById('btnPrev');
const btnNext = document.getElementById('btnNext');
const surveyView = document.getElementById('surveyView');
const doneView = document.getElementById('doneView');
const btnSave = document.getElementById('btnSave');

function render() {
    progressEl.textContent = `Питання ${current + 1} з ${questions.length}`;
    questionEl.textContent = questions[current];
    answerEl.value = answers[current];
    answerEl.focus();
    btnPrev.disabled = current === 0;
    btnNext.textContent = current === questions.length - 1 ? 'Завершити' : 'Далі';
}

btnNext.addEventListener('click', () => {
    const value = answerEl.value.trim();
    if (!value) {
        answerEl.style.borderColor = '#c00';
        return;
    }
    answerEl.style.borderColor = '#ccc';
    answers[current] = value;

    if (current === questions.length - 1) {
        surveyView.style.display = 'none';
        doneView.style.display = 'block';
    } else {
        current++;
        render();
    }
});

btnPrev.addEventListener('click', () => {
    answers[current] = answerEl.value.trim();
    current--;
    render();
});

btnSave.addEventListener('click', async () => {
    const results = questions.map((q, i) => ({ question: q, answer: answers[i] }));
    const result = await window.api.saveResults(results);
    if (result.success) {
        window.close();
    }
});

render();
