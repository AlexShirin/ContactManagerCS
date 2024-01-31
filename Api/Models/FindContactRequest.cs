namespace ContactManagerCS.Models;

public class FindContactRequest
{
    public string Keyword { get; set; }

    public FindContactRequest(string keyword = "")
    {
        Keyword = keyword;
    }
}
