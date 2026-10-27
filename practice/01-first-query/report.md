# Занятие 1 · Первый запрос к базе из C#

МДК.01.11 Технология доступа к базам данных ADO.NET

| | |
|---|---|
| Студент | Розанов Владислав |
| Группа | 9/3-РПО-24/1 |
| Дата | 27.10.2026 |

## Баллы

| Задание | Баллы |
|---|---|
| 1. Порядок строк программы | 2 / 2 |
| 2. Строка подключения по docker-compose.yml | 0 / 2 |
| 3. Четыре запуска | 3 / 3 |
| 4. Новые заказы в стартовом проекте | 3 / 3 |
| **Итого** | **8 / 10** |

## Задание 1. Порядок строк

```csharp
await using var dataSource = NpgsqlDataSource.Create(connectionString);
await using var connection = await dataSource.OpenConnectionAsync();
await using var command = new NpgsqlCommand("SELECT status FROM orders", connection);
await using var reader = await command.ExecuteReaderAsync();
while (await reader.ReadAsync())
    Console.WriteLine(reader.GetString(0));
```

## Задание 2. Строка подключения

```text
Host=localhost;Port=6543:5432;Database=warehouse;Username=keeper;Password=keeper
```

## Задание 3. Четыре запуска

| Запуск | Сообщение | Выбранная строка |
|---|---|---|
| 1 | `System.InvalidOperationException: Connection is not open` | 7 |
| 2 | `Npgsql.NpgsqlException: Failed to connect to 127.0.0.1:5432` | 4 |
| 3 | `Npgsql.PostgresException: 42601: syntax error at or near "order"` | 10 |
| 4 | `System.InvalidCastException: Reading as 'System.Int32' is not supported for fields having DataTypeName 'text'` | 17 |

## Задание 4. Новые заказы

Запрос:

```sql
SELECT o.id, c.full_name, o.total
FROM orders o
JOIN customers c ON c.id = o.customer_id
WHERE o.status = 'new'
ORDER BY o.id
```

Вывод программы:

```text
Заказ 3: Мария Козлова, 3290,00 руб.
Заказ 5: Дмитрий Орлов, 5280,00 руб.
Заказ 7: Игорь Волков, 2490,00 руб.
Заказ 10: Елена Новикова, 1190,00 руб.
Новых заказов: 4
```
