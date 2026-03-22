namespace SurveyBasket.Helpers
{
    public static class EmailBodyBuilder 
    {
        public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
        {
            var TemplatePath = $"{Directory.GetCurrentDirectory()}/TemplateEmail/{template}.html";
            var StreamReader = new StreamReader(TemplatePath);
            var body = StreamReader.ReadToEnd();
            StreamReader.Close();
            foreach( var item in templateModel )
            {
                body = body.Replace(item.Key, item.Value);
            }
            return body;
        }
    }
}
