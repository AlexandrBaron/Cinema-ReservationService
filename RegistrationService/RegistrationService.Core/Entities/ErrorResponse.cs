namespace RegistrationService.Core.Entities;

public class ErrorResponse
{
    public string? Message { get; set; }  // Hlavní chybová zpráva
    public List<string>? Errors { get; set; } // Podrobnosti o chybách

    public ErrorResponse() { }

    public ErrorResponse(string message)
    {
        Message = message;
        Errors = new List<string>();
    }

    public ErrorResponse(string message, List<string> errors)
    {
        Message = message;
        Errors = errors;
    }
}
