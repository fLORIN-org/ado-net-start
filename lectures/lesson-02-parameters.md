# Занятие 2. Параметры запроса и SQL-инъекция

Страница занятия: [Параметры запроса и SQL-инъекция](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/lessons/02-parametry/index.html)  
Практическая работа: [`practice/02-parameters`](../practice/02-parameters/README.md)

## Что изучили

1. Ввод, вклеенный в текст SQL склейкой, интерполяцией или `string.Format`, может выйти за пределы строки и стать кодом запроса.
2. SQL-инъекция меняет смысл запроса: `' OR 1=1 --` выдаёт всю таблицу, ввод с `; UPDATE …` выполняет второй запрос.
3. Параметр — место `@имя` в тексте запроса. Значение добавляют через `command.Parameters.AddWithValue`, и сервер получает его отдельно от текста.
4. Тип параметра берётся из типа значения в C#. Пустое значение передают как `DBNull.Value`, а ищут условием `IS NULL`.
5. Параметром передают только значения. Имена столбцов и направление сортировки выбирают из белого списка.

## Термины

| Термин | Значение |
|---|---|
| SQL-инъекция | атака, при которой пользователь вводит текст, меняющий смысл SQL-запроса |
| параметр запроса | именованное место в тексте SQL, значение для которого передаётся отдельно от текста |
| `$1` | обозначение места параметра в протоколе PostgreSQL; Npgsql заменяет на него `@имя` |
| `ILIKE` | сравнение строки с шаблоном без учёта регистра; `%` — любые символы, `_` — один символ |
| белый список | заранее заданный набор допустимых вариантов; в запрос попадает только фрагмент из набора |

## Пример: поиск с параметром

```csharp
string sql = "SELECT id, name, price FROM products WHERE name ILIKE @pattern ORDER BY id";
await using var command = new NpgsqlCommand(sql, connection);
command.Parameters.AddWithValue("pattern", "%" + text + "%");
```

## Пример: сортировка по белому списку

```csharp
var sorts = new Dictionary<string, string>
{
    ["1"] = "id",
    ["2"] = "price",
    ["3"] = "price DESC",
};

if (!sorts.TryGetValue(choice, out string? orderBy))
    orderBy = "id";

await using var command = new NpgsqlCommand(
    $"SELECT name, price FROM products ORDER BY {orderBy}", connection);
```

## Частые ошибки

| Что написано | Что произойдёт | Как правильно |
|---|---|---|
| `$"… ILIKE '%{text}%' …"` | ввод `O'Neil` даёт `42601: syntax error at or near "Neil"`, ввод `' OR 1=1 --` выдаёт всю таблицу | `ILIKE @pattern` и `AddWithValue("pattern", "%" + text + "%")` |
| `AddWithValue("id", "3")` для столбца `integer` | `42883: operator does not exist: integer = text` | перевести ввод в число: `int.Parse(input)` |
| `AddWithValue("n", null)` | `Parameter 'n' must have either its DbType, NpgsqlDbType, DataTypeName or its Value set.` | `DBNull.Value` |
| `name = @n` со значением `DBNull.Value` | 0 строк без ошибки | `name IS NULL` |
| в тексте `@n`, а `AddWithValue` не вызван | `42703: column "n" does not exist` | добавить параметр с тем же именем |
| в тексте `@maxPrice`, параметр добавлен как `"price"` | `42703: column "maxprice" does not exist` | имя в `AddWithValue` совпадает с именем в тексте |
| `ORDER BY @col` | ошибки нет, строки не сортируются | белый список фрагментов `ORDER BY` |
