# Занятие 1. Первый запрос к базе из C#

Страница занятия: [Первый запрос к базе из C#](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/lessons/01-pervyy-zapros/index.html)  
Практическая работа: [`practice/01-first-query`](../practice/01-first-query/README.md)

## Что изучили

1. ADO.NET — базовые классы .NET для работы с базами данных. Провайдер Npgsql реализует их для PostgreSQL и подключается пакетом NuGet.
2. Четыре объекта создаются по порядку: `NpgsqlDataSource` → `NpgsqlConnection` → `NpgsqlCommand` → `NpgsqlDataReader`.
3. Запросы на сервер уходят в двух строках: `OpenConnectionAsync` и `ExecuteReaderAsync`.
4. Значения строки подключения берутся из настроек сервера в `docker-compose.yml`. Порт — левое число в `ports`.
5. Текст ошибки указывает этап, на котором остановилась программа: сеть, вход, база или запрос.
6. `ReadAsync` переводит читатель на следующую строку. Get-метод выбирают по типу столбца, номер столбца считают с нуля по порядку в `SELECT`.
7. `await using` закрывает объекты автоматически и возвращает соединение в пул.

## Термины

| Термин | Значение |
|---|---|
| ADO.NET | часть .NET для работы с базами: классы `DbConnection`, `DbCommand`, `DbDataReader` |
| провайдер данных | библиотека, которая реализует классы ADO.NET для одной СУБД; для PostgreSQL — Npgsql |
| источник данных | `NpgsqlDataSource`: хранит строку подключения и пул соединений, при создании к серверу не подключается |
| соединение | `NpgsqlConnection`: открытый сетевой канал к серверу с выполненным входом |
| команда | `NpgsqlCommand`: текст SQL и соединение, по которому его отправить |
| читатель | `NpgsqlDataReader`: получает строки результата по одной и двигается только вперёд |
| пул соединений | запас открытых соединений внутри источника данных; закрытое соединение возвращается в пул открытым |
| строка подключения | пары `ключ=значение` через точку с запятой: `Host`, `Port`, `Database`, `Username`, `Password` |

## Пример: чтение заказов

```csharp
const string connectionString =
    "Host=localhost;Port=5433;Database=orders;Username=student;Password=student";

await using var dataSource = NpgsqlDataSource.Create(connectionString);
await using var connection = await dataSource.OpenConnectionAsync();

await using var command = new NpgsqlCommand(
    "SELECT id, status, total FROM orders ORDER BY id", connection);

await using var reader = await command.ExecuteReaderAsync();

while (await reader.ReadAsync())
{
    int id = reader.GetInt32(0);
    string status = reader.GetString(1);
    decimal total = reader.GetDecimal(2);

    Console.WriteLine($"Заказ {id}: {status}, {total} руб.");
}
```

## Частые ошибки

| Сообщение | Причина | Как исправить |
|---|---|---|
| `Failed to connect to 127.0.0.1:5433` | контейнер с базой не запущен | `docker compose up -d` в корне репозитория |
| `28000: role "student" does not exist` | программа подключилась к другому PostgreSQL на порту 5432 | `Port=5433` в строке подключения |
| `28P01: password authentication failed for user "student"` | неверный пароль | пароль `student`, как в `docker-compose.yml` |
| `3D000: database "order" does not exist` | опечатка в имени базы | база называется `orders` |
| `42601: syntax error at or near "order"` | в запросе вместо `orders` написано `order` — ключевое слово SQL | таблица называется `orders` |
| `Connection is not open` | соединение создано `CreateConnection()`, но не открыто | `await dataSource.OpenConnectionAsync()` |
| `Reading as 'System.Int32' is not supported for fields having DataTypeName 'text'` | Get-метод не подходит к типу столбца | для `text` — `GetString`, для `numeric` — `GetDecimal` |
| `Ordinal must be between 0 and 2` | номер столбца больше последнего | номера считаются с нуля по порядку в `SELECT` |
