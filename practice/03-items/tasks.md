# Практическая работа 3. Задания

Занятие: [Строки в объекты и ошибки ограничений](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/lessons/03-obekty-i-oshibki/index.html), раздел «Практика».
Конспект: [`lectures/lesson-03-items.md`](../../lectures/lesson-03-items.md)

В этом файле условия всех четырёх заданий. Ответы на задания 1–3 вводятся на странице занятия:
там проверка, баллы и разбор. Задание 4 выполняется в проекте `ItemsApp` из этой папки.
Всего 10 баллов.

## Задание 1. Маппер для статистики клиента · 2 балла

Запрос возвращает для каждого клиента имя, число заказов и дату последнего отменённого заказа.
У четырёх клиентов из шести отменённых заказов нет.

```sql
SELECT c.full_name AS name,
       count(o.id) AS orders_count,
       max(o.created_at) FILTER (WHERE o.status = 'cancelled') AS last_cancelled
FROM customers c
LEFT JOIN orders o ON o.customer_id = c.id
GROUP BY c.id, c.full_name
ORDER BY c.id
```

```csharp
record CustomerStat(string Name, long OrdersCount, DateOnly? LastCancelled);
```

Варианты для каждого свойства:

| Свойство | Варианты выражения |
|---|---|
| `Name` | `reader.GetString(reader.GetOrdinal("full_name"))`<br>`reader.GetString(reader.GetOrdinal("name"))`<br>`reader.GetInt32(0)` |
| `OrdersCount` | `reader.GetString(reader.GetOrdinal("orders_count"))`<br>`reader.GetInt64(reader.GetOrdinal("count"))`<br>`reader.GetInt64(reader.GetOrdinal("orders_count"))` |
| `LastCancelled` | `reader.GetFieldValue<DateOnly?>(reader.GetOrdinal("last_cancelled"))`<br>`reader.GetFieldValue<DateOnly>(reader.GetOrdinal("last_cancelled"))`<br>`(DateOnly?)reader.GetValue(reader.GetOrdinal("last_cancelled"))` |

**Что нужно сделать.** Для каждого свойства выбрать выражение, которое прочитает его
для всех строк результата без исключения.

## Задание 2. Чем закончится INSERT · 3 балла

Метод вызывается пять раз подряд на исходной базе из стартового проекта. Обработки ошибок в нём нет.
Заказов в базе 12, товаров 8. В заказе 2 уже есть товар 3.

```csharp
static async Task<int> InsertItemAsync(NpgsqlDataSource dataSource,
    int orderId, int productId, int quantity, decimal? price)
{
    await using var connection = await dataSource.OpenConnectionAsync();
    await using var command = new NpgsqlCommand(
        "INSERT INTO order_items (order_id, product_id, quantity, price) " +
        "VALUES (@order, @product, @quantity, @price)", connection);
    command.Parameters.AddWithValue("order", orderId);
    command.Parameters.AddWithValue("product", productId);
    command.Parameters.AddWithValue("quantity", quantity);
    command.Parameters.AddWithValue("price", (object?)price ?? DBNull.Value);
    return await command.ExecuteNonQueryAsync();
}
```

Вызовы по порядку:

1. `InsertItemAsync(ds, 2, 3, 1, 13990m)`
2. `InsertItemAsync(ds, 2, 1, 2, 2490m)`
3. `InsertItemAsync(ds, 13, 1, 1, 2490m)`
4. `InsertItemAsync(ds, 2, 1, -1, 2490m)`
5. `InsertItemAsync(ds, 2, 8, 1, null)`

Варианты результата: вернёт 1, вернёт 0, исключение 23505, исключение 23503, исключение 23514, исключение 23502.

**Что нужно сделать.** Для каждого вызова выбрать, чем он закончится.

## Задание 3. Метод выполнения команды · 2 балла

Задачи:

1. Получить список позиций заказа с названиями товаров.
2. Удалить позицию и узнать, была ли она в заказе.
3. Узнать, сколько заказов в статусе `new`.
4. Добавить клиента и получить номер, который ему назначила база.
5. Отменить все новые заказы клиента и узнать, сколько отменено.

Методы: `ExecuteReaderAsync`, `ExecuteNonQueryAsync`, `ExecuteScalarAsync`.

**Что нужно сделать.** Для каждой задачи выбрать метод `NpgsqlCommand`, которым её решают.

## Задание 4. Позиции заказа в ItemsApp · 3 балла

Приложение `ItemsApp` четыре раза пытается добавить позицию в заказ 1 и печатает позиции заказа.
Сейчас программа завершается на второй попытке с ошибкой 23505, а метод `GetItemsAsync`
возвращает пустой список.

1. Запустите базу и программу, убедитесь, что она падает на второй попытке.

   ```bash
   docker compose up -d                  # в корне репозитория
   cd practice/03-items/ItemsApp
   dotnet run
   ```

2. В `AddItemAsync` перехватите три ошибки и верните тексты:
   - для 23505 — `Товар {productId} уже есть в заказе {orderId}.`
   - для 23503 — `Заказа {orderId} или товара {productId} нет в базе.`
   - для 23514 — `Количество должно быть больше нуля, указано {quantity}.`
3. В `GetItemsAsync` создайте `OrderItemRow` из текущей строки, читая столбцы по имени
   через `GetOrdinal`, и добавьте его в список.
4. Запустите программу. Весь вывод вставьте в поле задания 4 на странице занятия.

Подсказки:

- вызов `ExecuteNonQueryAsync` и `return` помещаются в блок `try`; после него идут три блока вида
  `catch (PostgresException e) when (e.SqlState == PostgresErrorCodes.UniqueViolation)`, по одному на код;
- имена столбцов в запросе: `product_name`, `quantity`, `price`;
  пример чтения: `reader.GetString(reader.GetOrdinal("product_name"))`.

## Сдача

1. На странице занятия нажмите «Скачать отчёт .md» и сохраните файл как `practice/03-items/report.md`.
2. Сделайте коммит `practice 03: маппер и ошибки ограничений` вместе с изменённым `ItemsApp/Program.cs`
   и отправьте его на GitHub.
