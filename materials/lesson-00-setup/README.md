# Занятие 0. Установка .NET 10 SDK и Docker

Инструкция: [установка .NET 10 SDK и Docker](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/ustanovka/index.html)  
Конспект: [`lectures/lesson-00-setup.md`](../../lectures/lesson-00-setup.md)  
Домашнее задание: [`homework/00-first-project`](../../homework/00-first-project/README.md)

На странице инструкции раздаётся архив `orders-starter.zip`. Его файлы уже лежат в репозитории:

| Файл из архива | Где лежит в репозитории | Что это |
|---|---|---|
| `docker-compose.yml` | [`docker-compose.yml`](../../docker-compose.yml) | контейнер PostgreSQL 17: база `orders`, пользователь `student`, порт 5433 |
| `db/init.sql` | [`db/init.sql`](../../db/init.sql) | таблицы `customers`, `products`, `orders`, `order_items` и их строки |
| `OrdersApp/` | [`practice/01-first-query/OrdersApp`](../../practice/01-first-query/OrdersApp) | программа, которая печатает 12 заказов |
