# Отчёт по ДЗ 0. Первый проект на .NET



| | |
|---|---|
| Фамилия и имя | Розанов Владислав |
| Группа | 9/3-РПО-24/1 |
| Система на компьютере | Windows |

## 1. Установка

Вывод `dotnet --list-sdks`:

```text
10.0.401 [C:\Program Files\dotnet\sdk]
10.0.402 [C:\Program Files\dotnet\sdk]
```

Строка `Hello from Docker!` из вывода `docker run hello-world`:

```text
Hello from Docker!
```

## 2. HelloApp

Вывод первого запуска, до изменения `Program.cs`:

```text
Hello, World!
```

Вывод после замены программы:

```text
Привет! Меня зовут Розанов Владислав Александрович, группа 9/3-РПО-24/1.
Версия .NET: 10.0.13
Система: Microsoft Windows 10.0.26200
```

## 3. База заказов и OrdersApp

Вывод `docker compose ps`:

```text
NAME        IMAGE         COMMAND                  SERVICE   CREATED         STATUS         PORTS
orders-db   postgres:17   "docker-entrypoint.s…"   db        7 seconds ago   Up 6 seconds   0.0.0.0:5433->5432/tcp, [::]:5433->5432/tcp
```

Первые три строки вывода OrdersApp:

```text
Заказ 1: shipped, 3380,00 руб.
Заказ 2: paid, 13990,00 руб.
Заказ 3: new, 3290,00 руб.
```

Сколько заказов напечатала программа: 12

