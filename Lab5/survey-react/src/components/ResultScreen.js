import React, { useEffect, useState } from 'react';
import './ResultScreen.css';

export default function ResultScreen({ questions, answers, onRestart }) {
  const [saved, setSaved] = useState(false);

  useEffect(() => {
    const record = {
      timestamp: new Date().toISOString(),
      answers: Object.fromEntries(questions.map((q, i) => [q.text, answers[i]])),
    };

    const existing = JSON.parse(localStorage.getItem('survey_results') || '[]');
    existing.push(record);
    localStorage.setItem('survey_results', JSON.stringify(existing));
    setSaved(true);
  }, []);

  const handleDownload = () => {
    const data = localStorage.getItem('survey_results') || '[]';
    const blob = new Blob([JSON.stringify(JSON.parse(data), null, 2)], {
      type: 'application/json',
    });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'survey_results.json';
    a.click();
    URL.revokeObjectURL(url);
  };

  return (
    <div className="result-card">
      <div className="result-header">
        <h2 className="result-title">Опитування завершено</h2>
        <p className="result-subtitle">
          {saved ? 'Відповіді збережено.' : 'Зберігаємо...'}
        </p>
      </div>

      <div className="result-list">
        {questions.map((q, i) => (
          <div className="result-item" key={q.id}>
            <span className="result-question">{q.text}</span>
            <span className="result-answer">{answers[i] || '—'}</span>
          </div>
        ))}
      </div>

      <div className="result-footer">
        <button className="btn-outline" onClick={handleDownload}>
          Завантажити JSON
        </button>
        <button className="btn-next" onClick={onRestart}>
          Пройти знову
        </button>
      </div>
    </div>
  );
}
