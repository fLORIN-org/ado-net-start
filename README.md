# ado-net-start

<!-- Замените «Имя Фамилия» и группу на свои. Этот файл ваш: обновление материалов его не меняет. -->

Мои работы по дисциплине «Технология доступа к базам данных ADO.NET»: практические работы с занятий,
домашние задания, тесты и контрольные. Программы написаны на C# и работают с базой PostgreSQL,
которая запускается в Docker из корня репозитория.

| | |
|---|---|
| Автор | Имя Фамилия |
| Группа | |
| Курс | 3 курс, 2026/2027 |
| Язык | C# 14, .NET 10 |
| База данных | PostgreSQL 17 в Docker, провайдер Npgsql |
| Редактор | VS Code с расширением C# Dev Kit |

## Лучшие работы

<!-- Добавьте 2–3 работы, которые стоит открыть первыми: ссылка на папку и одна строка о программе. -->

| Работа | Что делает программа |
|---|---|
| [`practice/01-first-query`](practice/01-first-query/README.md) | читает заказы из PostgreSQL через Npgsql |

## Структура

```
ado-net-start/
├── docker-compose.yml   база заказов в Docker (ведёт преподаватель)
├── db/                  таблицы и строки базы (ведёт преподаватель)
├── practice/            работы на занятиях: одна папка на занятие
├── homework/            домашние задания, первое — ДЗ 0 «Первый проект на .NET»
├── exams/               тесты и контрольные работы
├── lectures/            конспекты занятий (ведёт преподаватель)
├── materials/           файлы с занятий (ведёт преподаватель)
└── docs/                правила сдачи и обновления (ведёт преподаватель)
```

## Как запустить

```bash
git clone https://github.com/<логин>/ado-net-start.git
cd ado-net-start
docker compose up -d                       # база заказов на порту 5433
cd practice/01-first-query/OrdersApp
dotnet run
```

Что установить для запуска: .NET 10 SDK и Docker, инструкция —
[установка .NET 10 SDK и Docker](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/ustanovka/index.html).
После установки выполните [ДЗ 0](homework/00-first-project/README.md): проект HelloApp и проверка базы.

## Материалы курса

- [Страница дисциплины: занятия и практика](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/index.html)
- [Конспекты занятий](lectures/README.md)
- [Как сдавать работы](docs/SUBMIT.md) и [как приходят новые материалы](docs/UPDATES.md)
- [Пошаговая инструкция по работе с репозиторием](https://algorthimization-course-vvodnoe.vercel.app/dup02-vvedenie-v-python/repozitoriy/index.html)
  (написана для курса Python: там репозиторий называется `python-start`, у вас — `ado-net-start`, шаги те же)
