# Занятие 2. Параметры запроса и SQL-инъекция

Страница занятия: [Параметры запроса и SQL-инъекция](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/lessons/02-parametry/index.html)  
Конспект: [`lectures/lesson-02-parameters.md`](../../lectures/lesson-02-parameters.md)  
Практическая работа: [`practice/02-parameters`](../../practice/02-parameters/README.md), условия заданий — [`tasks.md`](../../practice/02-parameters/tasks.md)

На занятии раздаётся архив `search-app.zip`. Его файлы уже лежат в репозитории:

| Файл из архива | Где лежит в репозитории | Что это |
|---|---|---|
| `SearchApp/` | [`practice/02-parameters/SearchApp`](../../practice/02-parameters/SearchApp) | поиск товаров, в котором текст поиска вклеивается в SQL; исправляется в задании 4 |

Вводы для опытов на занятии: `USB`, `' OR 1=1 --`, `'; UPDATE products SET price = 1; --`.
После опыта с `UPDATE` база возвращается к исходным строкам командами `docker compose down` и `docker compose up -d`.
