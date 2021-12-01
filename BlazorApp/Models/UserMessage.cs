namespace BlazorApp.Models;

public class UserMessage
{
    public string User { get; set; }
    public string Message { get; set; }
    public bool IsCurrentUser { get; set; }
    public DateTime DateSent { get; set; }
}