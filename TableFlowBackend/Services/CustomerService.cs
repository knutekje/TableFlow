using TableFlowBackend.Models;

namespace TableFlowBackend.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableFlowBackend.Models;

public class CustomerService
{
    private readonly IRepository<Customer> _customerRepository;

    public CustomerService(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {id} not found.");

        return customer;
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Name))
            throw new ArgumentException("Customer name cannot be empty.");
        if (string.IsNullOrWhiteSpace(customer.Email))
            throw new ArgumentException("Customer email cannot be empty.");

        await _customerRepository.AddAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(customer.CustomerId);
        if (existingCustomer == null)
            throw new KeyNotFoundException($"Customer with ID {customer.CustomerId} not found.");

        await _customerRepository.UpdateAsync(customer);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {id} not found.");

        await _customerRepository.DeleteAsync(id);
    }
}
