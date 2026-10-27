using Npgsql;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const string connectionString =
    "Host=localhost;Port=5433;Database=orders;Username=student;Password=student";

await using var dataSource = NpgsqlDataSource.Create(connectionString);
await using var connection = await dataSource.OpenConnectionAsync();

await using var command = new NpgsqlCommand("""
    SELECT o.id, c.full_name, o.total
    FROM orders o
    JOIN customers c ON c.id = o.customer_id
    WHERE o.status = 'new'
    ORDER BY o.id
    """, connection);

await using var reader = await command.ExecuteReaderAsync();

int count = 0;
while (await reader.ReadAsync())
{
    int id = reader.GetInt32(0);
    string customer = reader.GetString(1);
    decimal total = reader.GetDecimal(2);
    Console.WriteLine($"Заказ {id}: {customer}, {total:F2} руб.");
    count++;
}

Console.WriteLine($"Новых заказов: {count}");