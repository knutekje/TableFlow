namespace TableFlowBackend.Services;

public class TableService
{
    private readonly IRepository<Table> _tableRepository;

    public TableService(IRepository<Table> tableRepository)
    {
        _tableRepository = tableRepository;
    }

    public async Task<IEnumerable<Table>> GetAllTablesAsync()
    {
        return await _tableRepository.GetAllAsync();
    }

    public async Task<Table?> GetTableByIdAsync(int id)
    {
        return await _tableRepository.GetByIdAsync(id);
    }

    public async Task AddTableAsync(Table table)
    {
        if (table.Capacity <= 0)
            throw new ArgumentException("Table capacity must be greater than zero.");

        await _tableRepository.AddAsync(table);
    }

    public async Task UpdateTableAsync(Table table)
    {
        await _tableRepository.UpdateAsync(table);
    }

    public async Task DeleteTableAsync(int id)
    {
        await _tableRepository.DeleteAsync(id);
    }
}
