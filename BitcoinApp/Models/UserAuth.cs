namespace BitcoinApp.Models

{
    /// <summary>
    /// Record é similar para class, com propriedades automaticas
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Email"></param>
    /// <param name="Password"></param>
    /// <param name="Roles"></param>
    public record UserAuth(Guid Id, string Name, string Email, string Password, string[] Roles);
}
