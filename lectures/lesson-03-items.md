# Занятие 3. Строки в объекты и ошибки ограничений

Страница занятия: [Строки в объекты и ошибки ограничений](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/lessons/03-obekty-i-oshibki/index.html)  
Практическая работа: [`practice/03-items`](../practice/03-items/README.md)

## Что изучили

1. Строку результата переносят в объект `record` методом-маппером; метод чтения возвращает список объектов.
2. Столбцы читают по имени через `GetOrdinal`: маппер не зависит от порядка столбцов в `SELECT`.
3. NULL читают в тип с `?`: `GetFieldValue<DateOnly?>` или проверка `IsDBNull`. Без этого Get-метод бросает исключение.
4. `ExecuteNonQueryAsync` возвращает число затронутых строк, `ExecuteScalarAsync` — одно значение.
5. Нарушение ограничения приходит как `PostgresException` с кодом: 23505 — повтор ключа, 23503 — нет связанной строки, 23514 — не прошла проверка, 23502 — пустое значение.

## Термины

| Термин | Значение |
|---|---|
| `record` | тип C#, объявленный одной строкой; компилятор создаёт конструктор и свойства, которые не меняются после создания |
| маппер | метод, который переносит одну строку читателя в объект |
| `GetOrdinal` | возвращает номер столбца по имени; регистр не учитывается, после `AS` ищется новое имя |
| NULL | отсутствие значения в столбце; часто появляется после `LEFT JOIN` |
| ограничение | правило таблицы: `PRIMARY KEY`, `REFERENCES`, `CHECK`, `NOT NULL` |
| `PostgresException` | исключение Npgsql с ответом сервера: `SqlState`, `ConstraintName`, `MessageText`, `Detail` |

## Пример: маппер по именам

```csharp
static OrderRow MapOrder(NpgsqlDataReader reader) => new(
    reader.GetInt32(reader.GetOrdinal("id")),
    reader.GetString(reader.GetOrdinal("status")),
    reader.GetDecimal(reader.GetOrdinal("total")));

record OrderRow(int Id, string Status, decimal Total);
```

## Пример: обработка ошибки ограничения

```csharp
try
{
    int rows = await command.ExecuteNonQueryAsync();
    return $"Добавлено строк: {rows}";
}
catch (PostgresException e) when (e.SqlState == PostgresErrorCodes.UniqueViolation)
{
    return $"Товар {productId} уже есть в заказе {orderId}.";
}
```

## Частые ошибки

| Сообщение | Причина | Как исправить |
|---|---|---|
| `Reading as 'System.Int32' is not supported for fields having DataTypeName 'text'` | порядок столбцов в `SELECT` изменился, маппер читает по номерам | читать по имени через `GetOrdinal` |
| `Field not found in row: full_name` | столбец переименован словом `AS` | использовать имя после `AS` |
| `Column 'last_cancelled' is null.` | `GetFieldValue<DateOnly>` на NULL | `GetFieldValue<DateOnly?>` или `IsDBNull` |
| `23505: duplicate key value violates unique constraint "order_items_pkey"` | такая пара заказ — товар уже есть | сообщить пользователю или изменить количество через `UPDATE` |
| `23503: … violates foreign key constraint "order_items_product_id_fkey"` | товара с таким номером нет | проверить номер товара |
| `23514: … violates check constraint "order_items_quantity_check"` | количество 0 или меньше | количество больше нуля |
| `23502: null value in column "price" … violates not-null constraint` | цена передана как `DBNull.Value` | передать цену |
| `ExecuteNonQueryAsync` вернул -1 | так выполнен `SELECT` | для `SELECT` — читатель или `ExecuteScalarAsync` |
