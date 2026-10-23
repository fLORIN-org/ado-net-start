# Практическая работа 2. Задания

Занятие: [Параметры запроса и SQL-инъекция](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/lessons/02-parametry/index.html), раздел «Практика».
Конспект: [`lectures/lesson-02-parameters.md`](../../lectures/lesson-02-parameters.md)

В этом файле условия всех четырёх заданий. Ответы на задания 1–3 вводятся на странице занятия:
там проверка, баллы и разбор. Задание 4 выполняется в проекте `SearchApp` из этой папки.
Всего 10 баллов.

## Что хранится в базе

Все четыре задания работают с базой стартового проекта. В заданиях 2 и 4 она исходная: такая, какой её создаёт скрипт [`db/init.sql`](../../db/init.sql).
Скрипт выполняется при первом запуске контейнера. В ней четыре таблицы,
в практике этого занятия нужны две: `products` — товары и `customers` — клиенты.
Таблицы `orders` и `order_items` с заказами в заданиях не используются.

Таблица `products`, 8 строк:

| id | name | price | stock |
|---|---|---|---|
| 1 | Клавиатура | 2490.00 | 14 |
| 2 | Мышь | 890.00 | 40 |
| 3 | Монитор 24" | 13990.00 | 6 |
| 4 | Наушники | 3290.00 | 18 |
| 5 | Веб-камера | 2790.00 | 9 |
| 6 | Коврик для мыши | 390.00 | 55 |
| 7 | USB-хаб | 1190.00 | 22 |
| 8 | Подставка для ноутбука | 1890.00 | 11 |

Таблица `customers`, 6 строк:

| id | full_name | city |
|---|---|---|
| 1 | Анна Смирнова | Казань |
| 2 | Игорь Волков | Самара |
| 3 | Мария Козлова | Казань |
| 4 | Дмитрий Орлов | Уфа |
| 5 | Елена Новикова | Самара |
| 6 | Павел Соколов | Пермь |

Столбец `price` имеет тип `numeric(10,2)`: цена хранится с двумя знаками после точки, в C# читается как `decimal`.
`stock` — сколько штук товара на складе. Если в опытах цены изменились, исходные строки возвращаются
командами `docker compose down` и `docker compose up -d`.

## Задание 1. Где можно внедрить SQL · 2 балла

В каждой строке переменные `name`, `city`, `text` и `choice` получены от пользователя.
Словарь `sorts` — белый список из раздела 5 лекции.

```csharp
// 1
new NpgsqlCommand($"SELECT * FROM customers WHERE full_name = '{name}'", connection);

// 2
new NpgsqlCommand("SELECT * FROM customers WHERE full_name = @name", connection);
command.Parameters.AddWithValue("name", name);

// 3
new NpgsqlCommand("SELECT * FROM customers WHERE city = '" + city + "'", connection);

// 4
new NpgsqlCommand(string.Format("SELECT * FROM customers WHERE city = '{0}'", city), connection);

// 5
new NpgsqlCommand("SELECT * FROM customers WHERE city = @city", connection);
command.Parameters.AddWithValue("city", city);

// 6
new NpgsqlCommand("SELECT * FROM products WHERE name ILIKE @p", connection);
command.Parameters.AddWithValue("p", "%" + text + "%");

// 7
new NpgsqlCommand($"SELECT * FROM products ORDER BY {sorts[choice]}", connection);
```

**Что нужно сделать.** Для каждой строки отметить, можно ли через неё выполнить SQL-инъекцию: «можно» или «нельзя».

## Задание 2. Что сделает запрос · 3 балла

Программа поиска собирает запрос склейкой, как в разделе 1 лекции:

```csharp
string sql = $"SELECT id, name, price FROM products WHERE name ILIKE '%{text}%' ORDER BY id";
```

База — исходная из стартового проекта: 8 товаров. Сочетание «мыш» есть в двух названиях —
Мышь и Коврик для мыши.

Ввод пользователя:

1. `мыши`
2. `D'Artagnan`
3. `' OR 2>1 --`
4. `'; UPDATE products SET price = 1; --`
5. `мыш`

Варианты результата:

- не найдёт ни одного товара;
- найдёт 1 товар;
- найдёт 2 товара;
- выдаст все 8 товаров;
- закончится ошибкой 42601;
- выдаст 8 товаров и поставит всем цену 1.00.

**Что нужно сделать.** Для каждого ввода выбрать, что произойдёт при запуске.

## Задание 3. Запрос с параметром по образцу · 2 балла

Менеджер вводит максимальную цену, программа читает её в переменную `decimal maxPrice`
и должна показать товары не дороже этой цены, от дешёвых к дорогим.

```csharp
await using var command = new NpgsqlCommand(
    "SELECT id, name, price FROM products WHERE price <= ___ ORDER BY price", connection);

command.Parameters.AddWithValue(___, ___);
```

**Что нужно сделать.** Заполнить три пропуска так, чтобы цена передавалась параметром.

## Задание 4. Исправить поиск в SearchApp · 3 балла

Приложение `SearchApp` ищет товары по части названия и печатает запрос, который отправляет на сервер.
Текст поиска в нём вклеивается в SQL интерполяцией.

1. Запустите базу. Если вы меняли данные в прошлых опытах, сначала выполните `docker compose down`:
   база создастся заново.

   ```bash
   docker compose up -d                  # в корне репозитория
   cd practice/02-parameters/SearchApp
   dotnet run
   ```

2. На запрос «Поиск товара» введите `USB`: программа найдёт 1 товар.
3. Запустите ещё раз и введите `' OR 1=1 --`. Весь вывод скопируйте в поле 1 на странице занятия.
4. Исправьте `Program.cs`: в тексте запроса — параметр `@pattern`, значение со знаками `%` —
   через `command.Parameters.AddWithValue`.
5. Запустите исправленную программу дважды: с вводом `' OR 1=1 --` и с вводом `USB`.
   Выводы скопируйте в поля 2 и 3.

Подсказка: строка запроса больше не должна содержать `{text}`. Параметр добавляется после создания
команды и до вызова `ExecuteReaderAsync`. Шаблон собирается так: `"%" + text + "%"`.

## Сдача

1. На странице занятия нажмите «Скачать отчёт .md» и сохраните файл как `practice/02-parameters/report.md`.
2. Сделайте коммит `practice 02: поиск через параметр @pattern` вместе с исправленным `SearchApp/Program.cs`
   и отправьте его на GitHub.
