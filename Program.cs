namespace workApplications;

public class Customer
{
    public Customer(string name, string vat, DateTime creationDate, string address)
    {
        Name = name;
        Vat = vat;
        CreationDate = creationDate;
        Address = address;
    }
    public string Name;
    public string Vat;
    public DateTime CreationDate;
    public string Address;
}



internal class Program
{
    public static void Main(string[] args)
    {
        List<Customer> customers = ReadCustomersFromCsv("./customers.csv");
        string? command = "";
        string welcomeMessage = "\n Welcome to the system!\n" +
                                "1. Type 'insert' to insert a new customer\n" +
                                "2. Type 'edit' to edit an existing customer\n" +
                                "3. Type 'list' to list all customers\n" +
                                "4. Type 'delete' to delete an existing customer by name\n" +
                                "5. Type 'quit' to quit the system\n";
        
        while (command != "quit")
        {
            Console.WriteLine(welcomeMessage);
            command = Console.ReadLine();
            switch (command)
            {
                case "insert":
                    customers = InsertCustomerPrompt(customers);
                    break;
                case "edit":
                    customers = editCustomerPrompt(customers);
                    break;
                case "list":
                    listAllCustomers(customers);
                    break;
                case "delete":
                    customers = deleteCustomerPrompt(customers);
                    break;
                case "quit":
                    SaveCustomersToCsv("./customers.csv", customers);
                    return;
                default:
                    Console.WriteLine("This command does not exist!");
                    break;
            }
        }
    }

    private static List<Customer> ReadCustomersFromCsv(string customersCsv)
    {
        List<Customer> customers = new List<Customer>();
        string [] lines = File.ReadAllLines(customersCsv);
        foreach (string line in lines)
        {
            string [] properties = line.Split(';');
            customers.Add(new Customer(properties[0], properties[1], DateTime.Parse(properties[2]), properties[3]));
        }

        return customers;
    }
    private static void SaveCustomersToCsv(string customersCsv, List<Customer> customers)
    {
        string [] lines = new string[customers.Count];
        for (int i = 0; i < customers.Count; i++)
        {
            lines[i] = $"{customers[i].Name};{customers[i].Vat};{customers[i].CreationDate};{customers[i].Address}";
        }
        File.WriteAllLines(customersCsv, lines);
    }

    private static List<Customer> InsertCustomerPrompt(List<Customer> customers)
    {
        Console.Clear();
        Console.WriteLine("Input new customer info [name, vat, address] leaving whitespaces between each property:\n");
        try
        {
            string? input = Console.ReadLine();
            string [] inputProperties = input.Split(' ');
            if(customers.Any(el => el.Vat == inputProperties[1]))
                throw new Exception("Customer with this VAT already exists!");
            customers.Add(new Customer(inputProperties[0], inputProperties[1], DateTime.Now, inputProperties[2]));
            Console.WriteLine("Customer successfully inserted! Press any key to return...\n");
            Console.ReadLine();
            Console.Clear();
            return customers;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static List<Customer> editCustomerPrompt(List<Customer> customers)
    {
        Console.Clear();
        Console.WriteLine("Input vat of the customer you want to edit:\n");
        try
        {
            string? input = Console.ReadLine();
            if(!customers.Where(el => el.Vat == input).Any())
                throw new Exception("Customer with this VAT does not exist!");
            Console.WriteLine("Input new customer properties:\n");
            string newProperties = Console.ReadLine();
            string [] newPropertiesArray = newProperties.Split(' ');
            if(newPropertiesArray.Length != 3)
                throw new Exception("Invalid number of properties!");
            foreach (Customer cust in customers)
            {
                if (cust.Vat == input)
                {
                    cust.Name = newPropertiesArray[0];
                    cust.Vat = newPropertiesArray[1];
                    cust.Address = newPropertiesArray[2];
                }
            }
            Console.WriteLine("Customer successfully edited! Press any key to return...\n");
            Console.ReadLine();
            Console.Clear();
            return customers;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private static void listAllCustomers(List<Customer> customers)
    {
        Console.Clear();
        Console.WriteLine("Here are all the customers: \n");
        foreach (Customer cust in customers)
        {
            Console.WriteLine($"Name: {cust.Name}, VAT: {cust.Vat}, Date of Creation: {cust.CreationDate}, Address: {cust.Address}");
        }
        Console.WriteLine("Press any key to return...\n");
        Console.ReadLine();
        Console.Clear();
    }

    private static List<Customer> deleteCustomerPrompt(List<Customer> customers)
    {
        Console.Clear();
        Console.WriteLine("Select the customer you want to delete by vat:\n");
        string? input = Console.ReadLine();
        if(!customers.Where(el => el.Vat == input).Any())
            throw new Exception("Customer with this VAT does not exist!");
        customers = customers.Where(el => el.Vat != input).ToList();
        Console.WriteLine("Customer successfully deleted! Press any key to return...\n");
        Console.ReadLine();
        Console.Clear();
        return customers;
    }
}