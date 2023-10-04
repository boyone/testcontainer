namespace Repository.Provider;

public class Customer
{
    public Customer(string name)
    {
        Name = name;
    }

    public Customer(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public long Id { get; }
    public string Name { get; set; }
}