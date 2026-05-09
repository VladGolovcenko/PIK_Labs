package com.example.surveyapp

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.ProgressBar
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity

class SurveyActivity : AppCompatActivity() {

    private val questions = listOf(
        "Як вас звати?",
        "Скільки вам років?",
        "Яка ваша спеціальність або сфера діяльності?",
        "Що вам подобається найбільше у вашій роботі / навчанні?",
        "Які ваші плани на найближчий рік?"
    )

    private val answers = mutableListOf<String>()
    private var currentIndex = 0

    private lateinit var tvCounter: TextView
    private lateinit var tvQuestion: TextView
    private lateinit var etAnswer: EditText
    private lateinit var btnNext: Button
    private lateinit var progressBar: ProgressBar

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_survey)

        tvCounter = findViewById(R.id.tvCounter)
        tvQuestion = findViewById(R.id.tvQuestion)
        etAnswer = findViewById(R.id.etAnswer)
        btnNext = findViewById(R.id.btnNext)
        progressBar = findViewById(R.id.progressBar)

        progressBar.max = questions.size
        updateUI()

        btnNext.setOnClickListener {
            val answer = etAnswer.text.toString().trim()
            answers.add(answer)
            etAnswer.setText("")

            if (currentIndex + 1 < questions.size) {
                currentIndex++
                updateUI()
            } else {
                val intent = Intent(this, ResultActivity::class.java)
                intent.putStringArrayListExtra("questions", ArrayList(questions))
                intent.putStringArrayListExtra("answers", ArrayList(answers))
                startActivity(intent)
                finish()
            }
        }
    }

    private fun updateUI() {
        tvCounter.text = "${currentIndex + 1} / ${questions.size}"
        tvQuestion.text = questions[currentIndex]
        progressBar.progress = currentIndex + 1
        btnNext.text = if (currentIndex == questions.size - 1) "Завершити" else "Далі"
    }
}
