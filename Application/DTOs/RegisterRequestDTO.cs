namespace Application.DTOs
{
    public record RegisterRequestDTO(string FirstName,string LastName, string Email, string Password,string ConfirmPassword);
}
