using Bison.UserWebservice.Domain;
using System.Reflection;
using System.Text.Json;

namespace Bison.UserWebservice.Infrastructure;

/// <summary>
/// Dans le cadre de l'exercice, cette classe ne doit pas être touchée.
/// </summary>
public static class DoNotTouchClass
{
    /// <summary>
    /// This method simulate a call to the database
    /// DO NOT TOUCH
    /// </summary>
    /// <returns></returns>
    public static async IAsyncEnumerable<User> GetUserDataTask()
    {
        await Task.Delay(150); // Simulation d'un appel à la base de données
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Bison.UserWebservice.Infrastructure.db.json";

        await using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new Exception("Failed to load db");
        var userAsyncEnumerable = JsonSerializer.DeserializeAsyncEnumerable<User>(
            stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        await foreach (var user in userAsyncEnumerable)
        {
            if (user is not null)
            {
                yield return user;
            }
        }
    }
}
