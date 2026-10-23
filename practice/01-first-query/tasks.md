# Практическая работа 1. Задания

Занятие: [Первый запрос к базе из C#](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/lessons/01-pervyy-zapros/index.html), раздел «Практика».
Конспект: [`lectures/lesson-01-first-query.md`](../../lectures/lesson-01-first-query.md)

В этом файле условия всех четырёх заданий. Ответы на задания 1–3 вводятся на странице занятия:
там проверка, баллы и разбор. Задание 4 выполняется в проекте `OrdersApp` из этой папки.
Всего 10 баллов.

## Задание 1. Порядок строк программы · 2 балла

Ниже шесть строк программы, которая печатает статусы всех заказов. Строки перемешаны.

```csharp
await using var reader = await command.ExecuteReaderAsync();
await using var dataSource = NpgsqlDataSource.Create(connectionString);
    Console.WriteLine(reader.GetString(0));
await using var command = new NpgsqlCommand("SELECT status FROM orders", connection);
while (await reader.ReadAsync())
await using var connection = await dataSource.OpenConnectionAsync();
```

**Что нужно сделать.** Расставить строки в том порядке, в котором они идут в программе.

## Задание 2. Строка подключения по docker-compose.yml · 2 балла

Для другого проекта коллега прислал файл `docker-compose.yml` с базой склада.
Контейнер запущен на вашем компьютере.

```yaml
services:
  stock-db:
    image: postgres:17
    environment:
      POSTGRES_DB: warehouse
      POSTGRES_USER: keeper
      POSTGRES_PASSWORD: k33per
    ports:
      - "6543:5432"
```

**Что нужно сделать.** Заполнить пять полей строки подключения, по которой программа
на этом же компьютере подключится к базе из файла:

```text
Host=___;Port=___;Database=___;Username=___;Password=___
```

## Задание 3. Четыре запуска · 3 балла

В программе ниже четыре ошибки. Её запускали четыре раза и после каждого запуска исправляли одну ошибку.

- Контейнер из стартового проекта запущен, порт в `docker-compose.yml` — `"5433:5432"`.
- Другого PostgreSQL на компьютере нет.

```csharp
 1  using Npgsql;
 2
 3  const string connectionString =
 4      "Host=localhost;Port=5432;Database=orders;Username=student;Password=student";
 5
 6  await using var dataSource = NpgsqlDataSource.Create(connectionString);
 7  await using var connection = dataSource.CreateConnection();
 8
 9  await using var command = new NpgsqlCommand(
10      "SELECT id, status, total FROM order", connection);
11
12  await using var reader = await command.ExecuteReaderAsync();
13
14  while (await reader.ReadAsync())
15  {
16      int id = reader.GetInt32(0);
17      int status = reader.GetInt32(1);
18      decimal total = reader.GetDecimal(2);
19
20      Console.WriteLine($"Заказ {id}: {status}, {total} руб.");
21  }
```

Сообщения в том порядке, в котором они появлялись:

| Запуск | Сообщение |
|---|---|
| 1 | `System.InvalidOperationException: Connection is not open` |
| 2 | `Npgsql.NpgsqlException: Failed to connect to 127.0.0.1:5432` |
| 3 | `Npgsql.PostgresException: 42601: syntax error at or near "order"` |
| 4 | `System.InvalidCastException: Reading as 'System.Int32' is not supported for fields having DataTypeName 'text'` |

**Что нужно сделать.** Для каждого сообщения указать строку кода, которая вызвала эту ошибку.

## Задание 4. Новые заказы в стартовом проекте · 3 балла

Менеджеру склада нужен список новых заказов: номер заказа, имя клиента и сумма. В конце списка —
сколько всего новых заказов. Новые заказы — строки таблицы `orders` со статусом `new`.
Имя клиента хранится в таблице `customers` в столбце `full_name`, заказ связан с клиентом
через `orders.customer_id`.

1. Запустите базу и программу. Программа должна напечатать 12 заказов.

   ```bash
   docker compose up -d                  # в корне репозитория
   cd practice/01-first-query/OrdersApp
   dotnet run
   ```

2. Измените в `Program.cs` текст запроса: оставьте только новые заказы и вместо статуса выберите имя клиента.
3. Измените чтение второго столбца и вывод, чтобы строка выглядела так: `Заказ 3: Мария Козлова, 3290,00 руб.`
4. Добавьте счётчик строк и после цикла напечатайте `Новых заказов: N`.
5. Запустите `dotnet run`. Текст запроса и весь вывод программы вставьте в поля задания 4 на странице занятия.

Подсказки:

- таблицы соединяются через `JOIN customers c ON c.id = o.customer_id`, условие — `WHERE o.status = 'new'`;
  многострочный текст запроса в C# записывают в тройных кавычках `"""`;
- во втором столбце теперь текст, поэтому читать его нужно тем же методом, что и статус;
- счётчик — переменная `int count = 0;` до цикла и `count++;` в теле цикла.

## Сдача

1. На странице занятия нажмите «Скачать отчёт .md» и сохраните файл как `practice/01-first-query/report.md`.
2. Сделайте коммит `practice 01: новые заказы с именем клиента` вместе с изменённым `OrdersApp/Program.cs`
   и отправьте его на GitHub.
