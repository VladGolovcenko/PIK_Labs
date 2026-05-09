package com.example.surveyapp

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.LinearLayout
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import org.json.JSONArray
import org.json.JSONObject
import java.io.File
import java.text.SimpleDateFormat
import java.util.Date
import java.util.Locale

class ResultActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_result)

        val questions = intent.getStringArrayListExtra("questions") ?: return
        val answers = intent.getStringArrayListExtra("answers") ?: return

        saveResults(questions, answers)

        val container = findViewById<LinearLayout>(R.id.llResults)

        questions.forEachIndexed { i, question ->
            val qView = TextView(this).apply {
                text = question
                textSize = 12f
                setTextColor(0xFF888888.toInt())
                setPadding(0, 24, 0, 4)
            }

            val aView = TextView(this).apply {
                text = answers[i].ifEmpty { "—" }
                textSize = 15f
                setTextColor(0xFF111111.toInt())
            }

            val divider = android.view.View(this).apply {
                setBackgroundColor(0xFFDDDDDD.toInt())
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT, 1
                ).also { it.topMargin = 20 }
            }

            container.addView(qView)
            container.addView(aView)
            container.addView(divider)
        }

        findViewById<Button>(R.id.btnRestart).setOnClickListener {
            startActivity(Intent(this, SurveyActivity::class.java))
            finish()
        }
    }

    private fun saveResults(questions: List<String>, answers: List<String>) {
        val record = JSONObject().apply {
            put("timestamp", SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault()).format(Date()))
            val answersObj = JSONObject()
            questions.forEachIndexed { i, q -> answersObj.put(q, answers[i]) }
            put("answers", answersObj)
        }

        val file = File(filesDir, "survey_results.json")
        val data = JSONArray()

        if (file.exists()) {
            try {
                val existing = JSONArray(file.readText())
                for (j in 0 until existing.length()) data.put(existing.get(j))
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }

        data.put(record)
        file.writeText(data.toString(2))
    }
}
