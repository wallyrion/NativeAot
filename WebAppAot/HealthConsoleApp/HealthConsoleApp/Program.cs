// See https://aka.ms/new-console-template for more information

class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting web application");
            
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:8080/todos");
            response.EnsureSuccessStatusCode();
            
            return 0;
        }
        catch
        {
            return 1;
        }
    }
}