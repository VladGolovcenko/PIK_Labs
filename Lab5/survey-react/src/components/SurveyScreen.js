import React, { useState } from 'react';
import './SurveyScreen.css';

export default function SurveyScreen({ questions, onFinish }) {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [answers, setAnswers] = useState(Array(questions.length).fill(''));
  const [animating, setAnimating] = useState(false);

  const current = questions[currentIndex];
  const isLast = currentIndex === questions.length - 1;
  const progress = ((currentIndex + 1) / questions.length) * 100;

  const handleChange = (e) => {
    const updated = [...answers];
    updated[currentIndex] = e.target.value;
    setAnswers(updated);
  };

  const handleNext = () => {
    if (animating) return;
    setAnimating(true);
    setTimeout(() => {
      if (isLast) {
        onFinish(answers);
      } else {
        setCurrentIndex((i) => i + 1);
      }
      setAnimating(false);
    }, 200);
  };

  const handleKeyDown = (e) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleNext();
    }
  };

  return (
    <div className="survey-card">
      <div className="survey-header">
        <div className="progress-track">
          <div className="progress-fill" style={{ width: `${progress}%` }} />
        </div>
        <span className="counter">{currentIndex + 1} / {questions.length}</span>
      </div>

      <div className={`survey-body ${animating ? 'fade-out' : 'fade-in'}`}>
        <p className="question-label">Питання {currentIndex + 1}</p>
        <h2 className="question-text">{current.text}</h2>

        <textarea
          className="answer-input"
          placeholder="Ваша відповідь..."
          value={answers[currentIndex]}
          onChange={handleChange}
          onKeyDown={handleKeyDown}
          rows={4}
          autoFocus
        />
      </div>

      <div className="survey-footer">
        <span className="hint">Enter — продовжити</span>
        <button className="btn-next" onClick={handleNext}>
          {isLast ? 'Завершити' : 'Далі'}
        </button>
      </div>
    </div>
  );
}
