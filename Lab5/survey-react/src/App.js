import React, { useState } from 'react';
import SurveyScreen from './components/SurveyScreen';
import ResultScreen from './components/ResultScreen';
import questions from './questions';
import './App.css';

export default function App() {
  const [answers, setAnswers] = useState([]);
  const [finished, setFinished] = useState(false);

  const handleFinish = (collectedAnswers) => {
    setAnswers(collectedAnswers);
    setFinished(true);
  };

  const handleRestart = () => {
    setAnswers([]);
    setFinished(false);
  };

  return (
    <div className="app">
      {finished ? (
        <ResultScreen questions={questions} answers={answers} onRestart={handleRestart} />
      ) : (
        <SurveyScreen questions={questions} onFinish={handleFinish} />
      )}
    </div>
  );
}
