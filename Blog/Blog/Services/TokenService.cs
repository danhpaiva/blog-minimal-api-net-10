using Blog.Models;

namespace Blog.Services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        // Implementation for generating a token based on userId
        return $"token_for_{userId}";
    }
}
