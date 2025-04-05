using Microsoft.Extensions.Logging;

namespace SurveyBasket.Helpers;

public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
    {
        try
        {
            var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
            
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Email template not found at path: {templatePath}");
            }
            
            var streamReader = new StreamReader(templatePath);
            var body = streamReader.ReadToEnd();
            streamReader.Close();

            foreach (var item in templateModel)
            {
                if (!body.Contains(item.Key))
                {
                    Console.WriteLine($"Warning: Template placeholder '{item.Key}' not found in template");
                }
                body = body.Replace(item.Key, item.Value);
            }

            return body;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating email body: {ex.Message}");
            throw;
        }
    }
}