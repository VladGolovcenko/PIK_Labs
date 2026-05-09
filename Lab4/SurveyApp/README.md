# SurveyApp — Форма опитування

Android-застосунок написаний на Kotlin.

## Як відкрити в Android Studio

1. Встанови Android Studio: https://developer.android.com/studio
2. При першому запуску пройди Setup Wizard (встановить SDK автоматично)
3. На стартовому екрані: **Open** → обери папку `SurveyApp`
4. Зачекай поки Gradle синхронізується (1-3 хвилини, перший раз довше)

## Як запустити на емуляторі

1. У верхньому меню: **Tools → Device Manager**
2. **Create Device** → обери Pixel 6 → Next
3. Обери образ системи: **API 34 (Android 14)** → Download → Next → Finish
4. Натисни кнопку ▶ (Run) або `Shift+F10`
5. Застосунок відкриється в емуляторі

## Опис застосунку

- Послідовно показує 5 питань
- Користувач вводить відповідь у текстове поле
- Прогрес-бар показує поточний крок
- Після завершення відповіді зберігаються у файл `survey_results.json`
  у внутрішньому сховищі застосунку (`/data/data/com.example.surveyapp/files/`)
- Екран результатів відображає всі надані відповіді
- Кнопка "Пройти знову" повертає на початок

## Структура проекту

```
SurveyApp/
├── app/src/main/
│   ├── java/com/example/surveyapp/
│   │   ├── SurveyActivity.kt       # Екран опитування
│   │   └── ResultActivity.kt       # Екран результатів
│   ├── res/
│   │   ├── layout/
│   │   │   ├── activity_survey.xml
│   │   │   └── activity_result.xml
│   │   ├── drawable/
│   │   │   ├── bg_button.xml
│   │   │   └── bg_input.xml
│   │   └── values/
│   │       ├── strings.xml
│   │       └── themes.xml
│   └── AndroidManifest.xml
├── build.gradle
└── settings.gradle
```
