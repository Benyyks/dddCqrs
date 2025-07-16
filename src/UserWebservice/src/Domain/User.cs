namespace Bison.UserWebservice.Domain;

public record User(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string Address,
    string PostalZip,
    string Region,
    string Country
);