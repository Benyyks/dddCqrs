using Bogus;
using Bison.CsvWebservice.Application.Common.Models;

namespace CsvWebService.Application.Tests
{
	public static class Helper
	{
		private static Faker<User> FakerRulesForUser()
		{
			return new Faker<User>()
				.CustomInstantiator(f => new User(
					Id: f.Random.Guid(),
					Name: f.Name.FullName(),
					Email: f.Internet.Email(),
					Phone: f.Phone.PhoneNumber(),
					Address: f.Address.StreetAddress(),
					PostalZip: f.Address.ZipCode(),
					Region: f.Address.State(),
					Country: f.Address.Country()));
		}

		public static List<User> GenerateUsers(int nb)
		{
			return FakerRulesForUser()
				.Generate(nb);
		}

		public static User GenerateUser(Guid id)
		{
			return new Faker<User>()
				.CustomInstantiator(f => new User(
					Id: id,
					Name: f.Name.FullName(),
					Email: f.Internet.Email(),
					Phone: f.Phone.PhoneNumber(),
					Address: f.Address.StreetAddress(),
					PostalZip: f.Address.ZipCode(),
					Region: f.Address.State(),
					Country: f.Address.Country()))
				.Generate();
		}
	}
}
